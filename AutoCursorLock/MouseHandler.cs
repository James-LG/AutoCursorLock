using System;

namespace AutoCursorLock
{
	internal class MouseHandler
	{
		private readonly IntPtr hwnd;

        public MouseHandler(IntPtr hWnd)
        {
            this.hwnd = hWnd;
        }

        public bool LockCursor()
		{
			if (NativeMethods.GetWindowRect(hwnd, out var rect))
            {
				return NativeMethods.ClipCursor(rect);
			}
			return false;
		}

		public static bool UnlockCursor()
        {
			return NativeMethods.ClipCursor(IntPtr.Zero);
		}
	}
}
