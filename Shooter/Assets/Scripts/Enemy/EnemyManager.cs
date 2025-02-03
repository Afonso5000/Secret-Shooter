using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Text enemyCounterText; // UI Text to display the enemy count
    public GameObject victoryPanel; // UI panel for victory screen

    private int enemiesRemaining; // Number of enemies left

    void Start()
    {
        // Set enemy count only at the beginning
       enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
            Debug.Log("Enemies at start: " + enemiesRemaining); // Debug Log
            UpdateEnemyCounter();
        
        // Ensure the victory panel is disabled at start
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void EnemyDestroyed()
    {
        enemiesRemaining--; // Decrease the count when an enemy is destroyed
        UpdateEnemyCounter();

        // Check if the player wins
        if (enemiesRemaining <= 0)
        {
            TriggerVictory();
        }
    }

    private void UpdateEnemyCounter()
    {
        if (enemyCounterText != null)
        {
            enemyCounterText.text = "Enemies Left: " + enemiesRemaining;
        }
    }

    private void TriggerVictory()
    {
        Debug.Log("Victory! All enemies are destroyed.");
        
        // Show the victory panel if assigned
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
}