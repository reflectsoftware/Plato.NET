// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.DirectoryServices;
using Plato.WinAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Plato.Security.Cryptography
{
    public class Certificate : IDisposable
    {        
        private Asymmetric _asymmetric;

        /// <summary>
        /// Gets the cert.
        /// </summary>
        /// <value>
        /// The cert.
        /// </value>
        public X509Certificate2 Cert { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Certificate"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Formats the thumbprint.
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        public static string FormatThumbprint(string thumbprint)
        {
            return Regex.Replace(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(thumbprint.ToUpper())), "[^0-9,a-z,A-Z]", string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Certificate"/> class.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        public Certificate(byte[] rawData) : this(new X509Certificate2(rawData))
        {
            Disposed = false;
        }

        public Certificate(X509Certificate2 cert)
        {
            Disposed = false;
            Cert = cert;
            _asymmetric = Asymmetric.GetInstanceFromCertificate(Cert, GetCertThumbprint());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    _asymmetric?.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the files bytes.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static byte[] GetFilesBytes(string fileName)
        {
            var blob = (byte[])null;
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                blob = new byte[fs.Length];
                fs.Read(blob, 0, blob.Length);
            }

            return blob;
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="dataString">The data string.</param>
        /// <param name="encode">The encode.</param>
        /// <returns></returns>
        public string EncryptString(string dataString, Encoding encode)
        {
            return _asymmetric.EncryptString(dataString, encode);
        }

        /// <summary>
        /// Encrypts the bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] EncryptBytes(byte[] data)
        {
            return _asymmetric.EncryptBytes(data);
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="encryptedString">The encrypted string.</param>
        /// <param name="encode">The encode.</param>
        /// <returns></returns>
        public string DecryptString(string encryptedString, Encoding encode)
        {
            return _asymmetric.DecryptString(encryptedString, encode);
        }

        /// <summary>
        /// Decrypts the bytes.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns></returns>
        public byte[] DecryptBytes(byte[] encryptedData)
        {
            return _asymmetric.DecryptBytes(encryptedData);
        }

        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <returns></returns>
        public string GetIssuer()
        {
            return Cert.Issuer;
        }

        /// <summary>
        /// Gets the name of the issue to.
        /// </summary>
        /// <returns></returns>
        public string GetIssueToName()
        {
            return GetIssueToName(Cert);
        }

        /// <summary>
        /// Gets the subject name property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public string GetSubjectNameProperty(string property)
        {
            return GetSubjectNameProperty(Cert, property);
        }

        /// <summary>
        /// Gets the issuer name property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public string GetIssuerNameProperty(string property)
        {
            return GetIssuerNameProperty(Cert, property);
        }

        /// <summary>
        /// Gets the cert thumbprint.
        /// </summary>
        /// <returns></returns>
        public string GetCertThumbprint()
        {
            return GetCertThumbprint(Cert);
        }

        /// <summary>
        /// Gets the cert thumbprint.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string GetCertThumbprint(X509Certificate2 cert)
        {
            return FormatThumbprint(cert.Thumbprint);
        }

        /// <summary>
        /// Gets the name of the issue to.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string GetIssueToName(X509Certificate2 cert)
        {
            return GetSubjectNameProperty( cert, "CN");
        }

        /// <summary>
        /// Gets the subject name property.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static string GetSubjectNameProperty(X509Certificate2 cert, string property)
        {
            return ADHelper.GetDistinguishNamePropertyValue(cert.SubjectName.Name, property.ToUpper(), string.Empty);
        }

        /// <summary>
        /// Gets the issuer name property.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static string GetIssuerNameProperty(X509Certificate2 cert, string property)
        {
            return ADHelper.GetDistinguishNamePropertyValue(cert.IssuerName.Name, property.ToUpper(), string.Empty);
        }

        /// <summary>
        /// Determines whether the specified cert is root.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static bool IsRoot(X509Certificate2 cert)
        {
            return cert.Issuer == cert.Subject;
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pfxBlob">The PFX BLOB.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException">
        /// Invalid PFX format
        /// or
        /// </exception>
        public static string ImportPFX(string storeName, StoreLocation storeLoc, byte[] pfxBlob, string password)
        {
            var cBlob = new Crypt.CRYPT_DATA_BLOB();
            var hMemStore = IntPtr.Zero;
            try
            {
                cBlob.cbData = pfxBlob.Length;
                cBlob.pbData = Marshal.AllocHGlobal(pfxBlob.Length);
                Marshal.Copy(pfxBlob, 0, cBlob.pbData, pfxBlob.Length);

                if (!Crypt.PFXIsPFXBlob(ref cBlob))
                {
                    throw new CryptographicException("Invalid PFX format");
                }

                hMemStore = Crypt.PFXImportCertStore(ref cBlob, password, Crypt.CRYPT_USER_KEYSET);
                if (hMemStore == IntPtr.Zero)
                {
                    throw new CryptographicException(Kernel.GetFormatMessage(Marshal.GetLastWin32Error()));
                }

                var memStore = new X509Store(hMemStore);
                var certs = new X509Certificate2[ memStore.Certificates.Count ];
                memStore.Certificates.CopyTo(certs, 0);

                return ImportCertificates(storeName, storeLoc, certs);
            }
            finally
            {
                if (hMemStore != IntPtr.Zero)
                {
                    Crypt.CertCloseStore(hMemStore, Crypt.CERT_CLOSE_STORE_CHECK_FLAG);
                }

                if (cBlob.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cBlob.pbData);
                }
            }
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="pfxBlob">The PFX BLOB.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(string storeName, byte[] pfxBlob, string password)
        {
            return ImportPFX(storeName, StoreLocation.CurrentUser, pfxBlob, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pfxBlob">The PFX BLOB.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(StoreName name, StoreLocation storeLoc, byte[] pfxBlob, string password)
        {
            return ImportPFX(name.ToString(), storeLoc, pfxBlob, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pfxBlob">The PFX BLOB.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(StoreName name, byte[] pfxBlob, string password)
        {
            return ImportPFX(name.ToString(), StoreLocation.CurrentUser, pfxBlob, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pfxBlob">The PFX BLOB.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(StoreLocation storeLoc, byte[] pfxBlob, string password)
        {
            return ImportPFX(StoreName.My, storeLoc, pfxBlob, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pfxFile">The PFX file.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException">Invalid PFX format</exception>
        public static string ImportPFX(string storeName, StoreLocation storeLoc, string pfxFile, string password)
        {
            var pfxBlob = GetFilesBytes(pfxFile);
            if ( pfxBlob == null || pfxBlob.Length == 0 )
            {
                throw new CryptographicException("Invalid PFX format");
            }

            return ImportPFX(storeName, storeLoc, pfxBlob, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="pfxFile">The PFX file.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(string storeName, string pfxFile, string password)
        {
            return ImportPFX(storeName, StoreLocation.CurrentUser, pfxFile, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pfxFile">The PFX file.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(StoreName name, StoreLocation storeLoc, string pfxFile, string password)
        {
            return ImportPFX(name.ToString(), storeLoc, pfxFile, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pfxFile">The PFX file.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(StoreName name, string pfxFile, string password)
        {
            return ImportPFX(name.ToString(), StoreLocation.CurrentUser, pfxFile, password);
        }

        /// <summary>
        /// Imports the PFX.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pfxFile">The PFX file.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string ImportPFX(StoreLocation storeLoc, string pfxFile, string password)
        {
            return ImportPFX(StoreName.My, storeLoc, pfxFile, password);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pkcsBlob">The PKCS BLOB.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static string ImportPKCS7(string storeName, StoreLocation storeLoc, byte[] pkcsBlob)
        {
            var pvPara = IntPtr.Zero;
            var hMemStore = IntPtr.Zero;

            var cBlob = new Crypt.CRYPT_DATA_BLOB() { cbData = pkcsBlob.Length, pbData = Marshal.AllocHGlobal(pkcsBlob.Length) };
            Marshal.Copy(pkcsBlob, 0, cBlob.pbData, pkcsBlob.Length);
            try
            {
                pvPara = Marshal.AllocHGlobal(Marshal.SizeOf(cBlob));
                Marshal.StructureToPtr(cBlob, pvPara, false);

                var encodingType = Crypt.PKCS_7_ASN_ENCODING | Crypt.X509_ASN_ENCODING;
                hMemStore = Crypt.CertOpenStore(Crypt.CERT_STORE_PROV_PKCS7, encodingType, IntPtr.Zero, 0, pvPara);

                if (hMemStore == IntPtr.Zero)
                {
                    throw new CryptographicException(Kernel.GetFormatMessage(Marshal.GetLastWin32Error()));
                }
                var memStore = new X509Store(hMemStore);

                var certs = new X509Certificate2[memStore.Certificates.Count];
                memStore.Certificates.CopyTo(certs, 0);

                return ImportCertificates(storeName, storeLoc, certs);
            }
            finally
            {
                if (hMemStore != IntPtr.Zero)
                {
                    Crypt.CertCloseStore(hMemStore, Crypt.CERT_CLOSE_STORE_CHECK_FLAG);
                }

                if (pvPara != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pvPara);
                }

                if (cBlob.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cBlob.pbData);
                }
            }
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="pkcsBlob">The PKCS BLOB.</param>
        /// <returns></returns>
        public static string ImportPKCS7(string storeName, byte[] pkcsBlob)
        {
            return ImportPKCS7(storeName, StoreLocation.CurrentUser, pkcsBlob);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pkcsBlob">The PKCS BLOB.</param>
        /// <returns></returns>
        public static string ImportPKCS7(StoreName name, StoreLocation storeLoc, byte[] pkcsBlob)
        {
            return ImportPKCS7(name.ToString(), storeLoc, pkcsBlob);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pkcsBlob">The PKCS BLOB.</param>
        /// <returns></returns>
        public static string ImportPKCS7(StoreName name, byte[] pkcsBlob)
        {
            return ImportPKCS7(name.ToString(), StoreLocation.CurrentUser, pkcsBlob);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pkcsBlob">The PKCS BLOB.</param>
        /// <returns></returns>
        public static string ImportPKCS7(StoreLocation storeLoc, byte[] pkcsBlob)
        {
            return ImportPKCS7(StoreName.My, storeLoc, pkcsBlob);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pkcsFile">The PKCS file.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException">Invalid PKCS7 format</exception>
        public static string ImportPKCS7(string storeName, StoreLocation storeLoc, string pkcsFile)
        {
            var pkcsBlob = GetFilesBytes(pkcsFile);
            if (pkcsBlob == null || pkcsBlob.Length == 0)
            {
                throw new CryptographicException("Invalid PKCS7 format");
            }

            return ImportPKCS7(storeName, storeLoc, pkcsBlob);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="pkcsFile">The PKCS file.</param>
        /// <returns></returns>
        public static string ImportPKCS7(string storeName, string pkcsFile)
        {
            return ImportPKCS7(storeName, StoreLocation.CurrentUser, pkcsFile);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pkcsFile">The PKCS file.</param>
        /// <returns></returns>
        public static string ImportPKCS7(StoreName name, StoreLocation storeLoc, string pkcsFile)
        {
            return ImportPKCS7(name.ToString(), storeLoc, pkcsFile);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pkcsFile">The PKCS file.</param>
        /// <returns></returns>
        public static string ImportPKCS7(StoreName name, string pkcsFile)
        {
            return ImportPKCS7(name.ToString(), StoreLocation.CurrentUser, pkcsFile);
        }

        /// <summary>
        /// Imports the PKC s7.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="pkcsFile">The PKCS file.</param>
        /// <returns></returns>
        public static string ImportPKCS7(StoreLocation storeLoc, string pkcsFile)
        {
            return ImportPKCS7(StoreName.My, storeLoc, pkcsFile);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        public static void RemoveCert(string storeName, StoreLocation storeLoc, X509FindType fType, object findValue)
        {
            var loc = new X509Store(storeName, storeLoc);
            loc.Open(OpenFlags.ReadWrite | OpenFlags.IncludeArchived | OpenFlags.OpenExistingOnly);
            try
            {
                var col = loc.Certificates.Find(fType, findValue, false);
                if (col == null || col.Count == 0)
                {
                    return;
                }

                loc.RemoveRange(col);
            }
            finally
            {
                loc.Close();
            }
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        public static void RemoveCert(string storeName, X509FindType fType, object findValue)
        {
            RemoveCert(storeName, StoreLocation.LocalMachine, fType, findValue);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        public static void RemoveCert(StoreName name, StoreLocation storeLoc, X509FindType fType, object findValue)
        {
            RemoveCert(name.ToString(), storeLoc, fType, findValue);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        public static void RemoveCert(StoreName name, X509FindType fType, object findValue)
        {
            RemoveCert(name.ToString(), StoreLocation.CurrentUser, fType, findValue);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        public static void RemoveCert(StoreLocation storeLoc, X509FindType fType, object findValue)
        {
            RemoveCert(StoreName.My, storeLoc, fType, findValue);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        public static void RemoveCert(string storeName, StoreLocation storeLoc, X509Certificate2 cert)
        {
            RemoveCert(storeName, storeLoc, X509FindType.FindByThumbprint, GetCertThumbprint(cert));
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="cert">The cert.</param>
        public static void RemoveCert(string storeName, X509Certificate2 cert)
        {
            RemoveCert(storeName, StoreLocation.CurrentUser, cert);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        public static void RemoveCert(StoreName name, StoreLocation storeLoc, X509Certificate2 cert)
        {
            RemoveCert(name.ToString(), storeLoc, cert);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cert">The cert.</param>
        public static void RemoveCert(StoreName name, X509Certificate2 cert)
        {
            RemoveCert(name.ToString(), StoreLocation.CurrentUser, cert);
        }

        /// <summary>
        /// Removes the cert.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        public static void RemoveCert(StoreLocation storeLoc, X509Certificate2 cert)
        {
            RemoveCert(StoreName.My, storeLoc, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(string storeName, StoreLocation storeLoc, X509Certificate2 cert)
        {
            var loc = new X509Store(storeName, storeLoc);
            loc.Open(OpenFlags.ReadWrite | OpenFlags.IncludeArchived | OpenFlags.OpenExistingOnly);
            try
            {
                var thumbprint = GetCertThumbprint(cert);
                var col = loc.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (col == null || col.Count == 0)
                {
                    loc.Add(cert);
                }

                return thumbprint;
            }
            finally
            {
                loc.Close();
            }
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(string storeName, X509Certificate2 cert)
        {
            return ImportCert(storeName, StoreLocation.CurrentUser, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(StoreName name, StoreLocation storeLoc, X509Certificate2 cert)
        {
            return ImportCert(name.ToString(), storeLoc, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(StoreName name, X509Certificate2 cert)
        {
            return ImportCert(name.ToString(), StoreLocation.CurrentUser, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(StoreLocation storeLoc, X509Certificate2 cert)
        {
            return ImportCert(StoreName.My, storeLoc, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(string storeName, StoreLocation storeLoc, byte[] cert)
        {
            return ImportCert(storeName, storeLoc, new X509Certificate2(cert));
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(string storeName, byte[] cert)
        {
            return ImportCert(storeName, StoreLocation.CurrentUser, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(StoreName name, StoreLocation storeLoc, byte[] cert)
        {
            return ImportCert(name.ToString(), storeLoc, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(StoreName name, byte[] cert)
        {
            return ImportCert(name.ToString(), StoreLocation.CurrentUser, cert);
        }

        /// <summary>
        /// Imports the cert.
        /// </summary>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string ImportCert(StoreLocation storeLoc, byte[] cert)
        {
            return ImportCert(StoreName.My, storeLoc, cert);
        }

        /// <summary>
        /// Imports the certificates.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLoc">The store loc.</param>
        /// <param name="certs">The certs.</param>
        /// <returns></returns>
        public static string ImportCertificates(string storeName, StoreLocation storeLoc, X509Certificate2[] certs)
        {
            var rootCerts = new List<X509Certificate2>();
            var issuedCerts = new List<X509Certificate2>();

            var bRootWasFirst = false;
            var mainCertThumbprint = string.Empty;

            foreach (var cert in certs)
            {
                if (!IsRoot(cert))
                {
                    issuedCerts.Add(cert);
                }
                else
                {
                    rootCerts.Add(cert);
                    mainCertThumbprint = GetCertThumbprint(cert);

                    if (issuedCerts.Count == 0)
                    {
                        bRootWasFirst = true;
                    }
                }
            }


            if (issuedCerts.Count > 0)
            {
                if (bRootWasFirst)
                {
                    mainCertThumbprint = GetCertThumbprint(issuedCerts[issuedCerts.Count - 1]);
                }
                else
                {
                    mainCertThumbprint = GetCertThumbprint(issuedCerts[0]);
                }
            }


            foreach (X509Certificate2 cert in rootCerts)
            {
                ImportCert(StoreName.Root, StoreLocation.LocalMachine, cert);
            }


            foreach (X509Certificate2 cert in issuedCerts)
            {
                ImportCert(storeName, storeLoc, cert);
            }

            return mainCertThumbprint;
        }

        /// <summary>
        /// Gets the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="sLoc">The s loc.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        /// <param name="bValidOnly">if set to <c>true</c> [b valid only].</param>
        /// <returns></returns>
        public static X509Certificate2 GetCert(string storeName, StoreLocation sLoc, X509FindType fType, object findValue, bool bValidOnly)
        {
            var loc = new X509Store(storeName, sLoc);
            loc.Open(OpenFlags.ReadOnly | OpenFlags.IncludeArchived);
            try
            {
                var col = loc.Certificates.Find(fType, findValue, bValidOnly);
                if (col == null || col.Count == 0)
                {
                    return null;

                }
                return col[0];
            }
            finally
            {
                loc.Close();
            }
        }

        /// <summary>
        /// Gets the cert.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        /// <param name="bValidOnly">if set to <c>true</c> [b valid only].</param>
        /// <returns></returns>
        public static X509Certificate2 GetCert(string storeName, X509FindType fType, object findValue, bool bValidOnly)
        {
            return GetCert(storeName, StoreLocation.LocalMachine, fType, findValue, bValidOnly);
        }

        /// <summary>
        /// Gets the cert.
        /// </summary>
        /// <param name="sName">Name of the s.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        /// <param name="bValidOnly">if set to <c>true</c> [b valid only].</param>
        /// <returns></returns>
        public static X509Certificate2 GetCert(StoreName sName, X509FindType fType, object findValue, bool bValidOnly)
        {
            return GetCert(sName.ToString(), StoreLocation.LocalMachine, fType, findValue, bValidOnly);
        }

        /// <summary>
        /// Gets the cert.
        /// </summary>
        /// <param name="sLoc">The s loc.</param>
        /// <param name="fType">Type of the f.</param>
        /// <param name="findValue">The find value.</param>
        /// <param name="bValidOnly">if set to <c>true</c> [b valid only].</param>
        /// <returns></returns>
        public static X509Certificate2 GetCert(StoreLocation sLoc, X509FindType fType, object findValue, bool bValidOnly)
        {
            return GetCert(StoreName.My.ToString(), sLoc, fType, findValue, bValidOnly);
        }

        /// <summary>
        /// Gets the cert.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// findValue
        /// or
        /// storeLocation
        /// or
        /// storeName
        /// or
        /// x509FindType
        /// </exception>
        public static X509Certificate2 GetCert(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node.Attributes["findValue"] == null)
            {
                throw new ArgumentNullException("findValue");
            }

            if (node.Attributes["storeLocation"] == null)
            {
                throw new ArgumentNullException("storeLocation");
            }

            if (node.Attributes["storeName"] == null)
            {
                throw new ArgumentNullException("storeName");
            }

            if (node.Attributes["x509FindType"] == null)
            {
                throw new ArgumentNullException("x509FindType");
            }

            var storeName = node.Attributes["storeName"].Value;
            var storeLocation = node.Attributes["storeLocation"].Value;
            var x509FindType = node.Attributes["x509FindType"].Value;
            var sLoc = (StoreLocation)Enum.Parse(typeof(StoreLocation), storeLocation, true);
            var findType = (X509FindType)Enum.Parse(typeof(X509FindType), x509FindType, true);

            string findValue;
            if (findType == X509FindType.FindByThumbprint)
            {
                findValue = node.Attributes["findValue"].Value.Replace(" ", string.Empty);
            }
            else
            {
                findValue = node.Attributes["findValue"].Value;
            }

            return GetCert(storeName, sLoc, findType, findValue, false);
        }
    }
}

