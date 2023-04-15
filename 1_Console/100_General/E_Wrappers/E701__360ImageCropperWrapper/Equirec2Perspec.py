import cv2
import numpy as np


def xyz2lonlat(xyz):
    atan2 = np.arctan2
    asin = np.arcsin

    norm = np.linalg.norm(xyz, axis=-1, keepdims=True)
    xyz_norm = xyz / norm
    x = xyz_norm[..., 0:1]
    y = xyz_norm[..., 1:2]
    z = xyz_norm[..., 2:]

    lon = atan2(x, z)
    lat = asin(y)
    lst = [lon, lat]

    out = np.concatenate(lst, axis=-1)
    return out


def lonlat2XY(lonlat, shape):
    X = (lonlat[..., 0:1] / (2 * np.pi) + 0.5) * (shape[1] - 1)
    Y = (lonlat[..., 1:] / (np.pi) + 0.5) * (shape[0] - 1)
    lst = [X, Y]
    out = np.concatenate(lst, axis=-1)

    return out


def left2right(extrinsic_left: np.ndarray) -> np.ndarray:
    extrinsic_right = np.empty((4, 4))

    extrinsic_right[0, 0] = extrinsic_left[0, 0]
    extrinsic_right[0, 1] = extrinsic_left[2, 0]
    extrinsic_right[0, 2] = extrinsic_left[1, 0]
    extrinsic_right[1, 0] = extrinsic_left[0, 2]
    extrinsic_right[1, 1] = extrinsic_left[2, 2]
    extrinsic_right[1, 2] = extrinsic_left[1, 2]
    extrinsic_right[2, 0] = extrinsic_left[0, 1]
    extrinsic_right[2, 1] = extrinsic_left[2, 1]
    extrinsic_right[2, 2] = extrinsic_left[1, 1]

    extrinsic_right[:, 1] = -extrinsic_right[:, 1]
    extrinsic_right[1, :] = -extrinsic_right[1, :]

    return extrinsic_right


class Equirectangular:
    def __init__(self, img_name):
        self._img = cv2.imread(img_name, cv2.IMREAD_COLOR)
        [self._height, self._width, _] = self._img.shape

    def GetPerspectiveByMat(self, intrinsic: np.ndarray, extrinsic_left: np.ndarray) -> np.ndarray:

        extrinsic_right = left2right(extrinsic_left)

        # etract info, and reconstruct for safety
        f = intrinsic[0, 0]
        w = intrinsic[0, 2]
        h = intrinsic[1, 2]
        K = np.array(
            [
                [f, 0, w/2],
                [0, f, h/2],
                [0, 0,   1],
            ],
            np.float32,
        )
        
        K_inv = np.linalg.inv(K)

        x = np.arange(w)
        y = np.arange(h)
        x, y = np.meshgrid(x, y)
        z = np.ones_like(x)
        xyz = np.concatenate([x[..., None], y[..., None], z[..., None]], axis=-1)
        xyz = xyz @ K_inv.T

        THETA = 0
        PHI = -90

        y_axis = np.array([0.0, 1.0, 0.0], np.float32)
        x_axis = np.array([1.0, 0.0, 0.0], np.float32)
        R1, _ = cv2.Rodrigues(y_axis * np.radians(THETA))
        R2, _ = cv2.Rodrigues(np.dot(R1, x_axis) * np.radians(PHI))
        R = R2 @ R1

        calibrated_extrinsic = np.dot(R, extrinsic_right[:3, :3].T)

        xyz = xyz @ calibrated_extrinsic
        lonlat = xyz2lonlat(xyz)
        XY = lonlat2XY(lonlat, shape=self._img.shape).astype(np.float32)
        persp = cv2.remap(
            self._img,
            XY[..., 0],
            XY[..., 1],
            cv2.INTER_CUBIC,
            borderMode=cv2.BORDER_WRAP,
        )

        return persp
