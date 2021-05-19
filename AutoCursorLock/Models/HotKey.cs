// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Models
{
    using System.Windows.Input;
    using AutoCursorLock.Native;

    /// <summary>
    /// Represents a hot key combination.
    /// </summary>
    public class HotKey
    {
        private const uint FlagAlt = 0x0001;
        private const uint FlagControl = 0x0002;
        private const uint FlagShift = 0x0004;
        private const uint FlagWin = 0x0008;

        private readonly uint virtualKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="key">The key to be pressed.</param>
        /// <param name="modifierKey">The modifier key.</param>
        public HotKey(int id, Key key, ModifierKey modifierKey)
        {
            Id = id;
            Key = key;
            ModifierKey = modifierKey;
            this.virtualKey = (uint)KeyInterop.VirtualKeyFromKey(key);
        }

        /// <summary>
        /// Gets the ID of the HotKey.
        /// </summary>
        /// <remarks>
        /// This is used to register and unregister the hot key with Windows.
        /// </remarks>
        public int Id { get; }

        /// <summary>
        /// Gets the key to be pressed.
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// Gets the modifier key.
        /// </summary>
        /// <remarks>
        /// This can be keys such as Shift, or Control.
        /// </remarks>
        public ModifierKey ModifierKey { get; }

        /// <summary>
        /// Gets the Win32 virtual key value for this hot key.
        /// </summary>
        /// <returns>The Win32 virtual key value.</returns>
        public uint GetVirtualKeyCode()
        {
            return this.virtualKey;
        }

        /// <summary>
        /// Gets the Win32 key modifier value for this hot key.
        /// </summary>
        /// <returns>The Win32 key modifier value.</returns>
        public uint GetKeyModifier()
        {
            return BoolToUInt(ModifierKey == ModifierKey.Alt) & FlagAlt
               | BoolToUInt(ModifierKey == ModifierKey.Control) & FlagControl
               | BoolToUInt(ModifierKey == ModifierKey.Shift) & FlagShift
               | BoolToUInt(ModifierKey == ModifierKey.Windows) & FlagWin;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{ModifierKey}+{Key}";
        }

        private static uint BoolToUInt(bool val)
        {
            return val ? uint.MaxValue : 0;
        }
    }
}
