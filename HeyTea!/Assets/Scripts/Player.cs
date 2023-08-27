using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : Player.cs
Function  : user control
Author    : Yong Wu
Data      : 16.08.2023

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

        //collision detection
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position+Vector3.up*playerHeight, playerRadius, moveDir, moveDistance);

        //update player postion
        if (!canMove) {
            //first attempt x axis
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                } else {
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }
        

        isWalking = moveDir != Vector3.zero;

        //update dirction
        float rotatedSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward , moveDir, rotatedSpeed*Time.deltaTime);

    }

    public bool IsWalking() {
        return isWalking;
    }

}
