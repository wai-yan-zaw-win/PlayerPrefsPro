using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace HarioGames.Encryption
{
    public class Encryptor
    {
        private static string defaultKey = "?H@&i0G@^^e$";
        private string key;

        public string Key { get => key; set => key = value; }

        #region Constructors
        /// <summary>
        /// Cryptography by HARIO GAMES
        /// </summary>
        public Encryptor()
        {
            Key = defaultKey;
        }

        /// <summary>
        /// Cryptography by HARIO GAMES
        /// </summary>
        /// <param name="key">Encryption Key</param>
        public Encryptor(string key)
        {
            this.Key = key;
        } 
        #endregion

        #region Encrypt
        /// <summary>
        /// Encrypt any object or data types to string
        /// </summary>
        /// <typeparam name="T">Object or data types</typeparam>
        /// <param name="toEncrypt">Object to encrypt</param>
        /// <returns></returns>
        public string Encrypt<T>(T toEncrypt)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(toEncrypt.GetType());
                MemoryStream memStrm = new MemoryStream();
                UTF8Encoding utf8e = new UTF8Encoding();
                XmlTextWriter xmlSink = new XmlTextWriter(memStrm, utf8e);
                xmlSerializer.Serialize(xmlSink, toEncrypt);
                byte[] utf8EncodedData = memStrm.ToArray();
                return EncryptString(utf8e.GetString(utf8EncodedData));
            }
            catch (Exception exc)
            {
                Debug.LogError("Error while encrypting " +
                    "\n" + exc);
                return "";
            }
        }

        /// <summary>
        /// Encrypts strings
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public string EncryptString(string toEncrypt)
        {
            byte[] input = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] output = Encrypt(input, Key);
            return Convert.ToBase64String(output);
        }

        private byte[] Encrypt(byte[] input, string encryptionKey)
        {
            try
            {
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider mD5Crypto = new MD5CryptoServiceProvider();

                byte[] key = mD5Crypto.ComputeHash(Encoding.ASCII.GetBytes(encryptionKey));
                byte[] iV = mD5Crypto.ComputeHash(Encoding.ASCII.GetBytes(encryptionKey));

                ICryptoTransform service = tripleDES.CreateEncryptor(key, iV);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, service, CryptoStreamMode.Write);

                cryptoStream.Write(input, 0, input.Length);
                cryptoStream.FlushFinalBlock();

                memoryStream.Position = 0;
                byte[] result = new byte[Convert.ToInt32(memoryStream.Length)];
                memoryStream.Read(result, 0, Convert.ToInt32(result.Length));

                memoryStream.Close();
                cryptoStream.Close();

                return result;

            }
            catch (Exception E)
            {
                Debug.LogError("Error while encrypting \n " + E);
                return new byte[0];
            }
        }
        #endregion

        #region Decrypt

        /// <summary>
        /// Decrypts any objects
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="toDecrypt">Encrypted string</param>
        /// <returns></returns>
        public T Decrypt<T>(string toDecrypt)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                using (TextReader textReader = new StringReader(DecryptString(toDecrypt)))
                {
                    return (T)xmlSerializer.Deserialize(textReader);
                }
            }
            catch (Exception exc)
            {
                Debug.LogError("Error while decrypting \n " + exc);
                return default(T);
            }
        }

        /// <summary>
        /// Decrypts strings
        /// </summary>
        /// <param name="toDecrypt">String to decrypt</param>
        /// <returns></returns>
        public string DecryptString(string toDecrypt)
        {
            byte[] input = Convert.FromBase64String(toDecrypt);
            byte[] output = Decrypt(input, Key);
            return Encoding.UTF8.GetString(output);
        }

        private byte[] Decrypt(byte[] input, string decryptionKey)
        {
            try
            {

                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider mD5Crypto = new MD5CryptoServiceProvider();

                byte[] key = mD5Crypto.ComputeHash(Encoding.ASCII.GetBytes(decryptionKey));
                byte[] iV = mD5Crypto.ComputeHash(Encoding.ASCII.GetBytes(decryptionKey));

                ICryptoTransform service = tripleDES.CreateDecryptor(key, iV);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, service, CryptoStreamMode.Write);

                cryptoStream.Write(input, 0, input.Length);
                cryptoStream.FlushFinalBlock();

                memoryStream.Position = 0;
                byte[] result = new byte[Convert.ToInt32(memoryStream.Length)];
                memoryStream.Read(result, 0, Convert.ToInt32(result.Length));

                memoryStream.Close();
                cryptoStream.Close();

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError("Error while decrypting \n " + e);
                return new byte[0];
            }
        }

        #endregion
    }
}
