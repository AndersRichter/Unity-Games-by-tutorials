using System;
using System.Collections.Generic;
using Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Localization
{
    public class LocaleDropdown : MonoBehaviour
    {
        private Dropdown _dropdown;

        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();

            SetOptionsFromLocaleEnum();
            SetValueByCurrentLocale();
        }
        
        private void SetOptionsFromLocaleEnum()
        {
            List<Dropdown.OptionData> newOptions = new List<Dropdown.OptionData>();
            var localesValues = Enum.GetValues(typeof(LocalesEnum));
    
            for(int i = 0; i < localesValues.Length; i++)
            {
                 newOptions.Add(new Dropdown.OptionData(Enum.GetName(typeof(LocalesEnum), i)));
            }
    
            _dropdown.ClearOptions();
            _dropdown.AddOptions(newOptions);
        }

        private void SetValueByCurrentLocale()
        {
            var currentLocale = LocalizationManager.Instance.CurrentLocale;
            _dropdown.value = (int)Enum.Parse(typeof(LocalesEnum), currentLocale);
        }

        public void OnLocaleChange(int newLocale)
        {
            LocalizationManager.Instance.ChangeLocale(((LocalesEnum)newLocale).ToString());
        }
    }
}