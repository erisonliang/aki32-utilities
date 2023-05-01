using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Aki32Utilities.WinFormAppUtilities.Control
{
    public partial class ZoomableImage : UserControl
    {

        // ★★★★★★★★★★★★★★★ props

        public FileInfo ImageLocation
        {
            set => Image = new Bitmap(value.FullName);
        }
        public Bitmap Image
        {
            get => bmp;
            set => InitDrawImage(value);
        }


        // ★★★★★★★★★★★★★★★ fields

        /// <summary>
        /// 表示するBitmap
        /// </summary>
        private Bitmap bmp = null;
        /// <summary>
        /// 描画用Graphicsオブジェクト
        /// </summary>
        private Graphics g = null;
        /// <summary>
        /// マウスダウンフラグ
        /// </summary>
        private bool MouseDownFlg = false;
        /// <summary>
        /// マウスをクリックした位置の保持用
        /// </summary>
        private PointF OldPoint;
        /// <summary>
        /// アフィン変換行列
        /// </summary>
        private Matrix mat;


        // ★★★★★★★★★★★★★★★ inits

        public ZoomableImage()
        {
            InitializeComponent();

            MainPictureBox.MouseWheel += new MouseEventHandler(this.MainPictureBox_MouseWheel);

            // 強制的に実行。Graphicsオブジェクトの作成のため。
            MainPictureBox_SizeChanged(null, null);
        }


        // ★★★★★★★★★★★★★★★ events

        private void MainPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (g != null)
            {
                mat = g.Transform;
                g.Dispose();
                g = null;
            }

            // PictureBoxと同じ大きさのBitmapクラスを作成する。
            Bitmap bmpPicBox = new Bitmap(MainPictureBox.Width, MainPictureBox.Height);
            // 空のBitmapをPictureBoxのImageに指定する。
            MainPictureBox.Image = bmpPicBox;
            // Graphicsオブジェクトの作成(FromImageを使う)
            g = Graphics.FromImage(MainPictureBox.Image);
            // アフィン変換行列の設定
            if (mat != null)
            {
                g.Transform = mat;
            }

            // 補間モードの設定（このサンプルではNearestNeighborに設定）
            g.InterpolationMode
                = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            // 画像の描画
            RedrawImage();
        }

        private void MainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // 右ボタンがクリックされたとき
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // アフィン変換行列に単位行列を設定する
                mat.Reset();
                // 画像の描画
                RedrawImage();

                return;
            }
            // フォーカスの設定
            //（クリックしただけではMouseWheelイベントが有効にならない）
            MainPictureBox.Focus();
            // マウスをクリックした位置の記録
            OldPoint.X = e.X;
            OldPoint.Y = e.Y;
            // マウスダウンフラグ
            MouseDownFlg = true;
        }

        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // マウスをクリックしながら移動中のとき
            if (MouseDownFlg == true)
            {
                // 画像の移動
                mat.Translate(e.X - OldPoint.X, e.Y - OldPoint.Y,
                    System.Drawing.Drawing2D.MatrixOrder.Append);
                // 画像の描画
                RedrawImage();

                // ポインタ位置の保持
                OldPoint.X = e.X;
                OldPoint.Y = e.Y;
            }
        }

        private void MainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // マウスダウンフラグ
            MouseDownFlg = false;
        }

        private void MainPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // ポインタの位置→原点へ移動
            mat.Translate(-e.X, -e.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            if (e.Delta > 0)
            {
                // 拡大
                if (mat.Elements[0] < 100)  // X方向の倍率を代表してチェック
                {
                    mat.Scale(1.5f, 1.5f,
                        System.Drawing.Drawing2D.MatrixOrder.Append);
                }
            }
            else
            {
                // 縮小
                if (mat.Elements[0] > 0.01)  // X方向の倍率を代表してチェック
                {
                    mat.Scale(1.0f / 1.5f, 1.0f / 1.5f,
                        System.Drawing.Drawing2D.MatrixOrder.Append);
                }
            }
            // 原点→ポインタの位置へ移動(元の位置へ戻す)
            mat.Translate(e.X, e.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            // 画像の描画
            RedrawImage();
        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// Bitmap初期描画
        /// </summary>
        private void InitDrawImage(Bitmap bitmap)
        {
            bmp?.Dispose();
            bmp = bitmap;

            mat?.Dispose();
            mat = new Matrix();

            RedrawImage();
        }

        /// <summary>
        /// Bitmap再描画
        /// </summary>
        private void RedrawImage()
        {
            if (bmp == null)
                return;

            if (mat != null)
                g.Transform = mat;

            g.Clear(MainPictureBox.BackColor);
            g.DrawImage(bmp, 0, 0);

            MainPictureBox.Refresh();
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
