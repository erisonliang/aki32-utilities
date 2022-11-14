using System.Runtime.InteropServices;


using System.IO;

namespace Aki32_Utilities.Extensions;

/// <summary>
/// Mainly, syntax sugar of device IO OS commands.
/// </summary>
public class IODeviceExtension
{

    // ★★★★★★★★★★★★★★★ IO Device Input Events

    #region consts
    private const int INPUT_MOUSE = 0;                  // マウスイベント
    private const int INPUT_KEYBOARD = 1;               // キーボードイベント
    private const int INPUT_HARDWARE = 2;               // ハードウェアイベント

    private const int WHEEL_DELTA = 120;                // ホイール回転値

    private const int KEYEVENTF_EXTENDEDKEY = 0x1;      // 拡張コード
    private const int VK_SHIFT = 0x10;                  // SHIFTキー
    #endregion

    private static int GetInputEventCode(IODeviceButton handlingButton, IODeviceButtonEvent handlingEvent)
    {
        switch (handlingButton)
        {
            case IODeviceButton.MouseLeft:
                {
                    switch (handlingEvent)
                    {
                        case IODeviceButtonEvent.Down:
                            return 0x2;
                        case IODeviceButtonEvent.Up:
                            return 0x4;
                    }
                }
                break;
            case IODeviceButton.MouseMiddle:
                {
                    switch (handlingEvent)
                    {
                        case IODeviceButtonEvent.Down:
                            return 0x20;
                        case IODeviceButtonEvent.Up:
                            return 0x40;
                        case IODeviceButtonEvent.WHEEL:
                            return 0x800;
                    }
                }
                break;
            case IODeviceButton.MouseRight:
                {
                    switch (handlingEvent)
                    {
                        case IODeviceButtonEvent.Down:
                            return 0x8;
                        case IODeviceButtonEvent.Up:
                            return 0x10;
                    }
                }
                break;
            case IODeviceButton.Keyboard:
                {
                    switch (handlingEvent)
                    {
                        case IODeviceButtonEvent.Down:
                            return 0x0;
                        case IODeviceButtonEvent.Up:
                            return 0x2;
                    }
                }
                break;
        }

        throw new NotImplementedException();
    }

    public enum IODeviceButton
    {
        MouseLeft,
        MouseMiddle,
        MouseRight,
        Keyboard,
    }
    public enum IODeviceButtonEvent
    {
        Down,
        Up,
        WHEEL,
    }


    // ★★★★★★★★★★★★★★★ Keyboard Input Events

    // キー操作、マウス操作をシミュレート(擬似的に操作する)
    [DllImport("user32.dll")]
    private extern static void SendInput(int nInputs, ref INPUT pInputs, int cbsize);



    // ★★★★★★★★★★★★★★★ Mouse Move Events

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void SetCursorPos(int X, int Y);
    public static void MouseMove(int x, int y) => SetCursorPos(x, y);



    // ★★★★★★★★★★★★★★★ Mouse Click Events 

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    public static void MouseClick(IODeviceButton button = IODeviceButton.MouseLeft)
    {
        mouse_event(GetInputEventCode(button, IODeviceButtonEvent.Down), 0, 0, 0, 0);
        mouse_event(GetInputEventCode(button, IODeviceButtonEvent.Up), 0, 0, 0, 0);
    }
    public static void MouseDoubleClick()
    {
        MouseClick();
        MouseClick();
    }


    // ★★★★★★★★★★★★★★★ 未整理 採用予定


    // 仮想キーコードをスキャンコードに変換
    [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
    private extern static int MapVirtualKey(int wCode, int wMapType);




    // ★★★★★★★★★★★★★★★ 







    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public int dwFlags;
        public int time;
        public int dwExtraInfo;
    };

    // キーボードイベント(keybd_eventの引数と同様のデータ)
    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public short wVk;
        public short wScan;
        public int dwFlags;
        public int time;
        public int dwExtraInfo;
    };

    // ハードウェアイベント
    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    };

    // 各種イベント(SendInputの引数データ)
    [StructLayout(LayoutKind.Explicit)]
    private struct INPUT
    {
        [FieldOffset(0)] public int type;
        [FieldOffset(4)] public MOUSEINPUT mi;
        [FieldOffset(4)] public KEYBDINPUT ki;
        [FieldOffset(4)] public HARDWAREINPUT hi;
    };

    //// マウスホイールを回転する
    //private void button4_Click(object sender, EventArgs e)
    //{
    //    // (2)マウスホイールを前方(近づく方向)へ回転する
    //    inp[1].type = INPUT_MOUSE;
    //    inp[1].mi.dwFlags = MOUSEEVENTF_WHEEL;
    //    inp[1].mi.dx = 0;
    //    inp[1].mi.dy = 0;
    //    inp[1].mi.mouseData = -1 * WHEEL_DELTA;
    //    inp[1].mi.dwExtraInfo = 0;
    //    inp[1].mi.time = 0;

    //    // (1)マウスホイールを後方(離れる方向)へ回転する
    //    in2[0].type = INPUT_MOUSE;
    //    in2[0].mi.dwFlags = MOUSEEVENTF_WHEEL;
    //    in2[0].mi.dx = 0;
    //    in2[0].mi.dy = 0;
    //    in2[0].mi.mouseData = +1 * WHEEL_DELTA;
    //    in2[0].mi.dwExtraInfo = 0;
    //    in2[0].mi.time = 0;
    //}

    //// キーボードを押す
    //private void button5_Click(object sender, EventArgs e)
    //{
    //    // (1)キーボード(SHIFT)を押す
    //    inp[0].type = INPUT_KEYBOARD;
    //    inp[0].ki.wVk = VK_SHIFT;
    //    inp[0].ki.wScan = (short)MapVirtualKey(inp[0].ki.wVk, 0);
    //    inp[0].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN;
    //    inp[0].ki.dwExtraInfo = 0;
    //    inp[0].ki.time = 0;

    //    // (2)キーボード(A)を押す
    //    inp[1].type = INPUT_KEYBOARD;
    //    inp[1].ki.wVk = (short)Keys.A;
    //    inp[1].ki.wScan = (short)MapVirtualKey(inp[1].ki.wVk, 0);
    //    inp[1].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN;
    //    inp[1].ki.dwExtraInfo = 0;
    //    inp[1].ki.time = 0;

    //    // (3)キーボード(A)を離す
    //    inp[2].type = INPUT_KEYBOARD;
    //    inp[2].ki.wVk = (short)Keys.A;
    //    inp[2].ki.wScan = (short)MapVirtualKey(inp[2].ki.wVk, 0);
    //    inp[2].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
    //    inp[2].ki.dwExtraInfo = 0;
    //    inp[2].ki.time = 0;

    //    // (4)キーボード(SHIFT)を離す
    //    inp[3].type = INPUT_KEYBOARD;
    //    inp[3].ki.wVk = VK_SHIFT;
    //    inp[3].ki.wScan = (short)MapVirtualKey(inp[3].ki.wVk, 0);
    //    inp[3].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
    //    inp[3].ki.dwExtraInfo = 0;
    //    inp[3].ki.time = 0;

    //}









}
