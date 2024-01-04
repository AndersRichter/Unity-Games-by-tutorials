using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LevelLoaderControllerComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _transitionTime;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            InitLoader();
        }

        private static readonly int AnimatorEnabled = Animator.StringToHash("Enabled");

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private static void InitLoader()
        {
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);
        }

        public void Show(string sceneName)
        {
            StartCoroutine(StartAnimation(sceneName));
        }

        private IEnumerator StartAnimation(string sceneName)
        {
            _animator.SetBool(AnimatorEnabled, true);
            yield return new WaitForSeconds(_transitionTime);
            SceneManager.LoadScene(sceneName);
            _animator.SetBool(AnimatorEnabled, false);
            
            // If scene is really heavy and we need to track progress
            // var _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            // _asyncOperation.progress; <-- Value of progress, we can use it in progress bar or smth else
            // _asyncOperation.completed += OnComplete;
        }
    }
}