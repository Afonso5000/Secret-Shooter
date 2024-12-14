using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;       // Current health
    private float lerpTimer;    // Timer for health bar animation

    [Header("Health Bar")]
    public float maxHealth = 100f;        // Maximum health value
    public float chipSpeed = 2f;          // Speed for health bar back fill
    public Image frontHealthBar;          // Foreground health bar
    public Image backHealthBar;           // Background health bar

    [Header("Damage Overlay")]
    public Image overlay;                 // Damage overlay image
    public float duration = 1f;           // Duration overlay stays fully visible
    public float fadeSpeed = 1f;          // Speed at which overlay fades

    private float durationTimer = 0f;     // Timer for fading the overlay

    void Start()
    {
        health = maxHealth;               // Initialize health to max

        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0f);
    }

    void Update()
    {
        // Ensure health remains within bounds
        health = Mathf.Clamp(health, 0, maxHealth);

        // Update the health bar UI
        UpdateHealthUI();

        // Test damage on key press
        if (Input.GetKeyUp(KeyCode.T))
        {
            TakeDamage(Random.Range(5f, 10f));
        }

        // Handle damage overlay fade-out
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
        // Reduce health
        health -= damage;
        Debug.Log($"Player takes {damage} damage! Current health: {health}");

        // Reset damage overlay
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1f); // Set overlay alpha to 1
        durationTimer = 0f; // Reset the overlay fade timer

        // Reset lerp timer for smooth health bar animation
        lerpTimer = 0f;
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth; // Current health as fraction of max health

        if (fillB > hFraction)
        {
            // Health is decreasing
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            // Health is increasing (for healing, if implemented)
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }
}