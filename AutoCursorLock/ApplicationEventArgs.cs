using System;

namespace AutoCursorLock
{
    internal class ApplicationEventArgs : EventArgs
    {
        public ApplicationEventArgs(uint processId, string processName)
        {
            ProcessId = processId;
            ProcessName = processName ?? throw new ArgumentNullException(nameof(processName));
        }

        public uint ProcessId { get; }

        public string ProcessName { get; }
    }
}
