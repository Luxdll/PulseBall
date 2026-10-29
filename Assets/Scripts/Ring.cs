using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField, Min(1)] private int points = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScore playerScore = other.GetComponent<PlayerScore>();

        if (playerScore == null)
            return;

        playerScore.AddPoints(points);
        Destroy(gameObject);
    }
}
