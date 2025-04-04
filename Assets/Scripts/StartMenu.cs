using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject instructionMenuUI; // Drag your Instruction Menu here
    public GameObject startMenuUI; // Drag your Start Menu UI here

    [Header("Audio")]
    public AudioClip startSound; // Drag your start sound clip here
    private AudioSource audioSource;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float startSoundVolume = 1f; // Volume for the start sound

    private bool isGameStarted = false; // Ensure no game logic runs prematurely

    public GameManager gameManager; // Reference to the GameManager (assigned in the Inspector)

    void Start()
    {
        // Set up the AudioSource and start playing the sound
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && startSound != null)
        {
            audioSource.clip = startSound;
            audioSource.loop = true;  // Loop the sound
            audioSource.volume = startSoundVolume; // Set volume for start sound
            audioSource.Play();  // Play the sound
        }
        else
        {
            Debug.LogWarning("AudioSource or start sound is not assigned!");
        }

        // Pause the game and show the Start Menu
        Time.timeScale = 0; // Pause everything
        isGameStarted = false;

        if (startMenuUI != null)
        {
            Debug.Log("Start");
            startMenuUI.SetActive(true); // Ensure Start Menu is visible at the beginning
        }
        else
        {
            Debug.LogWarning("Start Menu UI is not assigned in the Inspector!");
        }

        if (instructionMenuUI != null)
        {
            instructionMenuUI.SetActive(false); // Ensure Instruction Menu is hidden initially
        }
        else
        {
            Debug.LogWarning("Instruction Menu UI is not assigned in the Inspector!");
        }

        // Disable GameManager's logic components, but leave BGM running
        if (gameManager != null)
        {
            // Disable all game logic, keeping the AudioSource running
            gameManager.DisableGameLogic();
        }
    }

    public void OnStartButton()
    {
        if (startMenuUI != null)
        {
            startMenuUI.SetActive(false); // Hide the Start Menu
        }

        if (instructionMenuUI != null)
        {
            instructionMenuUI.SetActive(true); // Show the Instruction Menu
            Debug.Log("Instruction Menu is now active.");
        }
        else
        {
            Debug.LogError("Instruction Menu UI is missing!");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload current scene
        Time.timeScale = 1;  // Resume the game time
    }

    public void OnExitButton()
    {
        Debug.Log("Exit Game");
        Application.Quit(); // Exits the game

        // Note: Exit functionality works only in a built version of the game, not in the Unity editor.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#endif
    }

    void Update()
    {
        // Wait for Spacebar in the Instruction Menu to start the game
        if (!isGameStarted && instructionMenuUI != null && instructionMenuUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        if (instructionMenuUI != null)
        {
            instructionMenuUI.SetActive(false); // Hide the Instruction Menu
        }

        isGameStarted = true;
        Time.timeScale = 1; // Resume the game
        Debug.Log("Game Started!");

        // Stop the start sound when the game begins
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Activate the GameManager and enable the game logic
        if (gameManager != null)
        {
            gameManager.EnableGameLogic(); // Enable the game logic
        }
    }
}
