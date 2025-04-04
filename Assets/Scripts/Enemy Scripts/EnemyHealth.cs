// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.UI;

// public class EnemyHealth : MonoBehaviour
// {
//     public float maxHealth = 50f; // Maximum health of the enemy
//     public float health;         // Current health of the enemy
//     public GameObject healthBarPrefab; // Prefab for the health bar
//     private Slider healthBar;    // Reference to the health bar slider
//     private Transform healthBarTransform;
//     public AudioClip Scream;
//     private AudioSource audioSource;
//     private int score ;
//     public TextMeshProUGUI scoreText;

//     void Start()
//     {
//         score =ScoreManager.score;
//         // Set initial health
//         health = maxHealth;

//         // Instantiate the health bar prefab
//         GameObject healthBarInstance = Instantiate(healthBarPrefab);

//         // Set the health bar as a child of the enemy (it will move with the enemy)
//         healthBarInstance.transform.SetParent(transform, false);

//         // Adjust position above the enemy's head
//         healthBarInstance.transform.localPosition = new Vector3(0, 2, 0); // Adjust Y as needed

//         // Get the Slider component
//         healthBar = healthBarInstance.GetComponentInChildren<Slider>();

//         // Update the health bar initially
//         UpdateHealthBar();

//         // Store a reference to the health bar's transform for positioning
//         healthBarTransform = healthBarInstance.transform;
//         audioSource = GetComponent<AudioSource>();
//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//         }
//     }

//     void UpdateHealthBar()
//     {
//         if (healthBar != null)
//         {
//             // Update the slider value
//             healthBar.value = health / maxHealth;
//         }
//     }

// public void TakeDamage(float damage)
// {
//     // Reduce health
//     if (Scream != null)
//     {
//         audioSource.PlayOneShot(Scream);
//     }
//     health -= damage;
//     health = Mathf.Clamp(health, 0, maxHealth);

//     // Update the health bar
//     UpdateHealthBar();

//     // Check if the enemy is dead and call the Die() method from CybeMonsterAttack
//     if (health <= 0)
//     {
//         // Remove the "Enemy" tag and the enemy layer to prevent it from being counted
//         RemoveEnemyTagAndLayer();

//         // Call Die method from CybeMonsterAttack
//         CybeMonsterAttack monsterAttack = GetComponent<CybeMonsterAttack>();
//         if (monsterAttack != null)
//         {
//             monsterAttack.Die(); // Call the Die method from the attack script
//         }
//         else {
//             ScoreManager.AddScore(10);
//             score=ScoreManager.score;
//             UpdateScoreText();
            
//             Die(); // Call the Die method from the attack script
//         }
//     }
// }

// void UpdateScoreText()
// {
//         scoreText.text = $"Score: {ScoreManager.score}";
// }

// void RemoveEnemyTagAndLayer()
// {
//     // Remove the "Enemy" tag
//     gameObject.tag = "Untagged";  // Remove "Enemy" tag, you can also set this to another tag if needed

//     // Remove the enemy layer
//     gameObject.layer = LayerMask.NameToLayer("Default"); // Change to "Default" or any other appropriate layer
// }


//     void Die()
//     {
//         // Destroy the health bar
//         if (healthBarTransform != null)
//         {
//             Destroy(healthBarTransform.gameObject);
//         }

//         // Destroy the enemy
//         Destroy(gameObject);
//     }
// }



using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f; // Maximum health of the enemy
    public float health;         // Current health of the enemy
    public GameObject healthBarPrefab; // Prefab for the health bar
    private Slider healthBar;    // Reference to the health bar slider
    private Transform healthBarTransform;
    public AudioClip Scream;
    private AudioSource audioSource;
    private int score;
    public TextMeshProUGUI scoreText;

    [Header("Damage Animation")]
    public string damageAnimationTrigger = "IsDamage"; // Animation trigger name
    public float damageAnimationDuration = 0.5f; // Duration for damage animation to play
    
    public ScoreManager ScoreManager ;
    private Animator animator;

    void Start()
    {
        score = ScoreManager.score;
        UpdateScoreText();
        // Set initial health
        health = maxHealth;

        // Instantiate the health bar prefab
        GameObject healthBarInstance = Instantiate(healthBarPrefab);

        // Set the health bar as a child of the enemy (it will move with the enemy)
        healthBarInstance.transform.SetParent(transform, false);

        // Adjust position above the enemy's head
        healthBarInstance.transform.localPosition = new Vector3(0, 2, 0); // Adjust Y as needed

        // Get the Slider component
        healthBar = healthBarInstance.GetComponentInChildren<Slider>();

        // Update the health bar initially
        UpdateHealthBar();

        // Store a reference to the health bar's transform for positioning
        healthBarTransform = healthBarInstance.transform;

        // Get the AudioSource component or add one if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Update the slider value
            healthBar.value = health / maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce health
        if (animator != null)
        {
            animator.SetTrigger(damageAnimationTrigger); // Trigger the damage animation
        }
        if (Scream != null)
        {
            audioSource.PlayOneShot(Scream);
        }

        // Trigger the damage animation if there's an animator

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Update the health bar
        UpdateHealthBar();

        // Check if the enemy is dead and call the Die() method from CybeMonsterAttack
        if (health <= 0)
        {
            // Remove the "Enemy" tag and the enemy layer to prevent it from being counted
            RemoveEnemyTagAndLayer();

            // Call Die method from CybeMonsterAttack
            CybeMonsterAttack monsterAttack = GetComponent<CybeMonsterAttack>();
            if (monsterAttack != null)
            {
                monsterAttack.Die(); // Call the Die method from the attack script
            }
            else
            {
                Debug.Log("Health: " + health);
                ScoreManager.AddScore(10);
                score = ScoreManager.score;
                UpdateScoreText();

                Die(); // Call the Die method from the attack script
            }
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Score: {ScoreManager.score}";
    }

    void RemoveEnemyTagAndLayer()
    {
        // Remove the "Enemy" tag
        gameObject.tag = "Untagged";  // Remove "Enemy" tag, you can also set this to another tag if needed

        // Remove the enemy layer
        gameObject.layer = LayerMask.NameToLayer("Default"); // Change to "Default" or any other appropriate layer
    }

    void Die()
    {
        // Destroy the health bar
        if (healthBarTransform != null)
        {
            Destroy(healthBarTransform.gameObject);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
}
