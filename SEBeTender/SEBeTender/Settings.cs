using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

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

    }
}
