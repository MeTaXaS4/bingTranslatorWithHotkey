﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    /// <summary>
    /// κλάση που χρησιμοποιείται για να γίνει "εικονικό" πάτημα κουμπιών του πληκτρολογίου, για την αντιγραφή του επιλεγμένου κειμένου.
    /// </summary>
    class Keyboard
    {
        public static string[] keys = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
            "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", 
            "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" };

        /// <summary>
        /// συνάρτηση που κάνει το "εικονικό" πάτημα κουμπίών
        /// </summary>
        public static void SimulateKeyStroke(char key, bool ctrl , bool alt , bool shift )
        {
            List<ushort> keys = new List<ushort>();

            if (ctrl)
                keys.Add(VK_CONTROL);

            if (alt)
                keys.Add(VK_MENU);

            if (shift)
                keys.Add(VK_SHIFT);

            keys.Add(char.ToUpper(key));

            INPUT input = new INPUT();
            input.type = INPUT_KEYBOARD;
            int inputSize = Marshal.SizeOf(input);

            for (int i = 0; i < keys.Count; ++i)
            {
                input.mkhi.ki.wVk = keys[i];

                bool isKeyDown = (GetAsyncKeyState(keys[i]) & 0x10000) != 0;

                if (!isKeyDown)
                    SendInput(1, ref input, inputSize);
            }

            input.mkhi.ki.dwFlags = KEYEVENTF_KEYUP;
            for (int i = keys.Count - 1; i >= 0; --i)
            {
                input.mkhi.ki.wVk = keys[i];
                SendInput(1, ref input, inputSize);
            }
        }

        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(ushort vKey);

        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        struct INPUT
        {
            public int type;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }

        const int INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_KEYUP = 0x0002;

        const ushort VK_SHIFT = 0x10;
        const ushort VK_CONTROL = 0x11;
        const ushort VK_MENU = 0x12;
    }
}
