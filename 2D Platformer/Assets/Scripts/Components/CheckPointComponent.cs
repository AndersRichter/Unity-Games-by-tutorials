using Model;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;
        [SerializeField] private SpawnComponent _heroSpawn;

        private GameSession _gameSession;

        public string Id => _id;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();

            if (_gameSession.IsChecked(_id))
            {
                _setChecked?.Invoke();
            }
            else
            {
                _setUnchecked?.Invoke();
            }
        }

        public void Check()
        {
            _gameSession.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawn.Spawn();
        }
    }
}