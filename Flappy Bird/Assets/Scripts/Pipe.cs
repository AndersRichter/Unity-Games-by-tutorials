using System;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Player>(out Player player))
        {
            // or GetComponent<ScoreManager>().SetScore(1);
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            scoreManager.SetScore(1);
        }
    }
}
