using Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void ReloadLevel()
        {
            var gameSession = GameSession.Instance;
            gameSession.LevelReload();
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
