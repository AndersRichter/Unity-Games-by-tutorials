using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            OnPerformCloseCallback = () => { SceneManager.LoadScene("Level1"); };
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