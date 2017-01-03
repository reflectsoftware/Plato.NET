// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Runtime.InteropServices;

namespace Plato.WinAPI
{
    /// <summary>
    ///
    /// </summary>
    public static class Netapi
    {
        /// <summary>
        ///
        /// </summary>
        public enum DSGETDCNAME_FLAGS : uint
        {
            DS_AVOID_SELF = 0x00004000,
            DS_BACKGROUND_ONLY = 0x00000100,
            DS_DIRECTORY_SERVICE_PREFERRED = 0x00000020,
            DS_DIRECTORY_SERVICE_REQUIRED = 0x00000010,
            DS_FORCE_REDISCOVERY = 0x00000001,
            DS_GC_SERVER_REQUIRED = 0x00000040,
            DS_GOOD_TIMESERV_PREFERRED = 0x00002000,
            DS_IP_REQUIRED = 0x00000200,
            DS_IS_DNS_NAME = 0x00020000,
            DS_IS_FLAT_NAME = 0x00010000,
            DS_KDC_REQUIRED = 0x00000400,
            DS_ONLY_LDAP_NEEDED = 0x00008000,
            DS_PDC_REQUIRED = 0x00000080,
            DS_RETURN_DNS_NAME = 0x40000000,
            DS_RETURN_FLAT_NAME = 0x80000000,
            DS_TIMESERV_REQUIRED = 0x00000800,
            DS_WRITABLE_REQUIRED = 0x00001000
        }

        /// <summary>
        ///
        /// </summary>
        public enum DSGETDCOPEN_FLAGS : uint
        {
            DS_GFTI_UPDATE_TDO = 0x1,
            DS_GFTI_VALID_FLAGS = 0x1,
            DS_NOTIFY_AFTER_SITE_RECORDS = 0x02,
            DS_ONLY_DO_SITE_NAME = 0x01,
            DS_OPEN_VALID_OPTION_FLAGS = DS_ONLY_DO_SITE_NAME | DS_NOTIFY_AFTER_SITE_RECORDS
            ,
        }

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DOMAIN_CONTROLLER_INFO
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DomainControllerName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DomainControllerAddress;
            public uint DomainControllerAddressType;
            public Guid DomainGuid;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DomainName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DnsForestName;
            public uint Flags;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string DcSiteName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string ClientSiteName;
        }

        [DllImport("netapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DsGetDcName(
            [MarshalAs(UnmanagedType.LPTStr)] string ComputerName,
            [MarshalAs(UnmanagedType.LPTStr)] string DomainName,
            [In] int DomainGuid, [MarshalAs(UnmanagedType.LPTStr)] string SiteName,
            uint Flags,
            out IntPtr pDOMAIN_CONTROLLER_INFO
        );

        [DllImport("netapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DsGetDcOpen(
            [MarshalAs(UnmanagedType.LPTStr)] string DnsName,
            uint OptionFlags,
            [MarshalAs(UnmanagedType.LPTStr)] string SiteName, 
            IntPtr DomainGuid, 
            [MarshalAs(UnmanagedType.LPTStr)] string DnsForestName,
            uint DcFlags,
            out IntPtr RetGetDcContext
        );

        [DllImport("netapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void DsGetDcClose(IntPtr GetDcContextHandle);

        [DllImport("netapi32.dll", SetLastError = true)]
        public static extern int DsGetDcNext(
                IntPtr GetDcContextHandle,
                IntPtr SockAddressCount,
                IntPtr SockAddresses,
                out IntPtr DnsHostName
        );

        [DllImport("netapi32.dll", SetLastError = true)]
        public static extern int NetApiBufferFree(IntPtr Buffer);

        [DllImport("netapi32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern uint NetUserChangePassword(
            [MarshalAs(UnmanagedType.LPWStr)] string domainname,
            [MarshalAs(UnmanagedType.LPWStr)] string username,
            [MarshalAs(UnmanagedType.LPWStr)] string oldpassword,
            [MarshalAs(UnmanagedType.LPWStr)] string newpassword
        );
    }
}
