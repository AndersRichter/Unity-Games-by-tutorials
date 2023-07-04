using Model.Data.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class AudioSettingWidget : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _text;

        private FloatPersistentProperty _model;

        private void Start()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _model.Value = value;
        }

        public void SubscribeOnModelChanged(FloatPersistentProperty model)
        {
            _model = model;
            model.OnChanged += OnValueChanged;
            OnValueChanged(model.Value, model.Value);
        }

        private void OnValueChanged(float newValue, float oldValue)
        {
            var textValue = Mathf.Round(newValue * 100);
            _text.text = textValue.ToString();
            _slider.normalizedValue = newValue;
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _model.OnChanged -= OnValueChanged;
        }
    }
}