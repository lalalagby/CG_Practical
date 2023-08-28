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
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_Performed;
    }

    //publisher and subscriber to listen the interaction type action.
    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMoveVector() {
        //input vector,x=x axis move,y=y axis move
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        //move normalize to 1
        inputVector = inputVector.normalized;

        return inputVector;
    }

}
