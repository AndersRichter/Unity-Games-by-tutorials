using Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;

        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            LocalizationManager.Instance.OnLocaleChanged += OnLocaleChanged;
            Localize();
        }

        private void OnLocaleChanged()
        {
            Localize();
        }

        private void Localize()
        {
            _text.text = LocalizationManager.Instance.GetLocalizedString(_key);
        }

        private void OnDestroy()
        {
            LocalizationManager.Instance.OnLocaleChanged -= OnLocaleChanged;
        }
    }
}