using System.Collections.Generic;
using System.Linq;
using Components;
using Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private string _defaultCheckPoint;

        [HideInInspector] public PlayerData LevelStartPlayerData = new();

        public PlayerData PlayerData => _playerData;
        public QuickInventoryData QuickInventoryData { get; private set; }

        private readonly List<string> _checkpoints = new();

        private void Awake()
        {
            var existedSession = IsSessionExist();
            if (existedSession)
            {
                existedSession.LevelStartPlayerData.Initialize(existedSession.PlayerData);
                existedSession.StartSession(_defaultCheckPoint);
                DestroyImmediate(gameObject);
            }
            else
            {
                InitModels();
                DontDestroyOnLoad(this);
                LevelStartPlayerData.Initialize(_playerData);
                StartSession(_defaultCheckPoint);
            }
        }

        private void InitModels()
        {
            QuickInventoryData?.Dispose();
            QuickInventoryData = new QuickInventoryData(PlayerData);
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            SpawnHero(defaultCheckPoint);
            LoadHud();
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("GameHud", LoadSceneMode.Additive);
        }

        private void SpawnHero(string defaultCheckPoint)
        {
            var checkPointComponents = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();
            foreach (var checkPointComponent in checkPointComponents)
            {
                if (checkPointComponent.Id == lastCheckPoint)
                {
                    checkPointComponent.SpawnHero();
                    return;
                }
            }
            
            foreach (var checkPointComponent in checkPointComponents)
            {
                if (checkPointComponent.Id == defaultCheckPoint)
                {
                    checkPointComponent.SpawnHero();
                    return;
                }
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
            InitModels();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }
        
        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                LevelStartPlayerData.Initialize(_playerData);
                _checkpoints.Add(id);
            }
        }

        private void OnDestroy()
        {
            QuickInventoryData?.Dispose();
        }
    }
}