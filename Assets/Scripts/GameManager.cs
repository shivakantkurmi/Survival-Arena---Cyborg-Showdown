using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject victoryScreen;  // Reference to Victory Screen UI
    public GameObject defeatScreen;   // Reference to Defeat Screen UI
    public AudioClip victorySound;    // Reference to Victory sound effect
    public AudioClip defeatSound;     // Reference to Defeat sound effect
    public CanvasGroup victoryCanvasGroup; // CanvasGroup for Victory screen (used for fading)
    public CanvasGroup defeatCanvasGroup;  // CanvasGroup for Defeat screen (used for fading)
    public float fadeDuration = 2f;  // Time to fade the screens in/out
    public GameObject blurEffect;    // Optional: Reference to blur effect (Post-Processing or custom blur effect)

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float victorySoundVolume = 1f;  // Volume for victory sound
    [Range(0f, 1f)]
    public float defeatSoundVolume = 1f;   // Volume for defeat sound

    private bool gameEnded = false;

    // Start method: Removed BGM related logic
    void Start()
    {
        // No need to play or adjust BGM as it's removed
    }

    // Method to trigger the Victory Screen
    public void TriggerVictory()
    {
        if (!gameEnded)
        {
            gameEnded = true;

            // Play the victory sound with the specified volume
            if (victorySound != null)
            {
                AudioSource.PlayClipAtPoint(victorySound, Camera.main.transform.position, victorySoundVolume);
            }

            // Trigger the fade-in effect for the victory screen
            StartCoroutine(FadeInScreen(victoryCanvasGroup, victoryScreen, true));

            Debug.Log("Victory triggered!");
        }
    }

    // Method to trigger the Defeat Screen
    public void TriggerDefeat()
    {
        if (!gameEnded)
        {
            gameEnded = true;

            // Play the defeat sound with the specified volume
            if (defeatSound != null)
            {
                AudioSource.PlayClipAtPoint(defeatSound, Camera.main.transform.position, defeatSoundVolume);
            }

            // Trigger the fade-in effect for the defeat screen
            StartCoroutine(FadeInScreen(defeatCanvasGroup, defeatScreen, false));

            Debug.Log("Defeat triggered!");
        }
    }

    // Coroutine to handle the fading effect and show the screen
    private IEnumerator FadeInScreen(CanvasGroup screenCanvasGroup, GameObject screen, bool isVictory)
    {
        // Show the blur effect for the transition (optional)
        if (blurEffect != null)
        {
            blurEffect.SetActive(true); // Enable blur effect
        }

        // Gradually fade in the screen
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            screenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the screen is fully visible
        screenCanvasGroup.alpha = 1f;

        // Show the appropriate screen
        screen.SetActive(true);

        // Disable blur effect once transition is complete
        if (blurEffect != null)
        {
            blurEffect.SetActive(false); // Disable blur effect
        }

        // Prevent interaction with the game when victory/defeat is active
        Time.timeScale = 0;  // Pause the game time when victory/defeat screen shows
    }

    // Method to disable game logic
    public void DisableGameLogic()
    {
        this.enabled = false;
    }

    // Method to enable game logic
    public void EnableGameLogic()
    {
        this.enabled = true;
    }
}
