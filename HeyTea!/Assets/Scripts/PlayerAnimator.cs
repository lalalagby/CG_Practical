using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : Player.cs
Function  : convert the animation state machine.
Author    : Yong Wu
Data      : 27.08.2023

*/

public class PlayerAnimator : MonoBehaviour
{
    //use IsWalking to change the state. 
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake() {
        //Obtain the animation state machine of the player
        animator = GetComponent<Animator>();
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

    //Perform animation demonstrations by obtaining the iswalking state.
    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
