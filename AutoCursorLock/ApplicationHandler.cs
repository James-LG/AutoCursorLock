using System;
using System.Diagnostics;

namespace AutoCursorLock
{
    internal class ApplicationHandler
    {
        /// <summary>
        /// This has to be an instance member so it doesn't get garbage collected.
        /// </summary>
        NativeMethods.WinEventDelegate? dele = null;

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        private IntPtr hook;

        public event EventHandler<ApplicationEventArgs>? ApplicationChanged;

        public bool Register()
        {
            dele = new NativeMethods.WinEventDelegate(WinEventProc);
            hook = NativeMethods.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

            return hook != IntPtr.Zero;
        }

        public bool Unregister()
        {
            return NativeMethods.UnhookWinEvent(hook);
        }

        public void Update()
        {
            var hwnd = NativeMethods.GetForegroundWindow();

            RaiseApplicationChanged(hwnd);
        }

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            RaiseApplicationChanged(hwnd);
        }

        private void RaiseApplicationChanged(IntPtr hwnd)
        {
            NativeMethods.GetWindowThreadProcessId(hwnd, out uint processId);

            try
            {
                using Process p = Process.GetProcessById((int)processId);
                Trace.WriteLine(p.ProcessName);

                ApplicationChanged?.Invoke(this, new ApplicationEventArgs(processId, p.ProcessName));
            }
            catch (InvalidOperationException)
            {
                // The process property is not defined because the process has exited or it does not have an identifier.
            }
            catch (ArgumentException)
            {
                // The process specified by the processId parameter is not running.
            }
        }
    }
}
