using System;

namespace AutoCursorLock
{
    internal class ApplicationEventArgs : EventArgs
    {
        public ApplicationEventArgs(IntPtr handle, uint processId, string processName)
        {
            Handle = handle;
            ProcessId = processId;
            ProcessName = processName ?? throw new ArgumentNullException(nameof(processName));
        }

        public IntPtr Handle { get; }

        public uint ProcessId { get; }

        public string ProcessName { get; }
    }
}
