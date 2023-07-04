using Model.Data;
using UI.Widgets;
using UnityEngine;

namespace UI.Settings
{
    public class SettingsWindow : AnimatedWindowComponent
    {
        [SerializeField] private AudioSettingWidget _music;
        [SerializeField] private AudioSettingWidget _soundEffects;
        
        protected override void Start()
        {
            base.Start();

            _music.SubscribeOnModelChanged(GameSettings.Instance.MusicVolume);
            _soundEffects.SubscribeOnModelChanged(GameSettings.Instance.SoundEffectsVolume);
        }

        public void SaveSettings()
        {
            OnClose();
        }
    }
}