// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class GunShoot : MonoBehaviour
// {
//     public float damage = 10f;                // Damage per shot
//     public float range = 70f;                 // Weapon range
//     public float fireRate = 0.5f;             // Rate of fire
//     public Camera fpsCam;                    // Camera for raycasting
//     public ParticleSystem muzzleFlash;       // Muzzle flash effect
//     public GameObject impactEffect;          // Generic impact effect prefab
//     public AudioClip gunshotSound;           // Gunshot audio
//     public LayerMask hitLayers;              // Layers the gun can hit
//     public Image crosshair;                  // Reference to the crosshair image
//     public Color defaultCrosshairColor = Color.white; // Default crosshair color
//     public Color enemyCrosshairColor = Color.red;     // Crosshair color when aiming at an enemy
//     public EnemyManager enemyManager;        // Reference to EnemyManager script
//     [Range(0f, 100f)] public float knockbackForce = 10f; // Knockback force with adjustable range
//     private KnockbackEffect knockbackEffect;
//     private float nextTimeToFire = 0f;       // Timer for controlling fire rate
//     private AudioSource audioSource;
//     public GameManager gameManager;          // Reference to GameManager

//     void Start()
//     {
//         audioSource = GetComponent<AudioSource>();
//         knockbackEffect = GetComponent<KnockbackEffect>();

//         if (crosshair != null)
//         {
//             crosshair.color = defaultCrosshairColor; // Set the initial crosshair color
//         }
//     }

//     void Update()
//     {
//         // Raycast to detect if aiming at an enemy
//         RaycastHit hit;
//         if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitLayers))
//         {
//             EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
//             crosshair.color = target != null ? enemyCrosshairColor : defaultCrosshairColor;
//         }
//         else
//         {
//             crosshair.color = defaultCrosshairColor;
//         }

//         // Shoot when left mouse button is pressed and fire rate allows it
//         if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
//         {
//             nextTimeToFire = Time.time + 1f / fireRate;
//             Shoot();
//         }
//     }

//     void Shoot()
//     {
//         // Play muzzle flash
//         if (muzzleFlash != null)
//         {
//             muzzleFlash.Play();
//         }

//         // Play gunshot sound
//         if (gunshotSound != null)
//         {
//             audioSource.PlayOneShot(gunshotSound);
//         }

//         // Perform raycast to detect hit
//         RaycastHit hit;
//         if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitLayers))
//         {
//             Debug.Log($"Hit: {hit.transform.name}");

//             // Check if the hit object is an enemy
//             EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
//             if (target != null)
//             {
//                 Debug.Log($"Enemy hit: {hit.transform.name}");

//                 // Apply damage to the enemy
//                 target.TakeDamage(damage);

//                 // Apply knockback to the enemy using the KnockbackEffect script
//                 Rigidbody enemyRigidbody = hit.transform.GetComponent<Rigidbody>();
//                 if (enemyRigidbody != null)
//                 {
//                     Vector3 knockbackDirection = (hit.transform.position - fpsCam.transform.position).normalized;
//                     knockbackEffect.ApplyKnockback(enemyRigidbody, knockbackDirection, knockbackForce);
//                 }

//                 // Check if the enemy is dead
//                 if (target.health <= 0)
//                 {
//                     // Update enemy count through EnemyManager
//                     enemyManager.UpdateEnemyCount(enemyManager.GetActiveEnemyCount());
//                 }
//             }

//             // Spawn impact effect (even if not an enemy, e.g., on walls)
//             SpawnImpactEffect(hit.point, hit.normal);
//         }
//     }

//     void SpawnImpactEffect(Vector3 position, Vector3 normal)
//     {
//         GameObject impactGO = Instantiate(impactEffect, position, Quaternion.LookRotation(normal));
//         Destroy(impactGO, 2f); // Destroy effect after 2 seconds
//     }
// }




using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunShoot : MonoBehaviour
{
    public float damage = 10f;                // Damage per shot
    public float range = 70f;                 // Weapon range
    public float fireRate = 0.5f;             // Rate of fire
    public Camera fpsCam;                    // Camera for raycasting
    public ParticleSystem muzzleFlash;       // Muzzle flash effect
    public GameObject impactEffect;          // Generic impact effect prefab
    public AudioClip gunshotSound;           // Gunshot audio
    public LayerMask hitLayers;              // Layers the gun can hit
    public Image crosshair;                  // Reference to the crosshair image
    public Color defaultCrosshairColor = Color.white; // Default crosshair color
    public Color enemyCrosshairColor = Color.red;     // Crosshair color when aiming at an enemy
    public EnemyManager enemyManager;        // Reference to EnemyManager script
    [Range(0f, 100f)] public float knockbackForce = 10f; // Knockback force with adjustable range
    private KnockbackEffect knockbackEffect;
    private float nextTimeToFire = 0f;       // Timer for controlling fire rate
    private AudioSource audioSource;
    public GameManager gameManager;          // Reference to GameManager

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        knockbackEffect = GetComponent<KnockbackEffect>();

        if (crosshair != null)
        {
            crosshair.color = defaultCrosshairColor; // Set the initial crosshair color
        }
    }

    public void IncreaseGunDamage(int power){
        damage += power;
    }
    public void IncreaseRange(int power){
        range += power;
    }

    void Update()
    {
        // Raycast to detect if aiming at an enemy
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitLayers))
        {
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            crosshair.color = target != null ? enemyCrosshairColor : defaultCrosshairColor;
        }
        else
        {
            crosshair.color = defaultCrosshairColor;
        }

        // Shoot when left mouse button is pressed and fire rate allows it
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Play muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play gunshot sound
        if (gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
        }

        // Perform raycast to detect hit
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitLayers))
        {
            Debug.Log($"Hit: {hit.transform.name}");

            // Check if the hit object is an enemy
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target != null)
            {
                Debug.Log($"Enemy hit: {hit.transform.name}");

                // Apply damage to the enemy
                target.TakeDamage(damage);

                // Apply knockback to the enemy using the KnockbackEffect script
                Rigidbody enemyRigidbody = hit.transform.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    Vector3 knockbackDirection = (hit.transform.position - fpsCam.transform.position).normalized;
                    knockbackEffect.ApplyKnockback(enemyRigidbody, knockbackDirection, knockbackForce);
                }

                // Check if the enemy is dead
                if (target.health <= 0)
                {
                    // Update enemy count through EnemyManager
                    enemyManager.EnemyKilled();
                }
            }

            // Spawn impact effect (even if not an enemy, e.g., on walls)
            SpawnImpactEffect(hit.point, hit.normal);
        }
    }

    void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        GameObject impactGO = Instantiate(impactEffect, position, Quaternion.LookRotation(normal));
        Destroy(impactGO, 2f); // Destroy effect after 2 seconds
    }
}
