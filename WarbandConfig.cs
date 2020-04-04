using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace WarbandCasualtyLog
{
    public static class WarbandConfig
    {
        public const string DefaultFriendlyKill = "#9AD7B4FF";
        public const string DefaultFriendlyKilled = "#AF6353FF"; 
        public const string DefaultFriendlyUnconscious = "#FFA862FF";

        private static string _fileName;


        [WarbandConfig.ConfigPropertyString(9, 9, "#")]
        public static string FriendlyKill { get; set; } = DefaultFriendlyKill;

        [WarbandConfig.ConfigPropertyString(9, 9, "#")]
        public static string FriendlyKilled { get; set; } = DefaultFriendlyKilled;
        [WarbandConfig.ConfigPropertyString(9, 9, "#")]
        public static string FriendlyUnconscious { get; set; } = DefaultFriendlyUnconscious;

        public static void Initialize()
        {
            WarbandConfig._fileName = Utilities.GetConfigsPath() + "WarbandCasualtyLogConfig.txt";
            if (!File.Exists(WarbandConfig._fileName))
            {
                WarbandConfig.Save();
            }
            else
            {
                var streamReader = new StreamReader(WarbandConfig._fileName);
                var text = streamReader.ReadToEnd();
                streamReader.Close();
                if (string.IsNullOrEmpty(text))
                {
                    WarbandConfig.Save();
                }
                else
                {
                    var array = text.Split('\n');
                    for (var i = 0; i < array.Length; i++)
                    {
                        var array2 = array[i].Split('=');
                        var property = typeof(WarbandConfig).GetProperty(array2[0]);
                        if (property == null)
                        {
                            SubModule.invalidConfigFlag = true;
                        }
                        else
                        {
                            var text2 = array2[1];
                            try
                            {
                                if (property.PropertyType == typeof(string))
                                {
                                    var customAttribute = property.GetCustomAttribute<WarbandConfig.ConfigPropertyString>();
                                    if (customAttribute.IsValidValue(text2))
                                    {
                                        property.SetValue(null, text2);
                                    }
                                    else
                                    {
                                        SubModule.invalidConfigFlag = true;
                                    }
                                }
                                else
                                {
                                    SubModule.invalidConfigFlag = true;
                                }
                            }
                            catch
                            {
                                SubModule.invalidConfigFlag = true;
                            }
                        }
                    }
                }
            }
        }

        public static void Save()
        {
            var dictionary = new Dictionary<PropertyInfo, object>();
            foreach (var propertyInfo in typeof(WarbandConfig).GetProperties())
                if (propertyInfo.GetCustomAttribute<WarbandConfig.ConfigProperty>() != null)
                    dictionary.Add(propertyInfo, propertyInfo.GetValue(null, null));
            var text = "";
            foreach (var keyValuePair in dictionary)
                text = string.Concat(text, keyValuePair.Key.Name, "=", keyValuePair.Value.ToString(), "\n");
            try
            {
                File.WriteAllText(WarbandConfig._fileName, text.Substring(0, text.Length - 1));
            }
            catch
            {
                Trace.WriteLine("Could not create Warband Casualty Log Config file");
            }
        }

        private abstract class ConfigProperty : Attribute
        {
        }

        private sealed class ConfigPropertyString : WarbandConfig.ConfigProperty
        {
            private readonly int _minLength;
            private readonly int _maxLength;
            private readonly string _reqPrefix;

            public ConfigPropertyString(int minLength = 0, int maxLength = 10, string requiredPrefix = "")
            {
                this._maxLength = maxLength;
                this._minLength = minLength;
                this._reqPrefix = requiredPrefix;
            }
            public bool IsValidValue(string value)
            {
                if(!_reqPrefix.Equals("") && value.StartsWith(_reqPrefix))
                    return value.Length >= _minLength && value.Length <= _maxLength;
                return false;
            }
        }
    }
}