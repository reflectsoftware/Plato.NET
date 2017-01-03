// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Runtime.InteropServices;

namespace Plato.WinAPI
{
    /// <summary>   
    /// P/Invoke declarations for strong name APIs   
    /// </summary>
    public static class Mscoree
    {
        [Flags]
        public enum StrongNameKeyGenFlags : int
        {
            LeaveKey = 0x00000001,
            None = 0x00000000
        }

        /// <summary>  
        /// Return the last error  
        /// </summary>
        /// <returns>error information for the last strong name call</returns>
        [DllImport("mscoree.dll")]
        public extern static int StrongNameErrorInfo();

        /// <summary>  
        /// Free the buffer allocated by strong name functions  
        /// </summary>
        /// <param name="pbMemory">address of memory to free</param>
        [DllImport("mscoree.dll")]
        public extern static void StrongNameFreeBuffer(IntPtr pbMemory);

        /// <summary>  
        /// Retrieve the public portion of a key pair.  
        /// </summary>
        /// <param name="wszKeyContainer">key container to extract from, null to create a temporary container</param>
        /// <param name="pbKeyBlob">key blob to extract from, null to extract from a container</param>
        /// <param name="cbKeyBlob">size in bytes of <paramref name="pbKeyBlob"/></param>
        /// <param name="ppbPublicKeyBlob">[out]public key blob</param>
        /// <param name="pcbPublicKeyBlob">[out]size of <paramref name="pcbPublicKeyBlob"/></param>
        /// <returns>true on success, false on error</returns>
        [DllImport("mscoree.dll")]
        public extern static bool StrongNameGetPublicKey(
            [MarshalAs(UnmanagedType.LPWStr)] string wszKeyContainer,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pbKeyBlob,
            [MarshalAs(UnmanagedType.U4)] int cbKeyBlob,
            [Out] out IntPtr ppbPublicKeyBlob,
            [Out, MarshalAs(UnmanagedType.U4)] out int pcbPublicKeyBlob);

        /// <summary>  
        /// Delete a key pair from a key container  
        /// </summary>
        /// <param name="wszKeyContainer">key container name</param>
        /// <returns>true on success, false on failure</returns>
        [DllImport("mscoree.dll")]
        public extern static bool StrongNameKeyDelete([MarshalAs(UnmanagedType.LPWStr)] string wszKeyContainer);

        /// <summary>  
        /// Generate a new key pair with the specified key size for strong name use  
        /// </summary>
        /// <param name="wszKeyContainer">desired key container name</param>
        /// <param name="dwFlags">flags</param>
        /// <param name="dwKeySize">desired key size</param>
        /// <param name="ppbKeyBlob">[out] generated public / private key blob</param>
        /// <param name="pcbKeyBlob">[out] size of the generated blob</param>
        /// <returns>true if the key was generated, false if there was an error</returns>
        [DllImport("mscoree.dll")]
        public extern static bool StrongNameKeyGenEx(
        [MarshalAs(UnmanagedType.LPWStr)] string wszKeyContainer,
            StrongNameKeyGenFlags dwFlags,
            int dwKeySize,
            [Out] out IntPtr ppbKeyBlob,
            [Out] out long pcbKeyBlob);

        /// <summary>  
        /// Import a key pair into a key container  
        /// </summary>
        /// <param name="wszKeyContainer">desired key container name</param>
        /// <param name="pbKeyBlob">public/private key blob</param>
        /// <param name="cbKeyBlob">number of bytes in <paramref name="pbKeyBlob"/></param>
        /// <returns>true on success, false on error</returns>
        [DllImport("mscoree.dll")]
        public extern static bool StrongNameKeyInstall(
            [MarshalAs(UnmanagedType.LPWStr)] string wszKeyContainer,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pbKeyBlob,
            int cbKeyBlob);

        /// <summary>  
        /// Verify a strong name/manifest against a public key blob  
        /// </summary>
        /// <param name="wszFilePath">valid path to the PE file for the assembly</param>
        /// <param name="fForceVerification">verify even if the settings in the registry disable it</param>
        /// <param name="pfWasVerified">[out] set to false if verify succeeded due to registry settings</param>
        /// <returns>true if the assembly verified, false otherwise</returns>
        [DllImport("mscoree.dll")]
        public static extern bool StrongNameSignatureVerificationEx(
            [MarshalAs(UnmanagedType.LPWStr)] string wszFilePath, 
            bool fForceVerification, ref 
            bool pfWasVerified);

        /// <summary>  
        /// Create a strong name token from an assembly file, and additionally  
        /// return the full public key blob  
        /// </summary>
        /// <param name="wszFilePath">path to the PE file for the assembly</param>
        /// <param name="ppbStrongNameToken">[out]strong name token</param>
        /// <param name="pcbStrongNameToken">[out]length of <paramref name="ppbStrongNameToken"/></param>
        /// <param name="ppbPublicKeyBlob">[out]public key blob</param>
        /// <param name="pcbPublicKeyBlob">[out]length of <paramref name="ppbPublicKeyBlob"/></param> /// <returns>true on success, false on error</returns>
        [DllImport("mscoree.dll")]
        public extern static bool StrongNameTokenFromAssemblyEx(
            [MarshalAs(UnmanagedType.LPWStr)] string wszFilePath,
            [Out] out IntPtr ppbStrongNameToken,
            [Out, MarshalAs(UnmanagedType.U4)] out int pcbStrongNameToken,
            [Out] out IntPtr ppbPublicKeyBlob,
            [Out, MarshalAs(UnmanagedType.U4)] out int pcbPublicKeyBlob);

        /// <summary> 
        /// Create a strong name token from a public key blob 
        /// </summary>
        /// <param name="pbPublicKeyBlob">key blob to generate the token for</param>
        /// <param name="cbPublicKeyBlob">number of bytes in <paramref name="pbPublicKeyBlob"/></param>
        /// <param name="ppbStrongNameToken">[out]public key token</param>
        /// <param name="pcbStrongNameToken">[out]number of bytes in the token</param>
        /// <returns>true on success, false on error</returns>
        [DllImport("mscoree.dll")]
        public extern static bool StrongNameTokenFromPublicKey(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pbPublicKeyBlob, 
            int cbPublicKeyBlob, 
            [Out] out IntPtr ppbStrongNameToken, 
            [Out, MarshalAs(UnmanagedType.U4)] out int pcbStrongNameToken);        
    }
}
