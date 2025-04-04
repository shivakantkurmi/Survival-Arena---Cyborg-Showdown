// using UnityEngine;

// public class CursorVisibilityHandler : MonoBehaviour
// {
//     void Update()
//     {
//         // Check if the cursor is inside the game window
//         Vector3 mousePosition = Input.mousePosition;
//         bool isCursorInGameWindow = mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
//                                     mousePosition.y >= 0 && mousePosition.y <= Screen.height;

//         // Set cursor visibility
//         if (Time.timeScale==1 &&  isCursorInGameWindow)
//         {
//             Cursor.visible = false; // Hide the cursor when it's inside the game window
//         }
//         else
//         {
//             Cursor.visible = true; // Show the cursor when it's outside the game window
//         }
//     }
// }


using UnityEngine;
using UnityEngine.EventSystems;

public class CursorVisibilityHandler : MonoBehaviour
{
    void Update()
    {
        // Check if the cursor is inside the game window
        Vector3 mousePosition = Input.mousePosition;
        bool isCursorInGameWindow = mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
                                    mousePosition.y >= 0 && mousePosition.y <= Screen.height;

        // Check if the cursor is over a UI element
        bool isCursorOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        // Set cursor visibility based on game state, window position, and UI interactions
        if (Time.timeScale == 1 && isCursorInGameWindow && !isCursorOverUI)
        {
            Cursor.visible = false; // Hide the cursor when in the game window and not over UI
        }
        else
        {
            Cursor.visible = true; // Show the cursor otherwise
        }
    }
}
