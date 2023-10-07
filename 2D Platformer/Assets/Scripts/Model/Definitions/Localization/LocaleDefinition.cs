using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Definitions/LocaleDefinition", fileName = "LocaleDefinition")]

    public class LocaleDefinition : ScriptableObject
    {
        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in _localeItems)
            {
                dictionary.Add(item.Key, item.Value);
            }

            return dictionary;
        }

        [ContextMenu("Update locale")]
        public void UpdateLocale()
        {
            if (_request != null) return;

            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoad;
        }

        private void OnDataLoad(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                _localeItems.Clear();
                var rows = _request.downloadHandler.text.Split('\n');

                foreach (var row in rows)
                {
                    AddLocaleItem(row, _localeItems);
                }
            }
        }

        private void AddLocaleItem(string row, List<LocaleItem> items)
        {
            try
            {
                var parts = row.Split('\t');
                items.Add(new LocaleItem { Key = parts[0], Value = parts[1] });
            }
            catch (Exception exception)
            {
                Debug.LogError("Can't parse row: " + row + " \n" + exception);
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }

    public enum LocalesEnum
    {
        En,
        Ru,
    }
}