using UnityEngine;

namespace Components
{
    public class PlaySoundEffectComponent : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        
        private AudioSource _audioSource;

        public void Play()
        {
            if (!_audioSource)
            {
                _audioSource = GameObject.FindWithTag("SoundEffects").GetComponent<AudioSource>();
            }
            
            _audioSource.PlayOneShot(_clip);
        }
    }
}