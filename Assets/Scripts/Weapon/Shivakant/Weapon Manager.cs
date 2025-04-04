using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject currentWeapon;    // The weapon currently equipped
    public GameObject gun;              // Reference to the gun weapon
    public GameObject sword;            // Reference to the sword weapon
    private bool hasGun = false;        // Whether the player has the gun
    private bool hasSword = false;      // Whether the player has the sword

    void Update()
    {
        // Switch weapons when the user presses the 'Q' key (or any other key you prefer)
        if (Input.GetKeyDown(KeyCode.Q) && hasGun && hasSword)
        {
            SwitchWeapon();
        }
    }

    // Call this method to collect and equip a weapon
    public void CollectWeapon(GameObject weapon)
    {
        if (weapon.name.Contains("Gun"))
        {
            hasGun = true;
            gun = weapon;  // Assign the gun reference
        }
        else if (weapon.name.Contains("Sword"))
        {
            hasSword = true;
            sword = weapon;  // Assign the sword reference
        }

        EquipWeapon(weapon); // Equip the collected weapon
    }

    // Equip the specified weapon
    private void EquipWeapon(GameObject weapon)
    {
        // Disable the current weapon (if any)
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        // Enable the new weapon
        currentWeapon = weapon;
        currentWeapon.SetActive(true);
    }

    // Switch between the two weapons
    private void SwitchWeapon()
    {
        if (currentWeapon == gun)
        {
            EquipWeapon(sword); // Equip the sword if the gun is currently equipped
        }
        else if (currentWeapon == sword)
        {
            EquipWeapon(gun); // Equip the gun if the sword is currently equipped
        }
    }
}
