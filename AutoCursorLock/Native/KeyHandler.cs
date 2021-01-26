// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native
{
    using System;

    internal class KeyHandler
    {
        private readonly HotKey hotKey;
        private readonly IntPtr hWnd;

        public KeyHandler(HotKey hotKey, IntPtr hWnd)
        {
            this.hotKey = hotKey;
            this.hWnd = hWnd;
        }

        public bool Register()
        {
            var modifier = this.hotKey.GetKeyModifier();
            var virtualKey = this.hotKey.GetVirtualKeyCode();
            return NativeMethods.RegisterHotKey(this.hWnd, this.hotKey.Id, modifier, virtualKey);
        }

        public bool Unregister()
        {
            return NativeMethods.UnregisterHotKey(this.hWnd, this.hotKey.Id);
        }
    }
}
