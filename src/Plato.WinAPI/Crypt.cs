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
    public static class Crypt
    {
        public const uint CERT_CLOSE_STORE_FORCE_FLAG = 0x00000001;
        public const uint CERT_CLOSE_STORE_CHECK_FLAG = 0x00000002;
        public const int CERT_SYSTEM_STORE_CURRENT_USER_ID = 1;
        public const int CERT_SYSTEM_STORE_LOCAL_MACHINE_ID = 2;
        public const int CERT_SYSTEM_STORE_LOCATION_SHIFT = 16;
        public const uint CERT_STORE_ADD_REPLACE_EXISTING = 3;
        public const uint CERT_STORE_PROV_SYSTEM = 0xa;
        public const uint CERT_STORE_OPEN_EXISTING_FLAG = 0x00004000;
        public const uint CERT_SYSTEM_STORE_CURRENT_USER = (CERT_SYSTEM_STORE_CURRENT_USER_ID << CERT_SYSTEM_STORE_LOCATION_SHIFT);
        public const uint CERT_SYSTEM_STORE_LOCAL_MACHINE = (CERT_SYSTEM_STORE_LOCAL_MACHINE_ID << CERT_SYSTEM_STORE_LOCATION_SHIFT);
        public const uint CERT_FIND_SUBJECT_STR = 0x00080007;
        public const uint CERT_SIMPLE_NAME_STR = 0x01;
        public const uint CERT_OID_NAME_STR = 0x02;
        public const uint CERT_X500_NAME_STR = 0x03;
        public const uint CRYPT_MACHINE_KEYSET = 0x00000020;
        public const uint CRYPT_USER_KEYSET = 0x00001000;
        public const uint CRYPT_USER_PROTECTED = 0x00000002;
        public const uint CRYPT_EXPORTABLE = 0x00000001;
        public const uint PKCS_7_ASN_ENCODING = 0x00010000;
        public const uint X509_ASN_ENCODING = 0x00000001;
        public const uint CERT_STORE_PROV_PKCS7 = 0x05;
        public const uint CERT_STORE_PROV_FILENAME_A = 0x07;
        public const uint CERT_STORE_PROV_FILENAME_W = 0x08;

        public const uint CERT_NAME_EMAIL_TYPE             = 1;
        public const uint CERT_NAME_RDN_TYPE               = 2;
        public const uint CERT_NAME_ATTR_TYPE              = 3;
        public const uint CERT_NAME_SIMPLE_DISPLAY_TYPE    = 4;
        public const uint CERT_NAME_FRIENDLY_DISPLAY_TYPE  = 5;
        public const uint CERT_NAME_DNS_TYPE               = 6;
        public const uint CERT_NAME_URL_TYPE               = 7;
        public const uint CERT_NAME_UPN_TYPE               = 8;

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern IntPtr PFXImportCertStore(ref CRYPT_DATA_BLOB pPfx, [MarshalAs(UnmanagedType.LPWStr)] string szPassword, uint dwFlags);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool PFXIsPFXBlob(ref CRYPT_DATA_BLOB pPfx);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CertCloseStore(IntPtr hCertStore, uint dwFlags);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern IntPtr CertOpenStore(uint uStoreProvider, uint dwMsgAndCertEncodingType, IntPtr hCryptProv, uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pvPara);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern IntPtr CertOpenStore(uint uStoreProvider, uint dwMsgAndCertEncodingType, IntPtr hCryptProv, uint dwFlags, IntPtr pvPara);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern IntPtr CertEnumCertificatesInStore(IntPtr hCertStore, IntPtr pPrevCertContext);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CertFreeCertificateContext(IntPtr hCertStore);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CertAddCertificateContextToStore(IntPtr hCertStore, IntPtr pCertContext, uint dwFlags, IntPtr ppStoreContext);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CertDeleteCertificateFromStore(IntPtr pCertContext);

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern IntPtr CertFindCertificateInStore(IntPtr hCertStore, uint dwCertEncodingType, uint dwFindFlags, uint dwFindType, IntPtr pszFindPara, IntPtr pPrevCertCntxt);

        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CertGetNameString(IntPtr pCertContext, uint dwType, uint dwFlags, IntPtr pvTypePara, [MarshalAs(UnmanagedType.LPWStr)] string pszNameString, uint cchNameString );
    }
}
