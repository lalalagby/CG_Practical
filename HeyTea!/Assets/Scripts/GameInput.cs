using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : GameInput.cs
Function  : use newer input managet to get move
Author    : Yong Wu
Data      : 27.08.2023

*/
public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractCAction;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        //Subscribe to user input events
        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.InteractC.performed += InteractC_Performed;
    }

    //publisher and subscriber to listen the interaction type action.
    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    //cutting action
    private void InteractC_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        OnInteractCAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMoveVector() {
        //input vector,x=x axis move,y=y axis move
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        //move normalize to 1
        inputVector = inputVector.normalized;

        return inputVector;
    }

}
