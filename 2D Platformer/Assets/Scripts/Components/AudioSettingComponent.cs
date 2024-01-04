using System;
using Model.Data;
using Model.Data.Properties;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingComponent : MonoBehaviour
    {
        [SerializeField] private SoundSettingsKeys _settingKey;

        private FloatPersistentProperty _model;
        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            
            switch (_settingKey)
            {
                case SoundSettingsKeys.MusicVolume:
                    _model = GameSettings.Instance.MusicVolume;
                    break;
                case SoundSettingsKeys.SoundEffectsVolume:
                    _model = GameSettings.Instance.SoundEffectsVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _model.OnChanged += OnSoundSettingChanged;
            OnSoundSettingChanged(_model.Value, _model.Value);
        }

        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _audioSource.volume = newValue;
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.OnChanged -= OnSoundSettingChanged;
            }
        }
    }
}