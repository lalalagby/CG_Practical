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
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public Vector2 GetMoveVector() {
        //input vector,x=x axis move,y=y axis move
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        //move normalize to 1
        inputVector = inputVector.normalized;

        return inputVector;
    }

}
