// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Plato.WinAPI
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid"/>
    public class FileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandle"/> class.
        /// </summary>
        public FileHandle() : base(true)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandle"/> class.
        /// </summary>
        /// <param name="hHandle">The h handle.</param>
        public FileHandle(IntPtr hHandle) : base(true)
        {
            handle = hHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandle"/> class.
        /// </summary>
        /// <param name="hHandle">The h handle.</param>
        public FileHandle(int hHandle) : this(new IntPtr(hHandle))
        {
        }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid"/>
    public class WindowStationHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseWindowStation(IntPtr hnd);


        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStationHandle"/> class.
        /// </summary>
        public WindowStationHandle() : base(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStationHandle"/> class.
        /// </summary>
        /// <param name="hHandle">The h handle.</param>
        public WindowStationHandle(IntPtr hHandle) : base(true)
        {
            handle = hHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStationHandle"/> class.
        /// </summary>
        /// <param name="hHandle">The h handle.</param>
        public WindowStationHandle(int hHandle) : this(new IntPtr(hHandle))
        {
        }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            return CloseWindowStation(handle);
        }
    }
}
