using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    private PlayerUi playerUi; 
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUi = GetComponent<PlayerUi>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUi.UpdateText(string.Empty); // Clear previous text

        // Create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // Variable to store our collision information

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            // Check if the ray hits an interactable object
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Update the UI text with the prompt message
                playerUi.UpdateText(interactable.promptMessage);

                // Check if the player presses the interact button
                if (inputManager.onFoot.Interact.triggered)
                {
                    // Call BaseInteract to interact with the object
                    interactable.BaseInteract();
                }
            }
        }
    }
}
