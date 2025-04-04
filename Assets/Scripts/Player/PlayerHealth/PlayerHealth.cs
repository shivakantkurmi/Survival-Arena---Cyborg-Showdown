// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class PlayerHealth : MonoBehaviour
// {
//     private float health;
//     private float lerpTimer;
//     [Header("Health Bar")]
//     public float maxHealth = 100f;
//     public float chipSpeed = 1f;
//     public Image frontHealthBar;
//     public Image backHealthBar;
//     public TextMeshProUGUI healthText;
//     [Header("Damage Overlay")]
//     public Image overlay;
//     public float duration=2f;
//     public float fadeSpeed=1.5f;
//     private float durationTimer;
//     // Start is called before the first frame update
//     void Start()
//     {
//         health = maxHealth;
//         overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         health = Mathf.Clamp(health, 0.0f, maxHealth);
//         UpdateHealthUI();
//         if(overlay.color.a>0){
//             if(health<20) return;
//             durationTimer += Time.deltaTime;
//             if(durationTimer>duration){
//                 float tempAlpha = overlay.color.a;
//                 tempAlpha-=Time.deltaTime*fadeSpeed;
//                 overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
//             }
//         }
//     }
//     public void UpdateHealthUI()
//     {
//         // Debug.Log(health);
//         float fillF = frontHealthBar.fillAmount;
//         float fillB = backHealthBar.fillAmount;
//         float hFraction = health / maxHealth;
//         if (fillB > hFraction)
//         {
//             frontHealthBar.fillAmount = hFraction;
//             backHealthBar.color = Color.red;
//             lerpTimer += Time.deltaTime;
//             float percentComplete = lerpTimer / chipSpeed;
//             percentComplete=percentComplete*percentComplete;
//             backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
//         }
//         if (fillF < hFraction)
//         {
//             backHealthBar.fillAmount = hFraction;
//             backHealthBar.color = Color.green;
//             lerpTimer += Time.deltaTime;
//             float percentComplete = lerpTimer / chipSpeed;
//             percentComplete=percentComplete*percentComplete;
//             frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
//         }
//         healthText.text = health + "%" ;
//     }

//     public void TakeDamage(float damage)
//     {
//         health -= damage;
//         lerpTimer = 0f;
//         durationTimer = 0f;
//         overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
//         // StartCoroutine(LerpHealthBar());
//     }

//     public void RestoreHealth(float healAmount){
//         health += healAmount;
//         lerpTimer = 0f;
//     }

// }


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    
    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 1f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;
    
    [Header("Damage Overlay")]
    public Image overlay;
    public float duration = 2f;
    public float fadeSpeed = 1.5f;
    private float durationTimer;
    public GameManager gameManager;

    [Header("Player Interactions")]
    public LayerMask interactableLayer; // The layer for both enemies and interactables
    public float interactionRange = 2f; // Range at which player can interact with objects

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        UpdateHealthUI();

        if (overlay.color.a > 0)
        {
            if (health < 10) return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

        // Check for interactions with enemies and interactables within the interaction range
        DetectInteractions();
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
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }

        healthText.text = health + "%";
    }

    public void TakeDamage(float damage)
{
    health -= damage;
    lerpTimer = 0f;
    durationTimer = 0f;
    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.7f);
    
    // Ensure health doesn't go below zero
    if (health <= 0)
    {
        health = 0;  // Set health to exactly zero
        if (gameManager != null)
        {
            gameManager.TriggerDefeat();  // Trigger defeat when health is zero
        }
    }
}


    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    // Detect interactions with enemies and interactables
    void DetectInteractions()
{
    Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
    
    foreach (var collider in colliders)
    {
        // Check if the object is an enemy (based on its tag)
        if (collider.CompareTag("Enemy"))
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                // Check if the enemy is within the attack range
                float distanceToPlayer = Vector3.Distance(transform.position, enemy.player.position);
                if (enemy.isChasing && !enemy.agent.isStopped && distanceToPlayer <= enemy.attackRadius)
                {
                    // Take damage from the enemy if within the attack range
                    TakeDamage(enemy.attackDamage);  
                    Debug.Log("Player takes damage from enemy!");
                }
            }
        }
    }
}


    // Optionally, visualize interaction range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
