﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace FasType.LLKeyboardListener
{
    public class LowLevelKeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private readonly LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public LowLevelKeyboardListener()
        {
            _proc = HookCallback;
        }

        public void HookKeyboard()
        {
            if (_hookID == IntPtr.Zero)
            {
                _hookID = SetHook(_proc);
                Log.Information("{kblisetner} was hooked.", nameof(LowLevelKeyboardListener));
            }
            else
            {
                Log.Information("{kblisetner} was already hooked.", nameof(LowLevelKeyboardListener));
            }
        }

        public void UnHookKeyboard()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
                Log.Information("{kblisetner} was unhooked.", nameof(LowLevelKeyboardListener));
            }
            else
            {
                Log.Information("{kblisetner} was already unhooked.", nameof(LowLevelKeyboardListener));
            }
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule;
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KeyPressedArgs eventArgs = null;
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Key key = KeyInterop.KeyFromVirtualKey(vkCode);

                eventArgs = new (key, vkCode, false);
                OnKeyPressed?.Invoke(this, eventArgs);
            }

            IntPtr r = CallNextHookEx(_hookID, nCode, wParam, lParam);
            return eventArgs?.StopChain == true ? (IntPtr)1 : r;
        }
    }

    public class KeyPressedArgs : EventArgs
    {
        public int VkCode { get; private set; }
        public Key KeyPressed { get; private set; }
        public bool IsSystemKey { get; private set; }
        public bool StopChain { get; set; }

        public KeyPressedArgs(Key key, int vkCode, bool isSystemKey)
        {
            KeyPressed = key;
            VkCode = vkCode;
            IsSystemKey = isSystemKey;
            StopChain = false;
        }
    }
}