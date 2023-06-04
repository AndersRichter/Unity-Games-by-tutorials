using System.Linq;
using Model.Data;
using UnityEngine;

namespace Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        [HideInInspector] public PlayerData LevelStartPlayerData = new();

        public PlayerData PlayerData => _playerData;

        private void Awake()
        {
            var existedSession = IsSessionExist();
            if (existedSession)
            {
                existedSession.LevelStartPlayerData.Initialize(existedSession.PlayerData);
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                LevelStartPlayerData.Initialize(_playerData);
            }
        }

        private GameSession IsSessionExist()
        {
            var sessions = FindObjectsOfType<GameSession>();
            return sessions.FirstOrDefault(session => session != this);
        }

        public void LevelReload()
        {
            _playerData.Initialize(LevelStartPlayerData);
        }
    }
}