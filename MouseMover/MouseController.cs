using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MouseMover {
    static class MouseController {
        [DllImport("user32.dll")]
        extern static uint SendInput(
            uint nInputs,
            INPUT[] pInputs,
            int cbSize
            );

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT {
            public int dx;
            public int dy;
            public int mouseData;
            public MouseEvent dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        private enum MouseEvent {
            MOVED = 0x0001,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            WHEEL = 0x0800,
            XDOWN = 0x0100,
            XUP = 0x0200,
            ABSOLUTE = 0x8000
        }

        private const int SCREEN_LENGTH = 0x10000;
        private const int WHEEL_DELTA = 120;

        public static void LeftClick() {
            var inputs = new INPUT[2];
            inputs[0].mi.dwFlags = MouseEvent.LEFTDOWN;
            inputs[1].mi.dwFlags = MouseEvent.LEFTUP;

            SendInput(2, inputs, Marshal.SizeOf(inputs[0]));
        }

        public static void RightClick() {
            var inputs = new INPUT[2];
            inputs[0].mi.dwFlags = MouseEvent.RIGHTDOWN;
            inputs[1].mi.dwFlags = MouseEvent.RIGHTUP;

            SendInput(2, inputs, Marshal.SizeOf(inputs[0]));
        }

        public static void Wheel(int amount) {
            var inputs = new INPUT[1];
            inputs[0].mi.dwFlags = MouseEvent.WHEEL | MouseEvent.ABSOLUTE;
            inputs[0].mi.mouseData = - amount * WHEEL_DELTA;

            SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
        }
    }
}
