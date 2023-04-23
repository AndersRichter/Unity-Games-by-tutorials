using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimationComponent : MonoBehaviour
    {
        [Range(1, 60)] [SerializeField] private int frameRate;
        [SerializeField] private SpriteAnimation[] animations;
        [SerializeField] private UnityEvent<string> onComplete;

        private SpriteRenderer _spriteRenderer;
        
        private float _secondsPerFrame;
        private int _currentFrame;
        private float _nextFrameTime;
        private int _currentAnimation;
        private bool _isPlaying = true;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / frameRate;

            StartAnimation();
        }

        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible()
        {
            enabled = false;
        }

        public void SetAnimation(string animationName)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].Name == animationName)
                {
                    _currentAnimation = i;
                    StartAnimation();
                    return;
                }
            }

            enabled = _isPlaying = false;
        }

        private void OnEnable()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
        }
        
        private void StartAnimation()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
            enabled = _isPlaying = true;
            _currentFrame = 0;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time)
            {
                return;
            }

            var clip = animations[_currentAnimation];
            if (_currentFrame >= clip.Sprites.Length)
            {
                if (clip.Loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    enabled = _isPlaying = clip.AllowNext;
                    clip.OnComplete?.Invoke();
                    onComplete?.Invoke(clip.Name);

                    if (clip.AllowNext)
                    {
                        _currentFrame = 0;
                        _currentAnimation = (int) Mathf.Repeat(_currentAnimation + 1, animations.Length);
                    }
                }
                
                return;
            }

            _spriteRenderer.sprite = clip.Sprites[_currentFrame];
            _nextFrameTime += _secondsPerFrame;
            _currentFrame++;
        }
    }

    [Serializable]
    public class SpriteAnimation
    {
        public string Name;
        public bool Loop;
        public bool AllowNext;
        public Sprite[] Sprites;
        public UnityEvent OnComplete;
    }
}
