using System;
using System.Collections;
using Creatures.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _textSpeed = 0.09f;
        [SerializeField] private HeroInputEnableDisable _heroInputEnable;

        [Space] [Header("Sounds")]
        [SerializeField] private AudioClip _typingSound;
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;

        private DialogData _dialogData;
        private int _currentSentence;
        private AudioSource _audioSource;
        private Coroutine _typingCoroutine;
        
        private static readonly int AnimatorIsOpen = Animator.StringToHash("isOpen");

        private void Awake()
        {
            _audioSource = GameObject.FindWithTag("SoundEffects").GetComponent<AudioSource>();
        }

        public void ShowDialog(DialogData dialogData)
        {
            _dialogData = dialogData;
            _currentSentence = 0;
            _text.text = string.Empty;
            
            _container.SetActive(true);
            _audioSource.PlayOneShot(_openSound);
            _animator.SetBool(AnimatorIsOpen, true);
            _heroInputEnable.SetInput(false);
        }

        // used by animation event
        private void OnShowAnimationComplete()
        {
            _typingCoroutine = StartCoroutine(TypeDialogText());
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sentence = _dialogData.Sentences[_currentSentence];

            foreach (var letter in sentence)
            {
                _text.text += letter;
                _audioSource.PlayOneShot(_typingSound);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingCoroutine = null;
        }

        // used by animation event
        private void OnCloseAnimationComplete()
        {
            
        }

        public void OnSkip()
        {
            if (_typingCoroutine == null) return;
            
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
            _text.text = _dialogData.Sentences[_currentSentence];
        }

        public void OnContinue()
        {
            OnSkip();
            _currentSentence++;

            if (_currentSentence >= _dialogData.Sentences.Length)
            {
                _animator.SetBool(AnimatorIsOpen, false);
                _heroInputEnable.SetInput(true);
                _audioSource.PlayOneShot(_closeSound);
            }
            else
            {
                OnShowAnimationComplete();
            }
        }
    }

    [Serializable]
    public class DialogData
    {
        [SerializeField] private string[] _sentences;

        public string[] Sentences => _sentences;
    }
}