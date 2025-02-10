using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration = 1f;
    public float fadeSpeed = 1f;

    private float durationTimer = 0f;

    [Header("Defeat Screen")]
    public GameObject defeatScreen;

    private PlayerMovement gameScript;
    private PlayerShooting gunScript;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0f);

        if (defeatScreen != null)
            defeatScreen.SetActive(false);

        // Find player scripts
        gameScript = GetComponent<PlayerMovement>();
        gunScript = GetComponent<PlayerShooting>();
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Clamp01(tempAlpha));
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Player takes {damage} damage! Current health: {health}");

        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1f);
        durationTimer = 0f;

        lerpTimer = 0f;

        if (health <= 0)
        {
            TriggerDefeatScreen();
        }
    }

    public void TriggerDefeatScreen()
    {
        Debug.Log("Player Defeated!");
        if (defeatScreen != null)
        {
            defeatScreen.SetActive(true);
            Time.timeScale = 0f; // Pause game

            // Disable player movement and shooting
            if (gameScript) gameScript.enabled = false;
            if (gunScript) gunScript.enabled = false;

            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Unpause game
        SceneManager.LoadScene(0); // Load main menu (Scene Index 0)
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }
}