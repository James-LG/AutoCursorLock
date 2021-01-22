using System;

namespace AutoCursorLock
{
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
            var modifier = hotKey.GetKeyModifier();
            var virtualKey = hotKey.GetVirtualKeyCode();
            return NativeMethods.RegisterHotKey(hWnd, hotKey.Id, modifier, virtualKey);
        }

        public bool Unregister()
        {
            return NativeMethods.UnregisterHotKey(hWnd, hotKey.Id);
        }
    }
}
