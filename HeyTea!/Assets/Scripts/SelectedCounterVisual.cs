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
        //Subscribe to events where players can interact with cabinet changes
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Change the appearance of the counter when a message is received
    public void Player_OnSelectedCounterChanged(object render, Player.OnSelectedCounterChangedEventsArgs e) {
        // If the selected counter is the current baseCounter, call the Show method to display the
        if (e.selectedCounter == baseCounter) {
            Show();
        } else {
            Hide();
        }
    }

    // Show all GameObjects in the visualGameObjectArray array
    public void Show() {
        // Iterate through the visualGameObjectArray array, setting each GameObject to the active state
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(true);
        }
    }

    // Hide all GameObjects in the visualGameObjectArray array
    public void Hide() {
        // Iterate through the visualGameObjectArray array, setting each GameObject to an inactive state
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(false);
        }
    }
}
