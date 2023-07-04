using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Widgets
{
    public class ButtonSound : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _audioClip;

        private AudioSource _source;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_source)
            {
                _source = GameObject.FindWithTag("SoundEffects").GetComponent<AudioSource>();
            }
            
            _source.PlayOneShot(_audioClip);
        }
    }
}