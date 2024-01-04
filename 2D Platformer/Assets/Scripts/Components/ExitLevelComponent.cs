using UI;
using UnityEngine;

namespace Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        
        public void ExitLevel()
        {
            if (sceneName != null)
            {
                var loader = FindObjectOfType<LevelLoaderControllerComponent>();
                loader.Show(sceneName);
            }
        }
    }
}