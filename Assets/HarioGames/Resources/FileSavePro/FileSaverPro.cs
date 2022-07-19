using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace HarioGames.FileSaverPro
{
    /// <summary>
    /// Helps saving file with file path
    /// </summary>
    public class FileSaverPro
    {
        public FileFormat fileFormat;

        #region Delegates
        private delegate void SaveEvent<T>(string filePath, T toSave);
        private delegate T LoadEvent<T>(string filePath);
        #endregion

        #region Constructor
        /// <summary>
        /// Helps saving files in different format
        /// </summary>
        /// <param name="fileFormat">Format to save the file</param>
        public FileSaverPro(FileFormat fileFormat)
        {
            this.fileFormat = fileFormat;
        }
        #endregion

        #region WRITE

        /// <summary>
        /// Write data to the file
        /// </summary>
        /// <typeparam name="T">Data type or object</typeparam>
        /// <param name="filePath">Path of file to save</param>
        /// <param name="toSave">Data to save</param>
        public void WriteToFile<T>(string filePath, T toSave)
        {
            SaveEvent<T> saveEvent = null;
            switch (fileFormat)
            {
                case FileFormat.Binary:
                    saveEvent = WriteInBinary;
                    break;
                case FileFormat.Xml:
                    saveEvent = WriteInXml;
                    break;
                case FileFormat.Json:
                    saveEvent = WriteInJson;
                    break;
                case FileFormat.Md5Encrypted:
                    saveEvent = WriteInMd5Encrypted;
                    break;
            }
            saveEvent.Invoke(filePath, toSave);
        }

        #region Binary

        private void WriteInBinary<T>(string filePath, T toSave)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, toSave);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving in binary \n" + e);
            }
        }

        #endregion

        #region XML

        private void WriteInXml<T>(string filePath, T toSave)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextWriter textWriter = new StreamWriter(filePath))
                {
                    serializer.Serialize(textWriter, toSave);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving in Xml \n" + e);
            }
        }

        #endregion

        #region JSON

        private void WriteInJson<T>(string filePath, T toSave)
        {
            try
            {
                string jsonFromString = JsonUtility.ToJson(toSave);
                File.WriteAllText(filePath, jsonFromString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving in Json \n" + e);
            }
        }

        #endregion

        #region Md5Encrypted

        private void WriteInMd5Encrypted<T>(string filePath, T toSave)
        {
            try
            {
                Encryption.Encryptor cryptography = new Encryption.Encryptor();

                string encryptedString = cryptography.Encrypt<T>(toSave);

                File.WriteAllText(filePath, encryptedString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving and encrypting \n" + e);
            }
        }

        #endregion

        #endregion

        #region READ

        /// <summary>
        /// Read data from the file
        /// </summary>
        /// <typeparam name="T">Data type or object</typeparam>
        /// <param name="filePath">Path of file that you want to read from</param>
        /// <returns></returns>
        public T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default(T);

            return ReadFile<T>(filePath);
        }

        /// <summary>
        /// Read data from the file
        /// </summary>
        /// <typeparam name="T">Data type or object</typeparam>
        /// <param name="filePath">Path of file that you want to read from</param>
        /// <param name="defaultValue">Default data</param>
        /// <returns></returns>
        public T ReadFromFile<T>(string filePath, T defaultValue)
        {
            if (!File.Exists(filePath))
                return defaultValue;

            return ReadFile<T>(filePath);
        }

        private T ReadFile<T>(string filePath)
        {
            LoadEvent<T> loadEvent = null;
            switch (fileFormat)
            {
                case FileFormat.Binary:
                    loadEvent = ReadFromBinary<T>;
                    break;
                case FileFormat.Xml:
                    loadEvent = ReadFromXml<T>;
                    break;
                case FileFormat.Json:
                    loadEvent = ReadFromJson<T>;
                    break;
                case FileFormat.Md5Encrypted:
                    loadEvent = ReadFromMd5Encrypted<T>;
                    break;
            }
            return loadEvent.Invoke(filePath);
        }

        #region Binary

        private T ReadFromBinary<T>(string filePath)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading from binary \n" + e);
                return default(T);
            }
        }

        #endregion

        #region XML

        private T ReadFromXml<T>(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader textReader = new StreamReader(filePath))
                {
                    return (T)serializer.Deserialize(textReader);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading from xml \n" + e);
                return default(T);
            }
        }

        #endregion

        #region JSON

        private T ReadFromJson<T>(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(jsonString);
            }
            catch (Exception exc)
            {
                Debug.LogError("Error while loading from json \n" + exc);
                return default(T);
            }
        }

        #endregion

        #region Md5Encrypted

        private T ReadFromMd5Encrypted<T>(string filePath)
        {
            try
            {
                Encryption.Encryptor cryptography = new Encryption.Encryptor();
                string encryptedString = File.ReadAllText(filePath);
                return cryptography.Decrypt<T>(encryptedString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while decrypting and loading \n" + e);
                return default(T);
            }
        }

        #endregion

        #endregion
    }
}
