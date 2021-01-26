// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System.Windows.Input;
    using AutoCursorLock.Native;

    public class HotKey
    {
        private const uint FlagAlt = 0x0001;
        private const uint FlagControl = 0x0002;
        private const uint FlagShift = 0x0004;
        private const uint FlagWin = 0x0008;

        private readonly uint virtualKey;

        public HotKey(int id, Key key, ModifierKey modifierKey)
        {
            Id = id;
            Key = key;
            ModifierKey = modifierKey;
            this.virtualKey = (uint)KeyInterop.VirtualKeyFromKey(key);
        }

        public int Id { get; }

        public Key Key { get; }

        public ModifierKey ModifierKey { get; }

        public uint GetVirtualKeyCode()
        {
            return this.virtualKey;
        }

        public uint GetKeyModifier()
        {
            return (BoolToUInt(ModifierKey == ModifierKey.Alt) & FlagAlt)
               | (BoolToUInt(ModifierKey == ModifierKey.Control) & FlagControl)
               | (BoolToUInt(ModifierKey == ModifierKey.Shift) & FlagShift)
               | (BoolToUInt(ModifierKey == ModifierKey.Windows) & FlagWin);
        }

        private static uint BoolToUInt(bool val)
        {
            return val ? uint.MaxValue : 0;
        }

        public override string ToString()
        {
            return $"{ModifierKey}+{Key}";
        }
    }
}
