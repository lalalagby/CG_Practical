using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : CuttingCounterVisual.cs
Function  : CuttingCounter interaction animation class
Author    : Yong Wu
Data      : 01.09.2023

*/

public class CuttingCounterVisual : MonoBehaviour
{
    //define cutting animation state change parameter
    private const string CUT = "Cut";

    //The cabinet where the animation takes place
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        //Subscribe to events where users retrieve items from cabinets
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e) {
        //start animation
        animator.SetTrigger(CUT);
    }
}
