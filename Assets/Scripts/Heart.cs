using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private UnityEngine.UI.Text healthText;

    private int displayedHealth = -1;

    private void Update()
    {
        if (playerHealth == null || healthText == null)
            return;

        int health = playerHealth.CurrentHealth;

        if (health == displayedHealth)
            return;

        displayedHealth = health;
        healthText.text = "";

        for (int i = 0; i < health; i++)
        {
            if (i > 0)
                healthText.text += " ";

            healthText.text += "♥";
        }
    }
}