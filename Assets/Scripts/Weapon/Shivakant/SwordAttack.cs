// using UnityEngine;
// using UnityEngine.UI;
// using TMPro; // For TextMeshProUGUI

// public class SwordAttack : MonoBehaviour
// {
//     [Header("Sword Settings")]
//     public float damage = 25f;              // Damage dealt per hit
//     public float attackRange = 1.5f;        // Range of the sword attack
//     public float attackCooldown = 1f;       // Time between consecutive attacks
//     public LayerMask hitLayers;             // Layers that can be hit (e.g., Enemy)

//     [Header("Effects")]
//     public ParticleSystem slashEffect;      // Visual effect for the sword slash
//     public AudioClip slashSound;            // Sound effect for the sword slash
//     private AudioSource audioSource;        // Reference to AudioSource component

//     [Header("Crosshair Settings")]
//     public Image crosshair;                 // Crosshair image in UI
//     public Color defaultCrosshairColor = Color.white; // Default crosshair color
//     public Color enemyCrosshairColor = Color.red;     // Crosshair color when aiming at an enemy

//     [Header("UI Settings")]
//     public TextMeshProUGUI scoreText;       // Text to display score
//     public TextMeshProUGUI enemyCountText;  // Text to display live enemy count

//     [Header("References")]
//     public Camera playerCamera;             // Reference to the player's camera for raycast
//     public Animator swordAnimator;  
//     public GameManager gameManager;        // Animator for sword animations

//     private float nextAttackTime = 0f;      // Time until the next attack is allowed
//     // private int score = 0;                  // Player's score
//     private int initialEnemyCount;          // Total number of enemies at the start

//     void Start()
//     {
//         // Initialize audio source
//         audioSource = GetComponent<AudioSource>();
//         // score = ScoreManager.score; // Sync with global score
//         // UpdateScoreText();

//         // Ensure crosshair is set to the default color at the start
//         if (crosshair != null)
//         {
//             crosshair.color = defaultCrosshairColor;
//         }

//         // Count enemies at the start
//         initialEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
//         UpdateEnemyCount(initialEnemyCount); // Update enemy count UI
//         // UpdateScoreText();                   // Initialize score UI
//     }

//     void Update()
//     {
//         // Perform sword attack on left mouse click
//         if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
//         {
//             nextAttackTime = Time.time + attackCooldown;
//             PerformAttack();
//         }

//         // Update crosshair based on what the player is aiming at
//         RaycastHit hit;
//         if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange, hitLayers))
//         {
//             if (hit.transform.CompareTag("Enemy"))
//             {
//                 crosshair.color = enemyCrosshairColor; // Change crosshair to red when aiming at enemy
//             }
//             else
//             {
//                 crosshair.color = defaultCrosshairColor; // Default crosshair color
//             }
//         }
//         else
//         {
//             crosshair.color = defaultCrosshairColor; // Reset if not aiming at anything
//         }
//     }

//     void PerformAttack()
//     {
//         // Play sword attack animation
//         if (swordAnimator != null)
//         {
//             swordAnimator.SetTrigger("Attack");
//         }

//         // Play sword slash effect
//         if (slashEffect != null)
//         {
//             slashEffect.Play();
//         }

//         // Play slash sound effect
//         if (audioSource != null && slashSound != null)
//         {
//             audioSource.PlayOneShot(slashSound);
//         }

//         // Detect hits within attack range
//         RaycastHit hit;
//         if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange, hitLayers))
//         {
//             Debug.Log($"Hit: {hit.transform.name}");

//             // Check if the hit object is an enemy
//             EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
//             if (target != null)
//             {
//                 target.TakeDamage(damage);

//                 // Check if the enemy is dead
//                 if (target.health <= 0)
//                 {
//                     // ScoreManager.AddScore(10); // Add score globally
//                     // score = ScoreManager.score; // Sync local score              // Increase score
//                     // UpdateScoreText();         // Update score UI
//                     UpdateEnemyCount(--initialEnemyCount); // Update enemy count UI
//                 }
//             }
//         }
//         else
//         {
//             Debug.Log("Sword attack missed.");
//         }
//     }

//     // void UpdateScoreText()
//     // {
//     //     scoreText.text = $"Score: {ScoreManager.score}";
//     // }

//     public void UpdateEnemyCount(int currentEnemyCount)
//     {
//         enemyCountText.text = $"Enemies: {currentEnemyCount}";
//         if(currentEnemyCount == 0){
//                  gameManager.TriggerVictory();
//         }
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordAttack : MonoBehaviour
{
    [Header("Sword Settings")]
    public float damage = 25f;              // Damage dealt per hit
    public float attackRange = 1.5f;        // Range of the sword attack
    public float attackCooldown = 1f;       // Time between consecutive attacks
    public LayerMask hitLayers;             // Layers that can be hit (e.g., Enemy)

    [Header("Effects")]
    public ParticleSystem slashEffect;      // Visual effect for the sword slash
    public AudioClip slashSound;            // Sound effect for the sword slash
    private AudioSource audioSource;        // Reference to AudioSource component

    [Header("Crosshair Settings")]
    public Image crosshair;                 // Crosshair image in UI
    public Color defaultCrosshairColor = Color.white; // Default crosshair color
    public Color enemyCrosshairColor = Color.red;     // Crosshair color when aiming at an enemy

    [Header("References")]
    public Camera playerCamera;             // Reference to the player's camera for raycast
    public Animator swordAnimator;  
    public GameManager gameManager;        // Animator for sword animations
    public EnemyManager enemyManager;      // Reference to the EnemyManager

    private float nextAttackTime = 0f;      // Time until the next attack is allowed

    void Start()
    {
        // Initialize audio source
        audioSource = GetComponent<AudioSource>();

        // Ensure crosshair is set to the default color at the start
        if (crosshair != null)
        {
            crosshair.color = defaultCrosshairColor;
        }
    }

    void Update()
    {
        // Perform sword attack on left mouse click
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            PerformAttack();
        }

        // Update crosshair based on what the player is aiming at
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange, hitLayers))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                crosshair.color = enemyCrosshairColor; // Change crosshair to red when aiming at enemy
            }
            else
            {
                crosshair.color = defaultCrosshairColor; // Default crosshair color
            }
        }
        else
        {
            crosshair.color = defaultCrosshairColor; // Reset if not aiming at anything
        }
    }

    void PerformAttack()
    {
        // Play sword attack animation
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger("Attack");
        }

        // Play sword slash effect
        if (slashEffect != null)
        {
            slashEffect.Play();
        }

        // Play slash sound effect
        if (audioSource != null && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }

        // Detect hits within attack range
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange, hitLayers))
        {
            Debug.Log($"Hit: {hit.transform.name}");

            // Check if the hit object is an enemy
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Apply damage to enemy
                enemyHealth.TakeDamage(damage);

                // Check if the enemy is dead
                if (enemyHealth.health <= 0)
                {
                    // Update enemy count through EnemyManager
                    enemyManager.EnemyKilled();
                }
            }
        }
    }
}
