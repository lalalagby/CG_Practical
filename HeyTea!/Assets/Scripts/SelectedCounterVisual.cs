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
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    //Add a subscriber to the player selected counter event
    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    //Change the appearance of the locker when a message is received
    private void Player_OnSelectedCounterChanged(object render,Player.OnSelectedCounterChangedEventsArgs e) {
        if (e.selectedCounter == baseCounter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach(GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(true);
        }
    }
    private void Hide() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(false);
        }
    }
}
