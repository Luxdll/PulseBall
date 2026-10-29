using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private int score;

    private void Awake()
    {
        score = 0;
    }

    public void AddPoints(int points)
    {
        if (points <= 0)
            return;

        score += points;

        Debug.Log($"Очки: {score}");
    }
}