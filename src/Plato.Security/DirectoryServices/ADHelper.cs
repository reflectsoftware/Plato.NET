// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.WinAPI;
using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace Plato.Security.DirectoryServices
{
    /// <summary>
    ///
    /// </summary>
    public static class ADHelper
    {
        /// <summary>
        /// Gets the distinguish name property value.
        /// </summary>
        /// <param name="distinguishName">Name of the distinguish.</param>
        /// <param name="property">The property.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetDistinguishNamePropertyValue(string distinguishName, string property, string defaultValue)
        {
            property = property.Trim().ToLower();

            var parts = distinguishName.Split(',');
            foreach (var part in parts)
            {
                var subparts = part.Split('=');
                if (subparts[0].ToLower().Trim() == property)
                {
                    if (subparts.Length == 2)
                    {
                        return subparts[1];
                    }

                    return defaultValue;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the distinguish name property value.
        /// </summary>
        /// <param name="distinguishName">Name of the distinguish.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static string GetDistinguishNamePropertyValue(string distinguishName, string property)
        {
            return GetDistinguishNamePropertyValue(distinguishName, property, string.Empty);
        }

        /// <summary>
        /// Gets the domain string collection.
        /// </summary>
        /// <returns></returns>
        public static StringCollection GetDomainStringCollection()
        {
            var domainList = new StringCollection();
            var pcInfo = IntPtr.Zero;

            var dwRet = Netapi.DsGetDcName(null, null, 0, null, 0, out pcInfo);
            if (Kernel.ERROR_SUCCESS == dwRet)
            {
                try
                {
                    var dcInfo = (Netapi.DOMAIN_CONTROLLER_INFO)Marshal.PtrToStructure(pcInfo, typeof(Netapi.DOMAIN_CONTROLLER_INFO));


                    IntPtr hGetDc;
                    dwRet = Netapi.DsGetDcOpen(dcInfo.DomainName, (uint)Netapi.DSGETDCOPEN_FLAGS.DS_NOTIFY_AFTER_SITE_RECORDS, null, IntPtr.Zero, null, 0, out hGetDc);
                    if (Kernel.ERROR_SUCCESS == dwRet)
                    {
                        try
                        {
                            while (true)
                            {
                                var pDnsHostName = IntPtr.Zero;
                                dwRet = Netapi.DsGetDcNext(hGetDc, IntPtr.Zero, IntPtr.Zero, out pDnsHostName);

                                if (Kernel.ERROR_SUCCESS == dwRet)
                                {
                                    var hostName = Marshal.PtrToStringAnsi(pDnsHostName).ToUpper();

                                    var parts = hostName.Split('.');
                                    if (!domainList.Contains(parts[1]))
                                    {
                                        domainList.Add(parts[1]);
                                    }
                                    
                                    Netapi.NetApiBufferFree(pDnsHostName);
                                }
                                else
                                {
                                    if (Kernel.ERROR_NO_MORE_ITEMS == dwRet)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (Kernel.ERROR_FILEMARK_DETECTED == dwRet)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (Kernel.DNS_ERROR_NO_DNS_SERVERS == dwRet)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            Netapi.DsGetDcClose(hGetDc);
                        }
                    }
                }
                finally
                {
                    Netapi.NetApiBufferFree(pcInfo);
                }
            }

            return domainList;
        }
    }
}
