// using UnityEngine;

// public abstract class BaseWeapon : MonoBehaviour
// {
//     protected Camera playerCamera;  // Reference to the player's camera

//     public string weaponName;
//     public float damage;

//     // Method to handle hitting an enemy
//     public void HandleEnemyHit(GameObject enemy)
//     {
//         EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
//         if (enemyHealth != null)
//         {
//             enemyHealth.TakeDamage(damage);

//             // Check if the enemy died
//             if (enemyHealth.health <= 0)
//             {
//                 BaseWeaponHolder.Instance.AddScore(10); // Add score
//                 BaseWeaponHolder.Instance.DecreaseEnemyCount(); // Decrease enemy count
//             }
//         }
//     }

//     // Set the camera reference (called by the weapon holder)
//     public void SetCameraReference(Camera cam)
//     {
//         playerCamera = cam;
//     }

//     // Abstract method for specific weapon behavior
//     public abstract void UseWeapon();
// }
// // 