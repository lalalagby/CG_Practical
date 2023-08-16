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

    //callback each update
    private void Update() {
        //input vector,x=x axis move,y=y axis move
        Vector2 inputVector = new Vector2(0, 0);

        //get move input
        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = +1;
        }

        //move normalize to 1
        inputVector = inputVector.normalized;

        //transfer to game coordinate
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //update player postion
        transform.position += moveDir* moveSpeed*Time.deltaTime;

        //update dirction
        float rotatedSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward , moveDir, rotatedSpeed*Time.deltaTime);

        Debug.Log(inputVector);

    }

}
