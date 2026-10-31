using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D playerBody;

    private Ring[] rings;
    private bool completed;

    private void OnEnable()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(Restart);
    }

    private void Start()
    {
        victoryPanel.SetActive(false);
        rings = FindObjectsByType<Ring>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (completed || rings.Length == 0)
            return;

        foreach (Ring ring in rings)
        {
            if (ring != null)
                return;
        }

        completed = true;
        playerMovement.enabled = false;
        playerBody.linearVelocity = Vector2.zero;
        playerBody.angularVelocity = 0f;
        playerBody.simulated = false;
        victoryPanel.SetActive(true);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}