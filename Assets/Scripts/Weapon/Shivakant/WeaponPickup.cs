using UnityEngine;

public class WeaponPickup : Interactable
{
    public GameObject weaponToEnable;    // Reference to the weapon in player's hierarchy
    public GameObject weaponReplica;     // Reference to the weapon replica in the scene
    private WeaponManager weaponManager; // Reference to WeaponManager (handles switching)

    void Start()
    {
        // Get reference to the WeaponManager
        // weaponManager = FindObjectOfType<WeaponManager>();
        weaponManager=FindAnyObjectByType<WeaponManager>();
    }

    protected override void Interact()
    {
        // Disable the replica in the scene
        weaponReplica.SetActive(false);

        // Enable the weapon in the player's hand
        if (weaponManager != null)
        {
            weaponManager.CollectWeapon(weaponToEnable);
        }

        base.Interact();
    }
}
