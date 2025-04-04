using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;        // UI Text for displaying enemy count
    public GameManager gameManager;   // Reference to your GameManager

    private int currentEnemyCount;

    void Start()
    {
        // Initialize enemy count
        currentEnemyCount = GetActiveEnemyCount();
        UpdateEnemyCount(currentEnemyCount);

        // Debug: Check initialization
        if (enemyCountText == null || gameManager == null)
        {
            Debug.LogError("EnemyManager: Missing references! Assign 'enemyCountText' and 'gameManager' in the Inspector.");
        }
    }

    void Update()
    {
        // Dynamically update the enemy count every frame
        currentEnemyCount = GetActiveEnemyCount();
        UpdateEnemyCount(currentEnemyCount);
    }

    public int GetActiveEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy) count++;
        }

        return count;
    }

    public void EnemyKilled()
    {
        // Optionally, you could also call GetActiveEnemyCount here to update the count immediately after a kill
        currentEnemyCount = GetActiveEnemyCount();
        UpdateEnemyCount(currentEnemyCount);
    }

    public void UpdateEnemyCount(int count)
    {
        // Update the UI
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {count}";
        }

        // Check for victory condition
        if (count == 0 && gameManager != null)
        {
            gameManager.TriggerVictory();
        }
    }
}
