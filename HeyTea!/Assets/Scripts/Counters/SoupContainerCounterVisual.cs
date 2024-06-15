using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Handles the animation for the soup container counter interactions.
 * @details This class manages the visual animations for the soup container counter interactions,
 * including triggering the open/close animation when a player grabs an object.
 * 
 * @author Yong Wu
 * @date 01.09.2023
 */
public class SoupContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private SoupContainerCounter soupContainerCounter;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        //Subscribe to events where users retrieve items from cabinets
        soupContainerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) {
        //start animation
        animator.SetTrigger(OPEN_CLOSE);
    }
}
