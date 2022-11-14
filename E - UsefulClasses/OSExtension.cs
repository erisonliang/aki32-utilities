using System.Runtime.InteropServices;

namespace Aki32_Utilities.Extensions;
public class Device
{
    // ★★★★★★★★★★★★★★★ Cursor Move Events

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void SetCursorPos(int X, int Y);
    public static void MoveCursor(int X, int Y) => SetCursorPos(X, Y);

    // ★★★★★★★★★★★★★★★ Cursor Click Events 

    #region consts
    private const int INPUT_MOUSE = 0;                  // マウスイベント
    private const int INPUT_KEYBOARD = 1;               // キーボードイベント
    private const int INPUT_HARDWARE = 2;               // ハードウェアイベント

    private const int MOUSEEVENTF_MOVE = 0x1;           // マウスを移動する
    private const int MOUSEEVENTF_ABSOLUTE = 0x8000;    // 絶対座標指定
    private const int MOUSEEVENTF_LEFTDOWN = 0x2;       // 左　ボタンを押す
    private const int MOUSEEVENTF_LEFTUP = 0x4;         // 左　ボタンを離す
    private const int MOUSEEVENTF_RIGHTDOWN = 0x8;      // 右　ボタンを押す
    private const int MOUSEEVENTF_RIGHTUP = 0x10;       // 右　ボタンを離す
    private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;    // 中央ボタンを押す
    private const int MOUSEEVENTF_MIDDLEUP = 0x40;      // 中央ボタンを離す
    private const int MOUSEEVENTF_WHEEL = 0x800;        // ホイールを回転する
    private const int WHEEL_DELTA = 120;                // ホイール回転値

    private const int KEYEVENTF_KEYDOWN = 0x0;          // キーを押す
    private const int KEYEVENTF_KEYUP = 0x2;            // キーを離す
    private const int KEYEVENTF_EXTENDEDKEY = 0x1;      // 拡張コード
    private const int VK_SHIFT = 0x10;                  // SHIFTキー
    #endregion

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    public static void MoveClick()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    // ★★★★★★★★★★★★★★★ 

}
