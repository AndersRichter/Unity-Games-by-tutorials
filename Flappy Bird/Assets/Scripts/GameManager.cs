using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [CanBeNull] public GameObject losePanel;

    private void Start()
    {
        instance = this;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    public void Lose()
    {
        FindObjectOfType<ScoreManager>().SetUpLoseWindow();
        if (losePanel)
        {
            losePanel.SetActive(true);
        }

        Time.timeScale = 0;
        Cursor.visible = true;
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        Time.timeScale = 1;
        Cursor.visible = sceneNumber == 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
