// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App;

using System;

/// <summary>
/// Base exception for AutoCursorLock.
/// </summary>
public class AutoCursorLockException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoCursorLockException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AutoCursorLockException(string message)
        : base(message)
    {
    }
}
