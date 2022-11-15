using System.Drawing;
using System.Runtime.InteropServices;

namespace Aki32_Utilities.Extensions;

/// <summary>
/// Mainly, syntax sugar of device IO OS commands.
/// </summary>
public class IODeviceExtension
{

    // ★★★★★★★★★★★★★★★ IO Device Input Events Base

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

    private const int VIRTUAL_KEY_SHIFT = 0x10;
    private const int VIRTUAL_KEY_CTRL = 0x11;
    private const int VIRTUAL_KEY_ALT = 0x12;

    [DllImport("user32.dll")]
    public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    /// <summary>
    /// Virtual Keyboard Input.
    /// </summary>
    /// <param name="sendingKey">sending key code</param>
    public static void SendKey(byte sendingKey, bool withCtrl = false, bool withShift = false, bool withAlt = false)
    {
        // for safety
        Thread.Sleep(10);

        var button = IODeviceButton.Keyboard;

        if (withCtrl) _ = keybd_event(VIRTUAL_KEY_CTRL, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Down), (UIntPtr)0);
        if (withShift) _ = keybd_event(VIRTUAL_KEY_SHIFT, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Down), (UIntPtr)0);
        if (withAlt) _ = keybd_event(VIRTUAL_KEY_ALT, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Down), (UIntPtr)0);

        // for safety
        if (withCtrl | withShift | withAlt) Thread.Sleep(10);

        _ = keybd_event(sendingKey, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Down), (UIntPtr)0);
        _ = keybd_event(sendingKey, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Up), (UIntPtr)0);

        if (withCtrl) _ = keybd_event(VIRTUAL_KEY_CTRL, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Up), (UIntPtr)0);
        if (withShift) _ = keybd_event(VIRTUAL_KEY_SHIFT, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Up), (UIntPtr)0);
        if (withAlt) _ = keybd_event(VIRTUAL_KEY_ALT, 0, (uint)GetInputEventCode(button, IODeviceButtonEvent.Up), (UIntPtr)0);

    }
    public static void SendKey(ConsoleKey sendingKey, bool withCtrl = false, bool withShift = false, bool withAlt = false)
    {
        SendKey((byte)sendingKey, withCtrl, withShift, withAlt);
    }
    public static void SendKey(char sendingKey, bool withCtrl = false, bool withShift = false, bool withAlt = false)
    {
        withShift |= char.IsUpper(sendingKey);
        sendingKey = CorrectKey(sendingKey);
        SendKey((byte)sendingKey, withCtrl, withShift, withAlt);
    }
    public static void SendKeys(string sendingKeys)
    {
        foreach (var sendingKey in sendingKeys)
            SendKey(sendingKey);
    }
    private static char CorrectKey(char c)
    {
        if ('0' <= c && c <= '9') return c;

        if ('A' <= c && c <= 'Z') return c;

        if (c == ' ') return c;

        if (c == '\t') return c;

        if ('a' <= c && c <= 'z') return char.ToUpperInvariant(c);

        // それら以外の場合はＮＧ
        return ' ';
    }


    // ★★★★★★★★★★★★★★★ Get Mouse Cursor Position

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);
    public static PointF GetMouseCursorPosition()
    {
        GetCursorPos(out var p);
        return new PointF(p.X, p.Y);
    }
    public struct POINT
    {
        public int X;
        public int Y;
    }


    // ★★★★★★★★★★★★★★★ Move Mouse Cursor

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void SetCursorPos(int X, int Y);
    public static void MouseCursorMoveTo(int x, int y) => SetCursorPos(x, y);


    // ★★★★★★★★★★★★★★★ Mouse Input Events 

    private const int WHEEL_DELTA = 120;

    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    public static void MouseClick(IODeviceButton button = IODeviceButton.MouseLeft)
    {
        mouse_event(GetInputEventCode(button, IODeviceButtonEvent.Down), 0, 0, 0, 0);
        mouse_event(GetInputEventCode(button, IODeviceButtonEvent.Up), 0, 0, 0, 0);
    }
    public static void MouseDoubleClick(IODeviceButton button = IODeviceButton.MouseLeft)
    {
        MouseClick(button);
        MouseClick(button);
    }
    public static void MouseWheelScroll(bool toUp)
    {
        var direction = WHEEL_DELTA * (toUp ? 1 : -1);
        mouse_event(GetInputEventCode(IODeviceButton.MouseMiddle, IODeviceButtonEvent.WHEEL), 0, 0, direction, 0);
    }


    // ★★★★★★★★★★★★★★★ 

}
