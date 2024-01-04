using System;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedWindowComponent : MonoBehaviour
    {
        [SerializeField] private bool _showOnStart = true;
        [SerializeField] private bool _destroyOnClose = true;
        
        protected Action OnPerformCloseCallback;
        
        private Animator _animator;

        private static readonly int AnimatorShow = Animator.StringToHash("Show");
        private static readonly int AnimatorHide = Animator.StringToHash("Hide");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();

            if (_showOnStart)
                _animator.SetTrigger(AnimatorShow);
        }

        public virtual void ShowWindow()
        {
            _animator.SetTrigger(AnimatorShow);
        }

        public void OnClose()
        {
            _animator.SetTrigger(AnimatorHide);
        }

        public void PerformCloseAnimationComplete()
        {
            OnPerformCloseCallback?.Invoke();
            if (_destroyOnClose)
                Destroy(gameObject);
        }

        protected void InstantiateWindow(string path, bool shouldClose = true)
        {
            var window = Resources.Load<GameObject>(path);
            var canvas = GameObject.FindGameObjectWithTag("MenuCanvas");

            if (canvas && window != null)
            {
                Instantiate(window, canvas.transform);
            }
            
            if (shouldClose)
                OnClose();
        }
    }
}