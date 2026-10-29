using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField, Min(1)] private int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        health.Heal(healAmount);
        Destroy(gameObject);
    }
}