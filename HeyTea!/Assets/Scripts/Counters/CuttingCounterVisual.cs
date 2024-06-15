using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Handles the cutting counter interaction animations.
 * @details This class manages the visual animations for the cutting counter interactions,
 * including triggering the cutting animation when a cut event occurs.
 * 
 * @author Yong Wu
 * @date 01.09.2023
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
