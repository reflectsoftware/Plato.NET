// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace Plato.WinAPI
{
    /// <summary>
    ///
    /// </summary>
    public static class Advapi
    {
        /// <summary>
        ///
        /// </summary>
        public enum LogonType : int
        {
            /// <summary>
            /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without 
            /// their direct intervention. This type is also for higher performance servers that process many plain-text
            /// authentication attempts at a time, such as mail or Web servers. 
            /// The LogonUser function does not cache credentials for this logon type.
            /// </summary>
            LOGON32_LOGON_BATCH = 4,
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations; 
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            LOGON32_LOGON_INTERACTIVE = 2,
            /// <summary>
            /// This logon type is intended for high performance servers to authenticate plain-text passwords.
            /// </summary>
            /// The LogonUser function does not cache credentials for this logon type. 
            LOGON32_LOGON_NETWORK = 3,
            /// <summary>
            /// This logon type preserves the name and password in the authentication package, which allows the server to make 
            /// connections to other network servers while impersonating the client. A server can accept plain-text credentials 
            /// from a client, call LogonUser, verify that the user can access the system across the network, and still 
            /// communicate with other servers.
            /// NOTE: Windows NT:  This value is not supported. 
            /// </summary>
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
            /// <summary>
            /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
            /// The new logon session has the same local identifier but uses different credentials for other network connections. 
            /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported. 
            /// </summary>
            LOGON32_LOGON_NEW_CREDENTIALS = 9,
            /// <summary>
            /// Indicates a service-type logon. The account provided must have the service privilege enabled. 
            /// </summary>
            LOGON32_LOGON_SERVICE = 5,
            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be interactively using the computer. 
            /// This logon type can generate a unique audit record that shows when the workstation was unlocked. 
            /// </summary>
            LOGON32_LOGON_UNLOCK = 7,
        }

        /// <summary>
        ///
        /// </summary>
        public enum LogonProvider : int
        {
            /// <summary>
            /// Use the standard logon provider for the system. 
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name 
            /// is not in UPN format. In this case, the default provider is NTLM. 
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            LOGON32_PROVIDER_DEFAULT = 0,
            /// <summary>
            /// Use the NTLM logon provider.
            /// </summary>
            LOGON32_PROVIDER_WINNT35 = 1,
            /// <summary>
            /// Use the NTLM logon provider.
            /// </summary>
            LOGON32_PROVIDER_WINNT40 = 2,
            /// <summary>
            /// Use the negotiate logon provider.
            /// </summary>
            LOGON32_PROVIDER_WINNT50 = 3
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername,
                                               string lpszDomain,
                                               string lpszPassword,
                                               LogonType dwLogonType,
                                               LogonProvider dwLogonProvider,
        out IntPtr token);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();


        /// <summary>
        /// Impersonates the account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="password">The password.</param>
        /// <param name="logonType">Type of the logon.</param>
        /// <param name="logonProvider">The logon provider.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException"></exception>
        public static WindowsImpersonationContext ImpersonateAccount(string userName, string domain, string password, LogonType logonType, LogonProvider logonProvider)
        {
            WindowsIdentity tempWindowsIdentity;
            var token = IntPtr.Zero;
            var tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUser(userName, domain, password, logonType, logonProvider, out token))
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        var impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            var lpProfile = new Userenv.PROFILEINFO();
                            lpProfile.dwSize = Marshal.SizeOf(lpProfile);
                            lpProfile.lpUserName = userName;
                            lpProfile.dwFlags = 1;
                            Userenv.LoadUserProfile(tokenDuplicate, ref lpProfile);

                            Kernel.CloseHandle(token);
                            Kernel.CloseHandle(tokenDuplicate);

                            token = IntPtr.Zero;
                            tokenDuplicate = IntPtr.Zero;

                            return impersonationContext;
                        }
                    }
                }
                else
                {
                    throw new SecurityException(string.Format(@"Unable to impersonate user: {0}\{1}", domain, userName));
                }
            }

            if (token != IntPtr.Zero)
            {
                Kernel.CloseHandle(token);
                token = IntPtr.Zero;
            }

            if (tokenDuplicate != IntPtr.Zero)
            {
                Kernel.CloseHandle(tokenDuplicate);
                tokenDuplicate = IntPtr.Zero;
            }

            return null;
        }

        /// <summary>
        /// Impersonates the account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="password">The password.</param>
        /// <param name="logonType">Type of the logon.</param>
        /// <returns></returns>
        public static WindowsImpersonationContext ImpersonateAccount(string userName, string domain, string password, LogonType logonType)
        {
            return ImpersonateAccount(userName, ".", password, logonType, LogonProvider.LOGON32_PROVIDER_DEFAULT);
        }

        /// <summary>
        /// Impersonates the account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="logonType">Type of the logon.</param>
        /// <returns></returns>
        public static WindowsImpersonationContext ImpersonateAccount(string userName, string password, LogonType logonType)
        {
            return ImpersonateAccount(userName, ".", password, logonType, LogonProvider.LOGON32_PROVIDER_DEFAULT);
        }

        /// <summary>
        /// Undoes the impersonation.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        public static void UndoImpersonation(WindowsImpersonationContext ctx)
        {
            ctx.Undo();
            ctx.Dispose();
        }

        /// <summary>
        /// Determines whether this instance [can account logon] the specified LPSZ username.
        /// </summary>
        /// <param name="lpszUsername">The LPSZ username.</param>
        /// <param name="lpszDomain">The LPSZ domain.</param>
        /// <param name="lpszPassword">The LPSZ password.</param>
        /// <param name="lType">Type of the l.</param>
        /// <param name="lProvider">The l provider.</param>
        /// <param name="lastError">The last error.</param>
        /// <returns></returns>
        public static bool CanAccountLogon(string lpszUsername, string lpszDomain, string lpszPassword, LogonType lType, LogonProvider lProvider, out int lastError)
        {
            var hToken = IntPtr.Zero;
            try
            {
                var rValue = LogonUser(lpszUsername, lpszDomain, lpszPassword, lType, lProvider, out hToken);
                if (!rValue)
                {
                    lastError = Marshal.GetLastWin32Error();
                }
                else
                {
                    lastError = 0;
                }

                return rValue;
            }
            finally
            {
                if (hToken != IntPtr.Zero)
                {
                    Kernel.CloseHandle(hToken);
                    hToken = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Determines whether this instance [can account logon] the specified LPSZ username.
        /// </summary>
        /// <param name="lpszUsername">The LPSZ username.</param>
        /// <param name="lpszDomain">The LPSZ domain.</param>
        /// <param name="lpszPassword">The LPSZ password.</param>
        /// <param name="lastError">The last error.</param>
        /// <returns></returns>
        public static bool CanAccountLogon(string lpszUsername, string lpszDomain, string lpszPassword, out int lastError)
        {
            return CanAccountLogon(lpszUsername, lpszDomain, lpszPassword, LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT, out lastError);
        }

        /// <summary>
        ///
        /// </summary>
        /// <seealso cref="System.IDisposable"/>
        public class Impersonate : IDisposable
        {
            private WindowsImpersonationContext ImpersonationContext { get; set; }

            /// <summary>
            /// Gets a value indicating whether this <see cref="Impersonate"/> is disposed.
            /// </summary>
            /// <value>
            /// <c>true</c> if disposed; otherwise, <c>false</c>.
            /// </value>
            public bool Disposed { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Impersonate"/> class.
            /// </summary>
            public Impersonate()
            {
                Disposed = false;
                ImpersonationContext = null;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="Impersonate"/> class.
            /// </summary>
            ~Impersonate()
            {
                Dispose(false);
            }

            /// <summary>
            /// Logon the specified userId.
            /// </summary>
            /// <param name="userId">The userId.</param>
            /// <param name="domain">The domain.</param>
            /// <param name="password">The password.</param>
            /// <param name="logonType">Type of the logon.</param>
            /// <param name="logonProvider">The logon provider.</param>
            /// <exception cref="System.Security.SecurityException">Unable to logon Impersonated account.</exception>
            public void Logon(string userId, string domain, string password, LogonType logonType, LogonProvider logonProvider)
            {
                ImpersonationContext = ImpersonateAccount(userId, domain, password, logonType, logonProvider);
                if (ImpersonationContext == null)
                {
                    throw new SecurityException("Unable to logon Impersonated account.");
                }
            }

            /// <summary>
            /// Log off this instance.
            /// </summary>
            public void Logoff()
            {
                if (ImpersonationContext != null)
                {
                    UndoImpersonation(ImpersonationContext);
                    ImpersonationContext = null;
                }
            }

            /// <summary>
            /// Releases unmanaged and - optionally - managed resources.
            /// </summary>
            /// <param name="bDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
            protected void Dispose(bool bDisposing)
            {
                lock (this)
                {
                    if (!Disposed)
                    {
                        Disposed = true;
                        GC.SuppressFinalize(this);

                        Logoff();
                    }
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
            }
        }
    }
}
