using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
