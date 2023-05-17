using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class CooldownUtils
    {
        [SerializeField] private float value;

        private float _timesUp;

        public void Reset()
        {
            _timesUp = Time.time + value;
        }

        public bool IsReady => _timesUp <= Time.time;
    }
}