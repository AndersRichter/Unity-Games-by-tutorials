using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        
        public void ExitLevel()
        {
            if (sceneName != null)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}