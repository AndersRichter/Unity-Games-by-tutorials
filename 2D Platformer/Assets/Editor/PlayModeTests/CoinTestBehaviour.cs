using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.PlayModeTests
{
    public class CoinTestBehaviour : MonoBehaviour, IMonoBehaviourTest
    {
        public bool IsTestFinished => _timeToLive <= 0;

        private float _timeToLive = 1.2f;
        private GameObject _hero;

        private void Awake()
        {
            _hero = GameObject.FindWithTag("Player");
        }
        
        void Update()
        {
            _timeToLive -= Time.deltaTime;

            if (_timeToLive > 0)
            {
                _hero.SendMessage("SetDirection", new Vector2(1, 0));
            }
        }
    }
}
