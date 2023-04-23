using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        public PlayerData LevelStartPlayerData;

        public PlayerData PlayerData => playerData;

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
                LevelStartPlayerData.Initialize(playerData);
            }
        }

        [CanBeNull]
        private GameSession IsSessionExist()
        {
            var sessions = FindObjectsOfType<GameSession>();
            return sessions.FirstOrDefault(session => session != this);
        }

        public void LevelReload()
        {
            playerData.Initialize(LevelStartPlayerData);
        }
    }
}