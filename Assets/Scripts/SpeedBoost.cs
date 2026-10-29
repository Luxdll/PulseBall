using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField, Min(1.01f)] private float multiplier = 1.5f;
    [SerializeField, Min(0.1f)] private float duration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement movement = other.GetComponent<PlayerMovement>();

        if (movement == null)
            return;

        movement.ApplySpeedBoost(multiplier, duration);
        Destroy(gameObject);
    }
}