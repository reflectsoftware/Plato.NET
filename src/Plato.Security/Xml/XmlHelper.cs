// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Cryptography;
using Plato.Serializers.Interfaces;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace Plato.Security.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="obj">The object.</param>
        /// <param name="bPreserveWhitespace">if set to <c>true</c> [b preserve whitespace].</param>
        /// <returns></returns>
        public static XmlDocument SerializeObject(IObjectSerializer serializer, object obj, bool bPreserveWhitespace)
        {
            var xmlString = Encoding.UTF8.GetString(serializer.Serialize(obj));
            var xDoc = new XmlDocument() { PreserveWhitespace = bPreserveWhitespace };

            xDoc.LoadXml(xmlString);

            return xDoc;
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static XmlDocument SerializeObject(IObjectSerializer serializer, object obj)
        {
            return SerializeObject(serializer, obj, false);
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">The serializer.</param>
        /// <param name="xDoc">The x document.</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(IObjectSerializer serializer, XmlDocument xDoc) where T: class
        {
            return serializer.Deserialize<T>(Encoding.UTF8.GetBytes(xDoc.InnerXml));
        }

        /// <summary>
        /// Creates the XML document from node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <returns></returns>
        private static XmlDocument CreateXmlDocumentFromNode(XmlNode xNode)
        {
            var xDoc = new XmlDocument() { PreserveWhitespace = true };
            xDoc.LoadXml(xNode.OuterXml);

            return xDoc;
        }

        /// <summary>
        /// Signs the specified signed XML.
        /// </summary>
        /// <param name="signedXml">The signed XML.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static XmlElement Sign(SignedXml signedXml, AsymmetricAlgorithm key)
        {
            signedXml.SigningKey = key;
            signedXml.Signature.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationWithCommentsUrl;

            var xmlSignature = signedXml.Signature;
            var xmlReference = new Reference(string.Empty);
            var xmlEnv = new XmlDsigEnvelopedSignatureTransform(false);

            xmlReference.AddTransform(xmlEnv);
            xmlSignature.SignedInfo.AddReference(xmlReference);

            var kInfo = new KeyInfo();
            var kClause = (key is RSA) ? new RSAKeyValue((RSA)key) : (KeyInfoClause)new DSAKeyValue((DSA)key);
            kInfo.AddClause(kClause);
            xmlSignature.KeyInfo = kInfo;

            signedXml.ComputeSignature();

            return signedXml.GetXml();
        }

        /// <summary>
        /// Signs the specified x element.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static XmlElement Sign(XmlElement xElement, AsymmetricAlgorithm key)
        {
            xElement.AppendChild(Sign(new SignedXml(xElement), key));
            return xElement;
        }

        /// <summary>
        /// Signs the specified x element.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="rsa">The RSA.</param>
        /// <returns></returns>
        public static XmlElement Sign(XmlElement xElement, RSAService rsa)
        {
            return Sign(xElement, rsa.Provider);
        }

        /// <summary>
        /// Signs the specified x element.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static XmlElement Sign(XmlElement xElement, X509Certificate2 cert)
        {
            return Sign(xElement, cert.PrivateKey);
        }

        /// <summary>
        /// Signs the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static XmlNode Sign(XmlNode xNode, AsymmetricAlgorithm key)
        {
            return Sign((XmlElement)xNode.CloneNode(true), key);
        }

        /// <summary>
        /// Signs the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="rsa">The RSA.</param>
        /// <returns></returns>
        public static XmlNode Sign(XmlNode xNode, RSAService rsa)
        {
            return Sign(xNode, rsa.Provider);
        }

        /// <summary>
        /// Signs the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static XmlNode Sign(XmlNode xNode, X509Certificate2 cert)
        {
            return Sign(xNode, cert.PrivateKey);
        }

        /// <summary>
        /// Signs the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static XmlDocument Sign(XmlDocument xDoc, AsymmetricAlgorithm key)
        {
            var rDoc = (XmlDocument)xDoc.Clone();
            rDoc.PreserveWhitespace = xDoc.PreserveWhitespace;

            rDoc.DocumentElement.AppendChild(rDoc.ImportNode(Sign(new SignedXml(rDoc), key), true));

            if (rDoc.FirstChild is XmlDeclaration)
            {
                rDoc.RemoveChild(rDoc.FirstChild);
            }

            return rDoc;
        }

        /// <summary>
        /// Signs the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="rsa">The RSA.</param>
        /// <returns></returns>
        public static XmlDocument Sign(XmlDocument xDoc, RSAService rsa)
        {
            return Sign(xDoc, rsa.Provider);
        }

        /// <summary>
        /// Signs the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static XmlDocument Sign(XmlDocument xDoc, X509Certificate2 cert)
        {
            return Sign(xDoc, cert.PrivateKey);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlElement xElement, AsymmetricAlgorithm key)
        {
            var signedXml = new SignedXml(xElement);
            var signedElement = xElement;

            if (xElement.Name != "Signature")
            {
                var nodeList = xElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
                if (nodeList == null || nodeList.Count == 0)
                {
                    return false;
                }

                signedElement = (XmlElement)nodeList[0];
            }

            signedXml.LoadXml(signedElement);

            return signedXml.CheckSignature(key);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="rsa">The RSA.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlElement xElement, RSAService rsa)
        {
            return VerifySignature(xElement, rsa.Provider);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlElement xElement, X509Certificate2 cert)
        {
            return VerifySignature(xElement, cert.PublicKey.Key);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlNode xNode, AsymmetricAlgorithm key)
        {
            return VerifySignature((XmlElement)xNode, key);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="rsa">The RSA.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlNode xNode, RSAService rsa)
        {
            return VerifySignature(xNode, rsa.Provider);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlDocument xDoc, AsymmetricAlgorithm key)
        {
            var signedXml = new SignedXml(xDoc);

            var nodeList = xDoc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
            if (nodeList == null || nodeList.Count == 0)
            {
                return false;
            }

            signedXml.LoadXml((XmlElement)nodeList[0]);

            return signedXml.CheckSignature(key);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="rsa">The RSA.</param>
        /// <returns></returns>
        public static bool VerifySignature(XmlDocument xDoc, RSAService rsa)
        {
            return VerifySignature(xDoc, rsa.Provider);
        }

        public static bool VerifySignature(XmlDocument xDoc, X509Certificate2 cert)
        {
            return VerifySignature(xDoc, cert.PublicKey.Key);
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException">
        /// The specified algorithm is not supported for XML Encryption.
        /// </exception>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, XmlElement elementToEncrypt, SymmetricAlgorithm algo, string keyName)
        {
            var docCopy = (XmlDocument)xDoc.Clone();
            docCopy.PreserveWhitespace = xDoc.PreserveWhitespace;

            var eXml = new EncryptedXml();
            var encryptedElement = eXml.EncryptData(elementToEncrypt, algo, false);
            var edElement = new EncryptedData() { Type = EncryptedXml.XmlEncElementUrl };

            string encryptionMethod = null;
            if (algo is TripleDES)
            {
                encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
            }
            else
            {
                if (algo is DES)
                {
                    encryptionMethod = EncryptedXml.XmlEncDESUrl;
                }
                else
                {
                    if (algo is Rijndael)
                    {
                        switch (algo.KeySize)
                        {
                            case 128:
                                encryptionMethod = EncryptedXml.XmlEncAES128Url;
                                break;

                            case 192:
                                encryptionMethod = EncryptedXml.XmlEncAES192Url;
                                break;

                            case 256:
                                encryptionMethod = EncryptedXml.XmlEncAES256Url;
                                break;

                            default:
                                throw new CryptographicException(string.Format("Unsupported key size: {0}", algo.KeySize));
                        }
                    }
                    else
                    {
                        throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
                    }
                }
            }

            edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);
            edElement.KeyInfo = new KeyInfo();
            edElement.KeyInfo.AddClause(new KeyInfoName(keyName));
            edElement.CipherData.CipherValue = encryptedElement;

            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);

            if (xDoc.FirstChild is XmlDeclaration)
            {
                xDoc.RemoveChild(xDoc.FirstChild);
            }

            var rDoc = xDoc;
            xDoc = docCopy;

            return rDoc;
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, SymmetricAlgorithm algo, string keyName)
        {
            return Encrypt(ref xDoc, xDoc.DocumentElement, algo, keyName);
        }

        public static XmlDocument Decrypt(XmlDocument xDoc, SymmetricAlgorithm algo, string keyName)
        {
            var dDoc = (XmlDocument)xDoc.Clone();
            dDoc.PreserveWhitespace = xDoc.PreserveWhitespace;

            var exml = new EncryptedXml(dDoc);
            exml.AddKeyNameMapping(keyName, algo);
            exml.DecryptDocument();

            return dDoc;
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="method">The method.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, XmlElement elementToEncrypt, string method, RSA algo, string keyName)
        {
            var sessionKey = new RijndaelManaged() { KeySize = 256 };
            try
            {
                var docCopy = (XmlDocument)xDoc.Clone();
                docCopy.PreserveWhitespace = xDoc.PreserveWhitespace;

                var eXml = new EncryptedXml();
                var encryptedElement = eXml.EncryptData(elementToEncrypt, sessionKey, false);

                var ek = new EncryptedKey();
                var encryptedKey = EncryptedXml.EncryptKey(sessionKey.Key, algo, false);

                ek.CipherData = new CipherData(encryptedKey);
                ek.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSA15Url);

                var edElement = new EncryptedData() { Type = EncryptedXml.XmlEncElementUrl, EncryptionMethod = new EncryptionMethod(method), KeyInfo = new KeyInfo() };
                var kin = new KeyInfoName(keyName);

                ek.KeyInfo.AddClause(kin);
                edElement.KeyInfo.AddClause(new KeyInfoEncryptedKey(ek));
                edElement.CipherData.CipherValue = encryptedElement;

                EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);

                if (xDoc.FirstChild is XmlDeclaration)
                {
                    xDoc.RemoveChild(xDoc.FirstChild);
                }

                var rDoc = xDoc;
                xDoc = docCopy;

                return rDoc;
            }
            finally
            {
                sessionKey.Clear();
            }
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="method">The method.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, string method, RSA algo, string keyName)
        {
            return Encrypt(ref xDoc, xDoc.DocumentElement, method, algo, keyName);
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, XmlElement elementToEncrypt, RSA algo, string keyName)
        {
            return Encrypt(ref xDoc, elementToEncrypt, EncryptedXml.XmlEncAES256Url, algo, keyName);
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, RSA algo, string keyName)
        {
            return Encrypt(ref xDoc, xDoc.DocumentElement, EncryptedXml.XmlEncAES256Url, algo, keyName);
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="method">The method.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, string method, X509Certificate2 cert, string keyName)
        {
            return Encrypt(ref xDoc, xDoc.DocumentElement, method, (RSA)cert.PublicKey.Key, keyName);
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, XmlElement elementToEncrypt, X509Certificate2 cert, string keyName)
        {
            return Encrypt(ref xDoc, elementToEncrypt, EncryptedXml.XmlEncAES256Url, (RSA)cert.PublicKey.Key, keyName);
        }

        /// <summary>
        /// Encrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Encrypt(ref XmlDocument xDoc, X509Certificate2 cert, string keyName)
        {
            return Encrypt(ref xDoc, xDoc.DocumentElement, EncryptedXml.XmlEncAES256Url, (RSA)cert.PublicKey.Key, keyName);
        }

        /// <summary>
        /// Decrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Decrypt(XmlDocument xDoc, RSA algo, string keyName)
        {
            var dDoc = (XmlDocument)xDoc.Clone();
            dDoc.PreserveWhitespace = xDoc.PreserveWhitespace;

            var exml = new EncryptedXml(dDoc);
            exml.AddKeyNameMapping(keyName, algo);
            exml.DecryptDocument();

            return dDoc;
        }

        /// <summary>
        /// Decrypts the specified x document.
        /// </summary>
        /// <param name="xDoc">The x document.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlDocument Decrypt(XmlDocument xDoc, X509Certificate2 cert, string keyName)
        {
            return Decrypt(xDoc, (RSA)cert.PrivateKey, keyName);
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, XmlElement elementToEncrypt, SymmetricAlgorithm algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, elementToEncrypt, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, SymmetricAlgorithm algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, xDoc.DocumentElement, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Decrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Decrypt(XmlNode xNode, SymmetricAlgorithm algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Decrypt( xDoc, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="method">The method.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, XmlElement elementToEncrypt, string method, RSA algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, elementToEncrypt, method, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="method">The method.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, string method, RSA algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, xDoc.DocumentElement, method, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, XmlElement elementToEncrypt, RSA algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, elementToEncrypt, EncryptedXml.XmlEncAES256Url, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt( XmlNode xNode, RSA algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, xDoc.DocumentElement, EncryptedXml.XmlEncAES256Url, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="method">The method.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, string method, X509Certificate2 cert, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, xDoc.DocumentElement, method, (RSA)cert.PublicKey.Key, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="elementToEncrypt">The element to encrypt.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, XmlElement elementToEncrypt, X509Certificate2 cert, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, elementToEncrypt, EncryptedXml.XmlEncAES256Url, (RSA)cert.PublicKey.Key, keyName).DocumentElement;
        }

        /// <summary>
        /// Encrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Encrypt(XmlNode xNode, X509Certificate2 cert, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Encrypt(ref xDoc, xDoc.DocumentElement, EncryptedXml.XmlEncAES256Url, (RSA)cert.PublicKey.Key, keyName).DocumentElement;
        }

        /// <summary>
        /// Decrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Decrypt(XmlNode xNode, RSA algo, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Decrypt(xDoc, algo, keyName).DocumentElement;
        }

        /// <summary>
        /// Decrypts the specified x node.
        /// </summary>
        /// <param name="xNode">The x node.</param>
        /// <param name="cert">The cert.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public static XmlNode Decrypt(XmlNode xNode, X509Certificate2 cert, string keyName)
        {
            var xDoc = CreateXmlDocumentFromNode(xNode);

            return Decrypt(xDoc, (RSA)cert.PrivateKey, keyName).DocumentElement;
        }
    }
}
