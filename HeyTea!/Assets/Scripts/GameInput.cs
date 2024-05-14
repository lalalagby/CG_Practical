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
    public event EventHandler<OnOperationHoldActionEventArgs> OperationHoldAction;
    public class OnOperationHoldActionEventArgs : EventArgs {
        public float Time;
    }
    public event EventHandler OperationAction;

    private PlayerInputActions playerInputActions;

    private bool isHolding;
    private float sendInterval = 0.1f;
    private float cumulativeTime = 0f;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        //Subscribe to user input events
        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.Operation.performed += Operation_Performed;
        playerInputActions.Player.Operation.canceled += Operation_Canceled;
    }

    private void Update() {
        //if user is holding interact key,we will send subsribtion per interval;
        if (isHolding ) {
            if(cumulativeTime >= sendInterval) {
                OperationHoldAction?.Invoke(this, new OnOperationHoldActionEventArgs { Time = sendInterval });
                cumulativeTime = 0f;
            } else {
                cumulativeTime += Time.deltaTime;
            }
            
        } 
    }

    //publisher and subscriber to listen the interaction type action.
    public void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    //cutting action
    public void Operation_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        if (obj.interaction is HoldInteraction) {
            isHolding = true;
        } 
        OperationAction?.Invoke(this, EventArgs.Empty);
        
    }

    public void Operation_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //if the suscriber number is not zero, then we can board this message.
        if (obj.interaction is HoldInteraction) {
            isHolding = false;
        } else {

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
