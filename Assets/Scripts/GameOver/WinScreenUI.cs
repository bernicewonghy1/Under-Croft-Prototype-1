
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private PlayerHealth playerHealth;

    private bool gameFinished = false;
    private bool playerDied = false;

    private void Start()
    {
        winPanel.SetActive(false);

        if (playerHealth != null)
            playerHealth.OnPlayerDeath += PlayerDied;
    }

    private void Update()
    {
        if (gameFinished || playerDied)
            return;

        // Check if any enemies are still alive
        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        if (enemies.Length == 0)
        {
            ShowWinScreen();
        }
    }

    private void PlayerDied()
    {
        playerDied = true;
    }

    private void ShowWinScreen()
    {
        gameFinished = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnPlayerDeath -= PlayerDied;
    }
}
