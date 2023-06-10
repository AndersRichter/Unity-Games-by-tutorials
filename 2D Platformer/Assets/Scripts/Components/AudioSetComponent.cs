using System;
using UnityEngine;

namespace Components
{
    public class AudioSetComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioSetItem[] _sounds;

        public void Play(string id)
        {
            if (!_source) return;
            
            foreach (var sound in _sounds)
            {
                if (sound.Id == id)
                {
                    _source.PlayOneShot(sound.Clip);
                }
            }
        }

        [Serializable]
        public class AudioSetItem
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip;

            public string Id => _id;
            public AudioClip Clip => _clip;
        }
    }
}