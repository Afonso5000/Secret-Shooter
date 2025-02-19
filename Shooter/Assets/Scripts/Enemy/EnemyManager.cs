using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance; // Singleton instance

    public Text enemyCounterText; // UI Text to display the enemy count
    public GameObject victoryPanel; // UI panel for victory screen
    public AudioSource victoryMusic; // 🎶 Assign this in Unity
    public int enemiesRemaining = 0; // Enemy count

    private PlayerMovement playerMovement; // Reference to PlayerMovement script
    private PlayerShooting playerShooting; // Reference to PlayerShooting script

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateEnemyCount();

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Find player scripts automatically
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerShooting = FindObjectOfType<PlayerShooting>();
    }

    public void EnemyDestroyed()
    {
        enemiesRemaining--;
        UpdateEnemyCount();

        if (enemiesRemaining <= 0)
        {
            TriggerVictory();
        }
    }

    private void UpdateEnemyCount()
    {
        enemyCounterText.text = "Enemies Left: " + enemiesRemaining;
    }

    private void TriggerVictory()
    {
        Debug.Log("Victory! All enemies are destroyed.");

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // 🎶 Play victory music
        if (victoryMusic != null)
        {
            victoryMusic.Play();
        }

        Time.timeScale = 0f; // 🔴 Pause the game

        // 🔒 Disable player movement and shooting
        if (playerMovement) playerMovement.enabled = false;
        if (playerShooting) playerShooting.enabled = false;

        // 🔓 Unlock the cursor for menu interaction
        UnlockCursor();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 🔵 Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 🔒 Lock cursor again when the game restarts
        LockCursor();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // 🔵 Resume time
        SceneManager.LoadScene(0);

        // 🔓 Unlock cursor when returning to the main menu
        UnlockCursor();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // 🔵 Resume time
        Application.Quit();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
