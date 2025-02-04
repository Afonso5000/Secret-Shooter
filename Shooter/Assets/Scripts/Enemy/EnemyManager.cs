using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Text enemyCounterText; // UI Text to display the enemy count
    public GameObject victoryPanel; // UI panel for victory screen
    public static int enemiesRemaining = 0; // Static counter for enemies

    private void Start()
    {
        // Set enemy count at the beginning
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateEnemyCount();

        // Ensure the victory panel is disabled at start
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void EnemyDestroyed()
    {
        enemiesRemaining--; // Reduce count when an enemy dies
        UpdateEnemyCount();

        // Check if all enemies are defeated
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

        // Show the victory panel if assigned
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
}
