using UnityEngine;

public class KnockbackEffect : MonoBehaviour
{
    public void ApplyKnockback(Rigidbody rb, Vector3 hitDirection, float force)
    {
        if (rb != null)
        {
            rb.AddForce(hitDirection * force, ForceMode.Impulse);
        }
    }
}
