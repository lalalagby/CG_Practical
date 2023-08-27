using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : player.cs
Function  : user control
Author    : Yong Wu
Data      :16.08.2023

*/


public class Player : MonoBehaviour
{
    //define the player's move speed
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    //callback each update
    private void Update() {
        Vector2 inputVector = gameInput.GetMoveVector();

        //transfer to game coordinate
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //update player postion
        transform.position += moveDir* moveSpeed*Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        //update dirction
        float rotatedSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward , moveDir, rotatedSpeed*Time.deltaTime);

    }

    public bool IsWalking() {
        return isWalking;
    }

}
