using UnityEngine;
using TMPro; // Import TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static int score = 0; // Static score to be accessed globally
    public TextMeshProUGUI scoreText; // Reference to the UI text for displaying score

    void Start()
    {
        // Initialize the score UI
        UpdateScoreText();
    }

    public static void AddScore(int points)
    {
        score += points;
        // Call UpdateScoreText from an instance of ScoreManager to reflect the change in UI
        // Using an instance because score is a static variable and needs to be updated UI-wise
        FindAnyObjectByType<ScoreManager>().UpdateScoreText();
    }

    // Method to update the score text on UI
    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}"; // Update the score display
        }
    }
}
