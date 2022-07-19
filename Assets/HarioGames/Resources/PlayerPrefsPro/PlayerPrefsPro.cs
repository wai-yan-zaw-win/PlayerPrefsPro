using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;

namespace HarioGames.PlayerPrefsPro
{
    /// <summary>
    /// Stores and accesses any data types or objects in playerprefs between game sections
    /// </summary>
    /// Developed by Wai Yan Zaw Win
    /// Email : waiyanzawwinstar8@gmail.com
    /// LinkedIn : https://www.linkedin.com/in/wai-yan-zaw-win/
    /// GitHub : https://github.com/wai-yan-zaw-win/

    public static class PlayerPrefsPro
    {
        #region Delegates
        delegate void SaveEvent(string key, object value);
        delegate object LoadEvent(string key);
        delegate bool FoundEvent(string key);
        delegate void DeleteEvent(string key);

        delegate void EncryptedSaveEvent(string key, object value, string encryptionKey);
        delegate object EncryptedLoadEvent(string key, string encryptionKey);
        delegate bool EncryptedFoundEvent(string key, string encryptionKey);
        delegate void EncryptedDeleteEvent(string key, string encryptionKey);
        #endregion

        #region Save
        /// <summary>
        /// Save any object or data types in playerprefs
        /// </summary>
        /// <typeparam name="T">Object or data types</typeparam>
        /// <param name="key">Key for playerpref</param>
        /// <param name="value">Value to store</param>
        public static void Save<T>(string key, T value)
        {
            SaveEvent saveEvent = null;

            #region Common Data Types

            #region BUILD-IN PlayerPrefs Data Types
            if (value is int)
                saveEvent = SetInt;
            else if (value is float)
                saveEvent = SetFloat;
            else if (value is string)
                saveEvent = SetString;
            #endregion 

            else if (value is bool)
                saveEvent = SetBool;
            else if (value is byte)
                saveEvent = SetByte;
            else if (value is long)
                saveEvent = SetLong;
            else if (value is double)
                saveEvent = SetDouble;
            else if (value is short)
                saveEvent = SetShort;
            else if (value is char)
                saveEvent = SetChar;
            else if (value is DateTime)
                saveEvent = SetDateTime;

            #endregion

            #region Unsigned Data Types
            else if (value is uint)
                saveEvent = SetUnsignedInt;
            else if (value is ulong)
                saveEvent = SetUnsignedLong;
            else if (value is ushort)
                saveEvent = SetUnsignedShort;
            #endregion

            #region Object
            else if (value is Enum)
                saveEvent = SetEnum;
            else
                saveEvent = SetObject<T>;
            #endregion

            saveEvent.Invoke(key, value);
        }

        #region Saving

        #region Common Data Types Saving

        #region BUILD-IN Saving System
        //Unity Build-In PlayerPref Saving
        private static void SetInt(string key, object value)
        {
            PlayerPrefs.SetInt(key + "_int", (int)value);
        }
        private static void SetFloat(string key, object value)
        {
            PlayerPrefs.SetFloat(key + "_float", (float)value);
        }
        private static void SetString(string key, object value)
        {
            PlayerPrefs.SetString(key + "_string", (string)value);
        }
        #endregion

        private static void SetBool(string key, object value)
        {
            PlayerPrefs.SetString(key + "_bool", (bool)value == true ? "1" : "0");
        }
        private static void SetByte(string key, object value)
        {
            PlayerPrefs.SetString(key + "_byte", ((byte)value).ToString());
        }
        private static void SetLong(string key, object value)
        {
            PlayerPrefs.SetString(key + "_long", ((long)value).ToString());
        }
        private static void SetDouble(string key, object value)
        {
            PlayerPrefs.SetString(key + "_double", ((double)value).ToString());
        }
        private static void SetShort(string key, object value)
        {
            PlayerPrefs.SetInt(key + "_short", (short)value);
        }
        private static void SetChar(string key, object value)
        {
            PlayerPrefs.SetString(key + "_char", ((char)value).ToString());
        }
        private static void SetDateTime(string key, object value)
        {
            PlayerPrefs.SetString(key + "_dateTime", ((DateTime)value).ToString());
        }

