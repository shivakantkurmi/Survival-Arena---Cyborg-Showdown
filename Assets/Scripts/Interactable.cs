using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;  // To add or remove interaction events
    public string promptMessage = "Press E to interact";  // Message shown when the player looks at the object

    // Virtual method that can be overridden by derived classes to define custom interaction
    public virtual string OnLook()
    {
        return promptMessage;
    }

    // Main interaction method which can be triggered by the player
    public void BaseInteract()
    {
        if (useEvents)
        {
            // If useEvents is true, invoke interaction events (e.g., collected sound, animation)
            InteractionEvent interactionEvent = GetComponent<InteractionEvent>();
            if (interactionEvent != null)
            {
                interactionEvent.onInteract.Invoke();
            }
        }
        Interact();  // Call the derived class-specific interaction method
    }

    // Virtual method for handling interaction logic, meant to be overridden by child classes
    protected virtual void Interact()
    {
        // No implementation here in the base class, child classes should override this
    }
}
