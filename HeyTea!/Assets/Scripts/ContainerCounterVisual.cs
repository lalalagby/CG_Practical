using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : ContainerCounter.cs
Function  : Container interaction animation class
Author    : Yong Wu
Data      : 28.08.2023

*/

public class CountainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        //Subscribe to events where users retrieve items from cabinets
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) {
        //start animation
        animator.SetTrigger(OPEN_CLOSE);
    }
}