        #endregion

        #region Unsigned Data Types Saving

        private static void SetUnsignedInt(string key, object value)
        {
            PlayerPrefs.SetInt(key + "_uint", (int)(uint)value);
        }
        private static void SetUnsignedShort(string key, object value)
        {
            PlayerPrefs.SetInt(key + "_ushort", (ushort)value);
        }
        private static void SetUnsignedLong(string key, object value)
        {
            SetLong(key + "_ulong", (long)(ulong)value);
        }

        #endregion

        #region Saving Object
        private static void SetEnum(string key, object value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        private static void SetObject<T>(string key, object value)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(value.GetType());
                MemoryStream memoryStream = new MemoryStream();
                UTF8Encoding utf8Encoder = new UTF8Encoding();
                XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, utf8Encoder);
                xmlSerializer.Serialize(xmlWriter, value);
                byte[] encodedData = memoryStream.ToArray();
                PlayerPrefs.SetString(key + "_" + value.GetType().Name, utf8Encoder.GetString(encodedData));
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving \n" + e);
            }
        }
        #endregion

        #endregion

        #endregion

        #region Load

        /// <summary>
        /// Load any saved objects or data types
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <returns></returns>
        public static T Load<T>(string key)
        {
            if (!HasKey<T>(key))
                return default(T);

            return LoadDatas<T>(key);
        }

        /// <summary>
        /// Load any saved objects or data types
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <param name="defaultData">Default data</param>
        /// <returns></returns>
        public static T Load<T>(string key, T defaultData)
        {
            if (!HasKey<T>(key))
                return defaultData;

            return LoadDatas<T>(key, defaultData);
        }

        #region Load Datas
        private static T LoadDatas<T>(string key, T defaultData = default(T))
        {
            LoadEvent loadEvent;

            Type dataType = typeof(T);

            #region Common Data Types

            #region Build-In PlayerPrefs Data Types
            if (dataType == typeof(int))
                loadEvent = GetInt;
            else if (dataType == typeof(float))
                loadEvent = GetFloat;
            else if (dataType == typeof(string))
                loadEvent = GetString;
            #endregion

            else if (dataType == typeof(bool))
                loadEvent = GetBool;
            else if (dataType == typeof(byte))
                loadEvent = GetByte;
            else if (dataType == typeof(long))
                loadEvent = GetLong;
            else if (dataType == typeof(double))
                loadEvent = GetDouble;
            else if (dataType == typeof(short))
                loadEvent = GetShort;
            else if (dataType == typeof(char))
                loadEvent = GetChar;
            else if (dataType == typeof(DateTime))
                loadEvent = GetDateTime;

            #endregion

            #region Unsigned Data Types
            else if (dataType == typeof(uint))
                loadEvent = GetUnsignedInt;
            else if (dataType == typeof(ulong))
                loadEvent = GetUnsignedLong;
            else if (dataType == typeof(ushort))
                loadEvent = GetUnsignedShort;
            #endregion

            #region Objects
            else
                loadEvent = GetObject<T>;
            #endregion

            T loadedData;

            if (dataType != typeof(Enum))
            {
                loadedData = (T)loadEvent.Invoke(key);
            }
            else
            {
                loadedData = GetEnum<T>(key);
            }

            return loadedData;
        }
        #endregion

        #region Loading

        #region Common Data Types Loading

        #region BUILD-IN Loadin System
        //Unity Build-In PlayerPrefs Loading System
        private static object GetInt(string key)
        {
            return PlayerPrefs.GetInt(key + "_int");
        }
        private static object GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key + "_float");
        }
        private static object GetString(string key)
        {
            return PlayerPrefs.GetString(key + "_string");
        }
        #endregion

        private static object GetBool(string key)
        {
            return PlayerPrefs.GetString(key + "_bool") == "1";
        }
        private static object GetLong(string key)
        {
            return long.Parse(PlayerPrefs.GetString(key + "_long"));
        }
        private static object GetDouble(string key)
        {
            return double.Parse(PlayerPrefs.GetString(key + "_double"));
        }
        private static object GetByte(string key)
        {
            return byte.Parse(PlayerPrefs.GetString(key + "_byte"));
        }
        private static object GetShort(string key)
        {
            return (short)PlayerPrefs.GetInt(key + "_short");
        }
        private static object GetChar(string key)
        {
            return PlayerPrefs.GetString(key + "_char")[0];
        }
        private static object GetDateTime(string key)
        {
            return DateTime.Parse(PlayerPrefs.GetString(key + "_dateTime"));
        }

        #endregion

        #region Unsigned Data Types Loading
        private static object GetUnsignedInt(string key)
        {
            return (uint)PlayerPrefs.GetInt(key + "_uint");
        }
        private static object GetUnsignedShort(string key)
        {
            return (ushort)PlayerPrefs.GetInt(key + "_ushort");
        }
        private static object GetUnsignedLong(string key)
        {
            return (ulong)(long)GetLong(key + "_ulong");
        }
        #endregion

        #region Objects Loading
        private static T GetEnum<T>(string key, T defaultValue = default(T))
        {
            string stringValue = PlayerPrefs.GetString(key);

            if (!string.IsNullOrEmpty(stringValue))
            {
                return (T)Enum.Parse(typeof(T), stringValue);
            }
            else
            {
                return defaultValue;
            }
        }

        private static object GetObject<T>(string key)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                using (TextReader textReader = new StringReader(PlayerPrefs.GetString(key + "_" + typeof(T).Name)))
                {
                    return (T)xmlSerializer.Deserialize(textReader);
                }
            }
            catch (Exception E)
            {
                Debug.LogError("Error while loading \n" + E);
                return default(T);
            }
        }
        #endregion

        #endregion

        #endregion

        #region Check Key

        /// <summary>
        /// Check if the key exits in playerprefs
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <param name="isEncrypted">is the data encrypted</param>
        /// <returns></returns>
        public static bool HasKey<T>(string key, bool isEncrypted = false)
        {
            FoundEvent foundEvent;

            Type dataType = typeof(T);

            if (isEncrypted)
            {
                foundEvent = FoundEncryptedObjectsKey<T>;
            }
            else
            {
                #region Common Data Types

                #region Build-In PlayerPrefs Data Types
                if (dataType == typeof(int))
                    foundEvent = FoundIntKey;
                else if (dataType == typeof(float))
                    foundEvent = FoundFloatKey;
                else if (dataType == typeof(string))
                    foundEvent = FoundStringKey;
                #endregion

                else if (dataType == typeof(bool))
                    foundEvent = FoundBoolKey;
                else if (dataType == typeof(byte))
                    foundEvent = FoundByteKey;
                else if (dataType == typeof(long))
                    foundEvent = FoundLongKey;
                else if (dataType == typeof(double))
                    foundEvent = FoundDoubleKey;
                else if (dataType == typeof(short))
                    foundEvent = FoundShortKey;
                else if (dataType == typeof(char))
                    foundEvent = FoundCharKey;
                else if (dataType == typeof(DateTime))
                    foundEvent = FoundDateTimeKey;

                #endregion

                #region Unsigned Data Types
                else if (dataType == typeof(uint))
                    foundEvent = FoundUnsignedIntKey;
                else if (dataType == typeof(ulong))
                    foundEvent = FoundUnsignedLongKey;
                else if (dataType == typeof(ushort))
                    foundEvent = FoundUnsignedShortKey;
                #endregion

                #region Objects
                else
                    foundEvent = FoundObjectsKey<T>;
                #endregion
            }

            return foundEvent.Invoke(key);
        }

        #region Checking Keys

        #region Common Data Types

        #region BUILD-IN PlayerPref HasKey
        //Build-In haskey function by unity
        private static bool FoundIntKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_int");
        }
        private static bool FoundFloatKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_float");
        }
        private static bool FoundStringKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_string");
        }
        #endregion

        private static bool FoundBoolKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_bool");
        }
        private static bool FoundDoubleKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_double");
        }
        private static bool FoundLongKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_long");
        }
        private static bool FoundByteKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_byte");
        }
        private static bool FoundShortKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_short");
        }
        private static bool FoundCharKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_char");
        }
        private static bool FoundDateTimeKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_dateTime");
        }

        #endregion

        #region Unsigned Data Types
        private static bool FoundUnsignedIntKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_uint");
        }
        private static bool FoundUnsignedShortKey(string key)
        {
            return PlayerPrefs.HasKey(key + "_ushort");
        }
        private static bool FoundUnsignedLongKey(string key)
        {
            return FoundLongKey(key + "_ulong");
        }
        #endregion

        #region Objects
        private static bool FoundObjectsKey<T>(string key)
        {
            return PlayerPrefs.HasKey(key + "_" + typeof(T).Name);
        }
        #endregion

        #region Encrypted Objects
        private static bool FoundEncryptedObjectsKey<T>(string key)
        {
            return PlayerPrefs.HasKey(key + "_Encrypted" + typeof(T).Name);
        }
        #endregion

        #endregion

        #endregion

        #region Delete

        /// <summary>
        /// Delete the data with key from playerprefs
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <param name="isEncrypted">is the data encrypted</param>
        public static void Delete<T>(string key, bool isEncrypted = false)
        {
            DeleteEvent deleteEvent;
            Type dataType = typeof(T);

            if (isEncrypted)
            {
                deleteEvent = DeleteEncryptedObject<T>;
            }
            else
            {
                #region Common Data Types

                #region Build-In PlayerPrefs Data Types
                if (dataType == typeof(int))
                    deleteEvent = DeleteInt;
                else if (dataType == typeof(float))
                    deleteEvent = DeleteFloat;
                else if (dataType == typeof(string))
                    deleteEvent = DeleteString;
                #endregion

                else if (dataType == typeof(bool))
                    deleteEvent = DeleteBool;
                else if (dataType == typeof(double))
                    deleteEvent = DeleteDouble;
                else if (dataType == typeof(long))
                    deleteEvent = DeleteLong;
                else if (dataType == typeof(byte))
                    deleteEvent = DeleteByte;
                else if (dataType == typeof(short))
                    deleteEvent = DeleteShort;
                else if (dataType == typeof(char))
                    deleteEvent = DeleteChar;
                else if (dataType == typeof(DateTime))
                    deleteEvent = DeleteDateTime;

                #endregion

                #region Unsigned Data Types
                else if (dataType == typeof(uint))
                    deleteEvent = DeleteUnsignedInt;
                else if (dataType == typeof(ulong))
                    deleteEvent = DeleteUnsignedLong;
                else if (dataType == typeof(ushort))
                    deleteEvent = DeleteUnsignedShort;
                #endregion

                #region Object
                else
                    deleteEvent = DeleteObject<T>;
                #endregion
            }

            deleteEvent.Invoke(key);
        }

        #region Deleting

        #region Common Data Types Deleting

        #region BUILD-IN PlayerPrefs Deleting

        private static void DeleteInt(string key)
        {
            PlayerPrefs.DeleteKey(key + "_int");
        }
        private static void DeleteFloat(string key)
        {
            PlayerPrefs.DeleteKey(key + "_float");
        }
        private static void DeleteString(string key)
        {
            PlayerPrefs.DeleteKey(key + "_" +
                "string");
        }

        #endregion

        private static void DeleteBool(string key)
        {
            PlayerPrefs.DeleteKey(key + "_bool");
        }
        private static void DeleteLong(string key)
        {
            PlayerPrefs.DeleteKey(key + "_long");
        }
        private static void DeleteDouble(string key)
        {
            PlayerPrefs.DeleteKey(key + "_double");
        }
        private static void DeleteByte(string key)
        {
            PlayerPrefs.DeleteKey(key + "_byte");
        }
        private static void DeleteShort(string key)
        {
            PlayerPrefs.DeleteKey(key + "_short");
        }
        private static void DeleteChar(string key)
        {
            PlayerPrefs.DeleteKey(key + "_char");
        }
        private static void DeleteDateTime(string key)
        {
            PlayerPrefs.DeleteKey(key + "_dateTime");
        }

        #endregion

        #region Unsigned Data Types Deleting

        private static void DeleteUnsignedInt(string key)
        {
            PlayerPrefs.DeleteKey(key + "_uint");
        }
        private static void DeleteUnsignedShort(string key)
        {
            PlayerPrefs.DeleteKey(key + "_ushort");
        }
        private static void DeleteUnsignedLong(string key)
        {
            DeleteLong(key + "_ulong");
        }

        #endregion

        #region Object

        private static void DeleteObject<T>(string key)
        {
            PlayerPrefs.DeleteKey(key + "_" + typeof(T).Name);
        }

        #endregion

        #region Encrypted Object

        private static void DeleteEncryptedObject<T>(string key)
        {
            PlayerPrefs.DeleteKey(key + "_Encrypted" + typeof(T).Name);
        }

        #endregion

        #endregion

        #endregion

        #region Encrypt_Decrypt

        #region Encrypted Save
        /// <summary>
        /// Save any objects or data types encrypted in playerprefs
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <param name="value">Value to save</param>
        /// <param name="encryptionKey">Key for encryption</param>
        public static void EncrytedSave<T>(string key, T value, string encryptionKey = "?H@&i0G@^^e$")
        {
            EncryptedSaveEvent saveEvent = null;

            saveEvent = SetObject<T>;

            saveEvent.Invoke(key, value, encryptionKey);

        }

        #region Saving and Encrypting Object
        private static void SetObject<T>(string key, object value, string encryptionKey)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(value.GetType());
                MemoryStream memoryStream = new MemoryStream();
                UTF8Encoding utf8Encoder = new UTF8Encoding();
                XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, utf8Encoder);
                xmlSerializer.Serialize(xmlWriter, value);
                byte[] encodedData = memoryStream.ToArray();
                Encryption.Encryptor cryptography = new Encryption.Encryptor(encryptionKey);
                string encryptedString = cryptography.EncryptString(utf8Encoder.GetString(encodedData));
                PlayerPrefs.SetString(key + "_Encrypted" + value.GetType().Name, encryptedString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving \n" + e);
            }
        }
        #endregion

        #endregion

        #region Encrypted Load

        /// <summary>
        /// Loading encrypted objects or data types from playerprefs
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <param name="decryptionKey">Key for decryption</param>
        /// <returns></returns>
        public static T EncryptedLoad<T>(string key, string decryptionKey = "?H@&i0G@^^e$")
        {
            if (!HasKey<T>(key, true))
                return default(T);

            return LoadEncryptedDatas<T>(key, decryptionKey);
        }

        /// <summary>
        /// Loading encrypted objects or data types from playerprefs
        /// </summary>
        /// <typeparam name="T">Object or data type</typeparam>
        /// <param name="key">Key for playerprefs</param>
        /// <param name="defaultData">Default data</param>
        /// <param name="decryptionKey">Key for decryption</param>
        /// <returns></returns>
        public static T EncryptedLoad<T>(string key, T defaultData, string decryptionKey = "?H@&i0G@^^e$")
        {
            if (!HasKey<T>(key, true))
                return defaultData;

            return LoadEncryptedDatas<T>(key, decryptionKey);
        }

        #region Load Encrypted Datas
        private static T LoadEncryptedDatas<T>(string key, string decryptionKey)
        {
            EncryptedLoadEvent loadEvent;

            Type dataType = typeof(T);

            loadEvent = GetObject<T>;

            object loadedDatas = loadEvent.Invoke(key, decryptionKey);
            return (T)loadedDatas;
        }
        #endregion

        #region Loading Encrypted Object

        private static object GetObject<T>(string key, string decryptionKey)
        {
            try
            {
                Type dataType = typeof(T);

                XmlSerializer xmlSerializer = new XmlSerializer(dataType);

                Encryption.Encryptor cryptography = new Encryption.Encryptor(decryptionKey);

                string decryptedString = cryptography.DecryptString(PlayerPrefs.GetString(key + "_Encrypted" + dataType.Name));

                using (TextReader textReader = new StringReader(decryptedString))
                {
                    return (T)xmlSerializer.Deserialize(textReader);
                }
            }
            catch (Exception E)
            {
                Debug.LogError("Error while loading \n" + E);
                return default(T);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}

