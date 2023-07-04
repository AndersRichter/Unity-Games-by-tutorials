using Model.Data.Properties;
using UnityEngine;

namespace Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private FloatPersistentProperty _musicVolume;
        [SerializeField] private FloatPersistentProperty _soundEffectsVolume;

        public FloatPersistentProperty MusicVolume => _musicVolume;
        public FloatPersistentProperty SoundEffectsVolume => _soundEffectsVolume;

        private static GameSettings _instance;
        
        // Singleton pattern - when we always return the same class instance when it is requested
        public static GameSettings Instance => _instance == null ? LoadGameSettings() : _instance;

        private static GameSettings LoadGameSettings()
        {
            // Loading settings as a resource
            _instance = Resources.Load<GameSettings>("GameSettings");
            return _instance;
        }

        private void OnEnable()
        {
            _musicVolume = new FloatPersistentProperty(1, SoundSettingsKeys.MusicVolume.ToString());
            _soundEffectsVolume = new FloatPersistentProperty(1, SoundSettingsKeys.SoundEffectsVolume.ToString());
        }

        private void OnValidate()
        {
            _musicVolume.Validate();
            _soundEffectsVolume.Validate();
        }
    }

    public enum SoundSettingsKeys
    {
        MusicVolume,
        SoundEffectsVolume,
    }
}