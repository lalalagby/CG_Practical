using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : SelectedCounter.cs
Function  : Counter selected visualization
Author    : Yong Wu
Data      : 28.08.2023

*/

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;

    //Add a subscriber to the player selected counter event
    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    //Change the appearance of the locker when a message is received
    private void Player_OnSelectedCounterChanged(object render,Player.OnSelectedCounterChangedEventsArgs e) {
        if (e.selectedCounter == clearCounter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        visualGameObject.SetActive(true);
    }
    private void Hide() {
        visualGameObject.SetActive(false);
    }
}
