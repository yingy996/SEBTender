using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Security.Cryptography;
using System.IO;

namespace SEBeTender
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                if (CrossSettings.IsSupported)
                {
                    return CrossSettings.Current;
                }
                return null;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private static readonly string NotificationDefault = "default";
        private static readonly string UsernameDefault = string.Empty;
        private static readonly string PasswordDefault = string.Empty;
        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string NotificationSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault("isSubscribed", NotificationDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("isSubscribed", value);
            }
        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("username", UsernameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("username", value);
            }
        }

        public static string Password
        {
            get
            {
                string password = AppSettings.GetValueOrDefault("password", PasswordDefault);
                string decryptedPassword = decryptString(password, "passwordseb");
                //return AppSettings.GetValueOrDefault("password", PasswordDefault);
                return decryptedPassword;
            }
            set
            {              
                if (String.IsNullOrEmpty(value))
                {
                    AppSettings.AddOrUpdateValue("password", value);
                } else
                {
                    string encryptedPassword = encryptString(value, "passwordseb");
                    AppSettings.AddOrUpdateValue("password", encryptedPassword);
                }
                
            }
        }

        private static string encryptString(string stringToEncrypt, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] salt = new byte[] {80, 70, 60, 50, 40, 30, 20, 10};
                int iterations = 300;
                var keyGenerator = new Rfc2898DeriveBytes(encryptionKey, salt, iterations);
                
                aes.Key = keyGenerator.GetBytes(32);

                if (String.IsNullOrEmpty(stringToEncrypt) || stringToEncrypt.Length <= 0)
                {
                    throw new ArgumentNullException($"{nameof(stringToEncrypt)}");
                }

                if (aes.Key == null || aes.Key.Length <= 0)
                {
                    throw new ArgumentNullException($"{nameof(aes.Key)}");
                }

                if (aes.IV == null || aes.IV.Length <= 0)
                {
                    throw new ArgumentNullException($"{nameof(aes.IV)}");
                }

                byte[] encrypted;
                using (Aes aes2 = Aes.Create())
                {
                    aes2.Key = aes.Key;
                    aes2.IV = aes.IV;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor())
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(stringToEncrypt);        
                        }
                        encrypted = memoryStream.ToArray();
                    }
                }
                //byte[] encryptedString = 
                return Convert.ToBase64String(encrypted) + ";" + Convert.ToBase64String(aes.IV);
            }
        }

        private static string decryptString(this string encryptedValue, string encryptionKey)
        {
            string iv = encryptedValue.Substring(encryptedValue.IndexOf(';') + 1, encryptedValue.Length - encryptedValue.IndexOf(';') - 1);
            encryptedValue = encryptedValue.Substring(0, encryptedValue.IndexOf(';'));

            byte[] cipherText = Convert.FromBase64String(encryptedValue);
            byte[] ivByte = Convert.FromBase64String(iv);

            byte[] salt = new byte[] { 80, 70, 60, 50, 40, 30, 20, 10 };
            int iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(encryptionKey, salt, iterations);

            byte[] key = keyGenerator.GetBytes(32);

            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException($"{nameof(cipherText)}");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException($"{nameof(key)}");
            }

            if (ivByte == null || ivByte.Length <= 0)
            {
                throw new ArgumentNullException($"{nameof(ivByte)}");
            }

            string plainText = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = ivByte;

                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    plainText = streamReader.ReadToEnd();
                }
            }
            return plainText;
        }

    }
}
