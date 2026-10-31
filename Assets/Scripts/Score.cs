using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private UnityEngine.UI.Text scoreText;

    private int displayedScore = -1;

    private void Update()
    {
        if (playerScore == null || scoreText == null)
            return;

        int score = playerScore.Score;

        if (score == displayedScore)
            return;

        displayedScore = score;
        scoreText.text = $"Очки: {score}";
    }
}