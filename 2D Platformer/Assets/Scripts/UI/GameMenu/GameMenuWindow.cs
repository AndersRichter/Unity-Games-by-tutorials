using Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.GameMenu
{
    public class GameMenuWindow : AnimatedWindowComponent
    {
        [SerializeField] private ReloadLevelComponent _reloadLevelComponent;
        
        public void OnShowSettingsWindow()
        {
            InstantiateWindow("UI/SettingsWindow", false);
        }

        public void GoToMainMenu()
        {
            OnPerformCloseCallback = () =>
            {
                UnpauseGame();
                SceneManager.LoadScene("MainMenu");
            };
            OnClose();
        }

        public override void ShowWindow()
        {
            Time.timeScale = 0f;
            OnPerformCloseCallback = UnpauseGame;
            base.ShowWindow();
        }

        public void RestartLevel()
        {
            OnPerformCloseCallback = () =>
            {
                UnpauseGame();
                if (_reloadLevelComponent != null)
                {
                    _reloadLevelComponent.ReloadLevel();
                }
            };
            OnClose();
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1f;
        }
    }
}