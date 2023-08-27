using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
