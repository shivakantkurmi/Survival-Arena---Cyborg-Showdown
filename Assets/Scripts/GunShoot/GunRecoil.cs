using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Transform gunTransform; // Reference to the gun's transform
    public float recoilAmount = 0.1f; // Intensity of recoil
    public float recoilSpeed = 5f;    // Speed to reset the recoil

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = gunTransform.localPosition;
    }

    void Update()
    {
        // Smoothly reset gun position after recoil
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
    }

    public void ApplyRecoil()
    {
        gunTransform.localPosition -= Vector3.forward * recoilAmount;
    }
}
