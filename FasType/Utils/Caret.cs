﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FasType.Utils
{
    public static class Caret
    {

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);
        [StructLayout(LayoutKind.Sequential)]
        public struct GUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public Rectangle rcCaret;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ClientToScreen(IntPtr hwnd, ref Point lpRect);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ScreenToClient(IntPtr hwnd, ref Point lpRect);

        public static System.Drawing.Point GetCaretPos()
        {
            //int hwnd = 0;
            GUITHREADINFO guiti = new();
            guiti.cbSize = Marshal.SizeOf(guiti);

            var b1 = GetGUIThreadInfo(0, ref guiti);

            Point p = new(guiti.rcCaret.Left + 2, guiti.rcCaret.Top + 25);
            //p.Offset(guiti.rcCaret.Width, guiti.rcCaret.Height);
            Debug.WriteLine($"Caret Inside, (bool): ({p.X}, {p.Y}) , ({b1})");

            var b2 = ClientToScreen(guiti.hwndCaret, ref p);
            //var b2 = GetWindowRect(guiti.hwndActive, out RECT rect);
            //p.Offset(rect.Left, rect.Top);
            Debug.WriteLine($"Caret Outside, (bool): ({p.X}, {p.Y}), ({b2})");

            //System.Drawing.Point dp = new((int)p.X, (int)p.Y);
            return p;
        }

        public static System.Drawing.Rectangle GetWorkingArea(System.Drawing.Point dp)
        {
            var screen = System.Windows.Forms.Screen.FromPoint(dp);
            var wa = System.Windows.Forms.Screen.GetWorkingArea(dp);

            Debug.WriteLine($"Caret Screen Name (Primary): {screen.DeviceName} ({screen.Primary})");
            Debug.WriteLine($"Working Area: ({wa.Left}, {wa.Top}) {wa.Width}x{wa.Height}");

            return wa;
        }

    }
}