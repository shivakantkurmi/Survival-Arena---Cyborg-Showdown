using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUi : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI promptText;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization if needed
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
