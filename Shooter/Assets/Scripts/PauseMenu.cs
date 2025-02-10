using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private PlayerMovement gameScript; 
    private PlayerShooting gunScript; 

    void Start()
    {
        pauseMenuUI.SetActive(false);
        LockCursor();

        // Find the scripts automatically
        gameScript = FindObjectOfType<PlayerMovement>(); 
        gunScript = FindObjectOfType<PlayerShooting>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        LockCursor();
        isPaused = false;

        // Enable game script and shooting
        if (gameScript) gameScript.enabled = true;
        if (gunScript) gunScript.enabled = true;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        UnlockCursor();
        isPaused = true;

        // Disable game script and shooting
        if (gameScript) gameScript.enabled = false;
        if (gunScript) gunScript.enabled = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
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
