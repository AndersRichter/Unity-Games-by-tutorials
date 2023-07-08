using System;
using UnityEngine.Events;

namespace Utils.Disposable
{
    public static class UnityEventDisposable
    {
        public static IDisposable Subscribe(UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.AddListener(unityAction);
            return new ActionDisposable(() => unityEvent.RemoveListener(unityAction));
        }
        
        public static IDisposable Subscribe<T>(UnityEvent<T> unityEvent, UnityAction<T> unityAction)
        {
            unityEvent.AddListener(unityAction);
            return new ActionDisposable(() => unityEvent.RemoveListener(unityAction));
        }
    }
}