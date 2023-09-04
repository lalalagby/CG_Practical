using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

/*
File Name : GameInput.cs
Function  : use newer input managet to get move
Author    : Yong Wu
Data      : 27.08.2023

*/
public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler<OnInteractHoldActionEventArgs> OnInteractHoldAction;
    public class OnInteractHoldActionEventArgs : EventArgs {
        public float Time;
    }
    private PlayerInputActions playerInputActions;

    private bool isHolding;
    private float sendInterval = 0.1f;
    private float cumulativeTime = 0f;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        //Subscribe to user input events
        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.InteractC.performed += InteractHold_Performed;
        playerInputActions.Player.InteractC.canceled += InteractHold_Canceled;
    }

    private void Update() {
        //if user is holding interact key,we will send subsribtion per interval;
        if (isHolding ) {
            if(cumulativeTime >= sendInterval) {
                OnInteractHoldAction?.Invoke(this, new OnInteractHoldActionEventArgs { Time = sendInterval });
                cumulativeTime = 0f;
            } else {
                cumulativeTime += Time.deltaTime;
            }
            
        } 
    }

    //publisher and subscriber to listen the interaction type action.
    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    //cutting action
    private void InteractHold_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        if (obj.interaction is HoldInteraction) {
            isHolding = true;
        }
    }

    private void InteractHold_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        if (obj.interaction is HoldInteraction) {
            isHolding = false;
        }
    }

    public Vector2 GetMoveVector() {
        //input vector,x=x axis move,y=y axis move
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        //move normalize to 1
        inputVector = inputVector.normalized;

        return inputVector;
    }

}
