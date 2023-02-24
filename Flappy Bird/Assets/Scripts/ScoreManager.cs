using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // [SerializeField] - to see private field in Unity editor
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextResult;
    [SerializeField] private TextMeshProUGUI betsScore;
    [SerializeField] private TextMeshProUGUI medalText;
    public Medal[] medals;
    private int score;

    // Start is called before the first frame update
    private void Start()
    {
        SetScore(0);
    }

    public void SetUpLoseWindow()
    {
        CheckAndSetBestScore();
        SetMedal();
    }

    public void SetScore(int score)
    {
        this.score += score;
        scoreText.text = "SCORE: " + this.score;
        scoreTextResult.text = "YOUR SCORE: " + this.score;
    }

    private void CheckAndSetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        betsScore.text = "BEST SCORE: " + bestScore;
    }

    private void SetMedal()
    {
        foreach (var medal in medals)
        {
            if (medal.ScoreNeeded <= score)
            {
                medalText.text = medal.MedalText;
                medalText.color = medal.MedalColor;
            }
        }
    }
}
