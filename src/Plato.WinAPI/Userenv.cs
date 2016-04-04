// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Runtime.InteropServices;

namespace Plato.WinAPI
{
    /// <summary>
    ///
    /// </summary>
    public static class Userenv
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PROFILEINFO
        {
            public int dwSize;
            public int dwFlags;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpUserName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpProfilePath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDefaultPath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpServerName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpPolicyPath;
            public IntPtr hProfile;
        }

        [DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LoadUserProfile(IntPtr hToken, ref PROFILEINFO lpProfileInfo);
    }
}
