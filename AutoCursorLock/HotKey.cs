using System.Windows.Input;

namespace AutoCursorLock
{
    public class HotKey
    {
        private const uint FlagAlt = 0x0001;
        private const uint FlagControl = 0x0002;
        private const uint FlagShift = 0x0004;
        private const uint FlagWin = 0x0008;

        private readonly uint virtualKey;

        public HotKey(int id, Key key)
        {
            this.Id = id;
            this.virtualKey = (uint)KeyInterop.VirtualKeyFromKey(key);
        }

        public int Id { get; }

        public bool WithAlt { get; set; }
        public bool WithControl { get; set; }
        public bool WithShift { get; set; }
        public bool WithWin { get; set; }

        public uint GetVirtualKeyCode()
        {
            return virtualKey;
        }

        public uint GetKeyModifier()
        {
            return (BoolToUInt(WithAlt) & FlagAlt)
               | (BoolToUInt(WithControl) & FlagControl)
               | (BoolToUInt(WithShift) & FlagShift)
               | (BoolToUInt(WithWin) & FlagWin);
        }

        private static uint BoolToUInt(bool val)
        {
            return val ? uint.MaxValue : 0;
        }
    }
}
