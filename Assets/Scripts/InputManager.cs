using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private PlayerMovement movement;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        movement=GetComponent<PlayerMovement>();

        look = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx => movement.Jump();

        onFoot.Crouch.performed+= ctx=> movement.Crouch();
        onFoot.Sprint.performed += ctx => { 
    movement.Sprint(); 
};



    }

    // Update is called once per frame
void FixedUpdate()
{
    movement.ProcessMovement(onFoot.Movement.ReadValue<Vector2>());
}


    private void LateUpdate(){
        //tells the player to look using the value from our look action
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable(){
        onFoot.Disable();
    }
}
