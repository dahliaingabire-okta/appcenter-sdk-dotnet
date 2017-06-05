﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Azure.Mobile.Utils
{
    /*
     * Application settings implemented in-memory with no persistence
     */
    [ExcludeFromCodeCoverage]
    public class ApplicationSettings : IApplicationSettings
    {
        private static readonly Dictionary<object, object> Settings = new Dictionary<object, object>();
        
        public T GetValue<T>(string key, T defaultValue)
        {
            object result;
            var found = Settings.TryGetValue(key, out result);
            if (found)
            {
                return (T) result;
            }
            SetValue(key, defaultValue);
            return defaultValue;
        }

        public void SetValue(string key, object value)
        {
            Settings[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return Settings.ContainsKey(key);
        }

        public void Remove(string key)
        {
            Settings.Remove(key);
        }

        public static void Reset()
        {
            Settings.Clear();
        }

        public static bool IsEmpty => Settings.Count == 0;
    }
}
