using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] private TimerData[] timers;

        public void SetTimer(int index)
        {
            var timer = timers[index];

            StartCoroutine(StartTimer(timer));
        }

        private IEnumerator StartTimer(TimerData timer)
        {
            yield return new WaitForSeconds(timer.Delay);
            
            timer.OnTimerEnds?.Invoke();
        }

        [Serializable]
        public class TimerData
        {
            public float Delay;
            public UnityEvent OnTimerEnds;
        }
    }
}