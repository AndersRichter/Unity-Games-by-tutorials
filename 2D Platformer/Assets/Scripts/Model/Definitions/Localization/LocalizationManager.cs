using System;
using System.Collections.Generic;
using Model.Data.Properties;
using UnityEngine;

namespace Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public static readonly LocalizationManager Instance;

        private StringPersistentProperty _currentLocaleKey = new("En", "localization/current");
        private Dictionary<string, string> _locale;

        public string CurrentLocale => _currentLocaleKey.Value;

        public event Action OnLocaleChanged;

        static LocalizationManager()
        {
            Instance = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_currentLocaleKey.Value);
        }

        public void ChangeLocale(string newLocale)
        {
            _currentLocaleKey.Value = newLocale;
            LoadLocale(_currentLocaleKey.Value);
        }

        private void LoadLocale(string locale)
        {
            _locale = Resources.Load<LocaleDefinition>($"Locales/{locale}").GetData();
            OnLocaleChanged?.Invoke();
        }

        public string GetLocalizedString(string key)
        {
            if (_locale.TryGetValue(key, out var value))
            {
                return value;
            }

            return $"!!%{key}%!!";
        }
    }
}