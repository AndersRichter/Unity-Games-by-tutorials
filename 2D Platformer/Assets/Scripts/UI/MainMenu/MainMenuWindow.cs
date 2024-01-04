using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindowComponent
    {
        public void OnShowSettingsWindow()
        {
            InstantiateWindow("UI/SettingsWindow", false);
        }

        public void OnStartNewGame()
        {
            OnPerformCloseCallback = () =>
            {
                var loader = FindObjectOfType<LevelLoaderControllerComponent>();
                loader.Show("Level1");
            };
            OnClose();
        }

        public void OnExit()
        {
            OnPerformCloseCallback = () =>
            {
                Application.Quit();
                
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }; 
            OnClose();
        }
    }
}