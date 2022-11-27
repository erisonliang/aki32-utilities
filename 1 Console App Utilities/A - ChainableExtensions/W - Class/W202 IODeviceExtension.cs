using System.Drawing;
using System.Runtime.InteropServices;

namespace Aki32_Utilities.ChainableExtensions;

/// <summary>
/// Mainly, syntax sugar of device IO OS commands.
/// </summary>
public static class IODeviceExtension
{
    // ★★★★★★★★★★★★★★★ for use

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

    public static Point GetMouseCursorPosition()
    {
        GetCursorPos(out var p);
        return new Point(p.X, p.Y);
    }

    public static void MouseCursorMoveTo(int x, int y) => SetCursorPos(x, y);
    public static void MouseCursorMoveTo(Point p) => SetCursorPos(p.X, p.Y);

    public static void MouseClick(IODeviceButton button = IODeviceButton.MouseLeft)
    {
        mouse_event(GetInputEventCode(button, IODeviceButtonEvent.Down), 0, 0, 0, 0);
        mouse_event(GetInputEventCode(button, IODeviceButtonEvent.Up), 0, 0, 0, 0);
    }
    public static void MouseWheelScroll(bool toUp, int times = 1)
    {
        var direction = WHEEL_DELTA * times * (toUp ? 1 : -1);
        mouse_event(GetInputEventCode(IODeviceButton.MouseMiddle, IODeviceButtonEvent.WHEEL), 0, 0, direction, 0);
    }


    // ★★★★★★★★★★★★★★★ for use (applied)

    public static void MouseDoubleClick(IODeviceButton button = IODeviceButton.MouseLeft)
    {
        MouseClick(button);
        MouseClick(button);
    }

    public static Point GetMouseCursorPositionConversationally(string targetPointName = null, ConsoleKey terminateKey = ConsoleKey.Escape)
    {
        ConsoleExtension.WriteLineWithColor($"\r\nMove cursor to {targetPointName ?? "target"} and press Enter. Press {terminateKey} to redo. Make sure this window is focused!", ConsoleColor.Blue);

        var lastPosition = new Point(0, 0);
        Console.CursorVisible = false;
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                    break;
                if (key.Key == terminateKey)
                {
                    ConsoleExtension.ClearCurrentConsoleLine();
                    ConsoleExtension.WriteLineWithColor($"x Terminate key ({terminateKey}) was pressed", ConsoleColor.Red);
                    throw new OperationCanceledException($"Terminate key ({terminateKey}) was pressed");
                }
            }

            lastPosition = GetMouseCursorPosition();
            ConsoleExtension.ClearCurrentConsoleLine();
            Console.Write(lastPosition);
            Thread.Sleep(10);
        }
        Console.CursorVisible = true;
        Console.WriteLine();
        return lastPosition;
    }
    public static Point[] GetMouseCursorPositionConversationallyForMany(string[] targetPointNames, ConsoleKey terminateKey = ConsoleKey.Escape, bool consoleWriteResults = false)
    {
        var points = new Point[targetPointNames.Length];

        for (int i = 0; i < targetPointNames.Length; i++)
        {
            try
            {
                points[i] = GetMouseCursorPositionConversationally(targetPointNames[i], terminateKey);
            }
            catch (OperationCanceledException)
            {
                i -= 2;
                i = Math.Max(-1, i);
                continue;
            }
        }

        if (consoleWriteResults)
        {
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < points.Length; i++)
                Console.WriteLine($"{targetPointNames[i]}:{points[i]}");

        }

        Console.WriteLine();
        Console.WriteLine();

        return points;
    }

    public static void MouseCursorMoveAndClick(int x, int y, IODeviceButton button = IODeviceButton.MouseLeft)
    {
        MouseCursorMoveAndClick(new Point(x, y), button);
    }
    public static void MouseCursorMoveAndClick(Point p, IODeviceButton button = IODeviceButton.MouseLeft)
    {
        MouseCursorMoveTo(p);
        Thread.Sleep(10);
        MouseClick(button);
    }


    // ★★★★★★★★★★★★★★★ background processes

    private const int VIRTUAL_KEY_SHIFT = 0x10;
    private const int VIRTUAL_KEY_CTRL = 0x11;
    private const int VIRTUAL_KEY_ALT = 0x12;
    private const int WHEEL_DELTA = 120;
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


    [DllImport("user32.dll")]
    public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
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


    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);
    public struct POINT
    {
        public int X;
        public int Y;
    }


    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void SetCursorPos(int X, int Y);


    [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);


    // ★★★★★★★★★★★★★★★ 

}
