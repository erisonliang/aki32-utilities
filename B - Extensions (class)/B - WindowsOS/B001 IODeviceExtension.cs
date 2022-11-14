using System.Runtime.InteropServices;

namespace Aki32_Utilities.Extensions;

/// <summary>
/// Mainly, syntax sugar for device IO OS commands.
/// </summary>
public class IODeviceExtension
{

    // ★★★★★★★★★★★★★★★ Keyboard Input Events


    // キー操作、マウス操作をシミュレート(擬似的に操作する)
    //[DllImport("user32.dll")]
    //private extern static void SendInput(int nInputs, ref INPUT pInputs, int cbsize);


    // ★★★★★★★★★★★★★★★ Mouse Move Events

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void SetCursorPos(int X, int Y);
    public static void MouseMove(int X, int Y) => SetCursorPos(X, Y);

    // ★★★★★★★★★★★★★★★ Mouse Click Events 

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
    public static void MouseClick()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }


    // ★★★★★★★★★★★★★★★ 未整理 採用予定


    // 仮想キーコードをスキャンコードに変換
    [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
    private extern static int MapVirtualKey(int wCode, int wMapType);


    // ★★★★★★★★★★★★★★★ 


    //// マウスの右ボタンをクリックする
    //private void button1_Click(object sender, EventArgs e)
    //{
    //    // 自ウインドウを非表示(マウス操作対象のウィンドウへフォーカスを移動するため)
    //    this.Visible = false;

    //    // マウス操作実行用のデータ
    //    const int num = 3;
    //    INPUT[] inp = new INPUT[num];

    //    // (1)マウスカーソルを移動する(スクリーン座標でX座標=800ピクセル,Y=400ピクセルの位置)
    //    inp[0].type = INPUT_MOUSE;
    //    inp[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
    //    inp[0].mi.dx = 800 * (65535 / Screen.PrimaryScreen.Bounds.Width);
    //    inp[0].mi.dy = 400 * (65535 / Screen.PrimaryScreen.Bounds.Height);
    //    inp[0].mi.mouseData = 0;
    //    inp[0].mi.dwExtraInfo = 0;
    //    inp[0].mi.time = 0;

    //    // (2)マウスの右ボタンを押す
    //    inp[1].type = INPUT_MOUSE;
    //    inp[1].mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
    //    inp[1].mi.dx = 0;
    //    inp[1].mi.dy = 0;
    //    inp[1].mi.mouseData = 0;
    //    inp[1].mi.dwExtraInfo = 0;
    //    inp[1].mi.time = 0;

    //    // (3)マウスの右ボタンを離す
    //    inp[2].type = INPUT_MOUSE;
    //    inp[2].mi.dwFlags = MOUSEEVENTF_RIGHTUP;
    //    inp[2].mi.dx = 0;
    //    inp[2].mi.dy = 0;
    //    inp[2].mi.mouseData = 0;
    //    inp[2].mi.dwExtraInfo = 0;
    //    inp[2].mi.time = 0;

    //    // マウス操作実行
    //    SendInput(num, ref inp[0], Marshal.SizeOf(inp[0]));

    //    // 1000ミリ秒スリープ
    //    System.Threading.Thread.Sleep(1000);

    //    // 自ウインドウを表示
    //    this.Visible = true;
    //}

    //// マウスの左ボタンをダブルクリックする
    //private void button2_Click(object sender, EventArgs e)
    //{
    //    // 自ウインドウを非表示(マウス操作対象のウィンドウへフォーカスを移動するため)
    //    this.Visible = false;

    //    // マウス操作実行用のデータ
    //    const int num = 5;
    //    INPUT[] inp = new INPUT[num];

    //    // (1)マウスカーソルを移動する(スクリーン座標でX座標=800ピクセル,Y=400ピクセルの位置)
    //    inp[0].type = INPUT_MOUSE;
    //    inp[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
    //    inp[0].mi.dx = 800 * (65535 / Screen.PrimaryScreen.Bounds.Width);
    //    inp[0].mi.dy = 400 * (65535 / Screen.PrimaryScreen.Bounds.Height);
    //    inp[0].mi.mouseData = 0;
    //    inp[0].mi.dwExtraInfo = 0;
    //    inp[0].mi.time = 0;

    //    // (2)マウスの左ボタンを押す
    //    inp[1].type = INPUT_MOUSE;
    //    inp[1].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
    //    inp[1].mi.dx = 0;
    //    inp[1].mi.dy = 0;
    //    inp[1].mi.mouseData = 0;
    //    inp[1].mi.dwExtraInfo = 0;
    //    inp[1].mi.time = 0;

    //    // (3)マウスの左ボタンを離す
    //    inp[2].type = INPUT_MOUSE;
    //    inp[2].mi.dwFlags = MOUSEEVENTF_LEFTUP;
    //    inp[2].mi.dx = 0;
    //    inp[2].mi.dy = 0;
    //    inp[2].mi.mouseData = 0;
    //    inp[2].mi.dwExtraInfo = 0;
    //    inp[2].mi.time = 0;

    //    // マウス操作実行
    //    SendInput(num, ref inp[0], Marshal.SizeOf(inp[0]));

    //    // 1000ミリ秒スリープ
    //    System.Threading.Thread.Sleep(1000);

    //    // 自ウインドウを表示
    //    this.Visible = true;
    //}

    //// マウスの中央ボタンをクリックする
    //private void button3_Click(object sender, EventArgs e)
    //{
    //    // 自ウインドウを非表示(マウス操作対象のウィンドウへフォーカスを移動するため)
    //    this.Visible = false;

    //    // マウス操作実行用のデータ
    //    const int num = 3;
    //    INPUT[] inp = new INPUT[num];

    //    // (1)マウスカーソルを移動する(スクリーン座標でX座標=800ピクセル,Y=400ピクセルの位置)
    //    inp[0].type = INPUT_MOUSE;
    //    inp[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
    //    inp[0].mi.dx = 800 * (65535 / Screen.PrimaryScreen.Bounds.Width);
    //    inp[0].mi.dy = 400 * (65535 / Screen.PrimaryScreen.Bounds.Height);
    //    inp[0].mi.mouseData = 0;
    //    inp[0].mi.dwExtraInfo = 0;
    //    inp[0].mi.time = 0;

    //    // (2)マウスの中央ボタンを押す
    //    inp[1].type = INPUT_MOUSE;
    //    inp[1].mi.dwFlags = MOUSEEVENTF_MIDDLEDOWN;
    //    inp[1].mi.dx = 0;
    //    inp[1].mi.dy = 0;
    //    inp[1].mi.mouseData = 0;
    //    inp[1].mi.dwExtraInfo = 0;
    //    inp[1].mi.time = 0;

    //    // (3)マウスの中央ボタンを離す
    //    inp[2].type = INPUT_MOUSE;
    //    inp[2].mi.dwFlags = MOUSEEVENTF_MIDDLEUP;
    //    inp[2].mi.dx = 0;
    //    inp[2].mi.dy = 0;
    //    inp[2].mi.mouseData = 0;
    //    inp[2].mi.dwExtraInfo = 0;
    //    inp[2].mi.time = 0;

    //    // マウス操作実行
    //    SendInput(num, ref inp[0], Marshal.SizeOf(inp[0]));

    //    // 1000ミリ秒スリープ
    //    System.Threading.Thread.Sleep(1000);

    //    // 自ウインドウを表示
    //    this.Visible = true;
    //}

    //// マウスホイールを回転する
    //private void button4_Click(object sender, EventArgs e)
    //{
    //    // 自ウインドウを非表示(マウス操作対象のウィンドウへフォーカスを移動するため)
    //    this.Visible = false;

    //    // マウス操作実行用のデータ
    //    const int num = 2;
    //    INPUT[] inp = new INPUT[num];

    //    // (1)マウスカーソルを移動する(スクリーン座標でX座標=800ピクセル,Y=400ピクセルの位置)
    //    inp[0].type = INPUT_MOUSE;
    //    inp[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
    //    inp[0].mi.dx = 800 * (65535 / Screen.PrimaryScreen.Bounds.Width);
    //    inp[0].mi.dy = 400 * (65535 / Screen.PrimaryScreen.Bounds.Height);
    //    inp[0].mi.mouseData = 0;
    //    inp[0].mi.dwExtraInfo = 0;
    //    inp[0].mi.time = 0;

    //    // (2)マウスホイールを前方(近づく方向)へ回転する
    //    inp[1].type = INPUT_MOUSE;
    //    inp[1].mi.dwFlags = MOUSEEVENTF_WHEEL;
    //    inp[1].mi.dx = 0;
    //    inp[1].mi.dy = 0;
    //    inp[1].mi.mouseData = -1 * WHEEL_DELTA;
    //    inp[1].mi.dwExtraInfo = 0;
    //    inp[1].mi.time = 0;

    //    // マウス操作実行
    //    SendInput(num, ref inp[0], Marshal.SizeOf(inp[0]));

    //    // 1000ミリ秒スリープ
    //    System.Threading.Thread.Sleep(1000);

    //    // マウス操作実行用のデータ
    //    const int nu2 = 1;
    //    INPUT[] in2 = new INPUT[num];

    //    // (1)マウスホイールを後方(離れる方向)へ回転する
    //    in2[0].type = INPUT_MOUSE;
    //    in2[0].mi.dwFlags = MOUSEEVENTF_WHEEL;
    //    in2[0].mi.dx = 0;
    //    in2[0].mi.dy = 0;
    //    in2[0].mi.mouseData = +1 * WHEEL_DELTA;
    //    in2[0].mi.dwExtraInfo = 0;
    //    in2[0].mi.time = 0;

    //    // マウス操作実行
    //    SendInput(nu2, ref in2[0], Marshal.SizeOf(in2[0]));

    //    // 1000ミリ秒スリープ
    //    System.Threading.Thread.Sleep(1000);

    //    // 自ウインドウを表示
    //    this.Visible = true;
    //}

    //// キーボードを押す
    //private void button5_Click(object sender, EventArgs e)
    //{
    //    // 自ウインドウを非表示(キーボード操作対象のウィンドウへフォーカスを移動するため)
    //    this.Visible = false;

    //    // キーボード操作実行用のデータ
    //    const int num = 4;
    //    INPUT[] inp = new INPUT[num];

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

    //    // キーボード操作実行
    //    SendInput(num, ref inp[0], Marshal.SizeOf(inp[0]));

    //    // 1000ミリ秒スリープ
    //    System.Threading.Thread.Sleep(1000);

    //    // 自ウインドウを表示
    //    this.Visible = true;
    //}


}
