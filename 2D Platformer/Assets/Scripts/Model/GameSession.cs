using System.Linq;
using Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        [HideInInspector] public PlayerData LevelStartPlayerData = new();

        public PlayerData PlayerData => _playerData;
        public QuickInventoryData QuickInventoryData { get; private set; }

        private void Awake()
        {
            LoadHud();
            
            var existedSession = IsSessionExist();
            if (existedSession)
            {
                existedSession.LevelStartPlayerData.Initialize(existedSession.PlayerData);
                DestroyImmediate(gameObject);
            }
            else
            {
                InitModels();
                DontDestroyOnLoad(this);
                LevelStartPlayerData.Initialize(_playerData);
            }
        }

        private void InitModels()
        {
            QuickInventoryData = new QuickInventoryData(PlayerData);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("GameHud", LoadSceneMode.Additive);
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

        private void OnDestroy()
        {
            QuickInventoryData.Dispose();
        }
    }
}