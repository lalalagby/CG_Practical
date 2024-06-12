using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief Manages the visual representation of the selected counter.
 * 
 * @details This class handles the visual changes of the counter when it is selected or deselected by the player.
 *          It subscribes to the player's selected counter change event and updates the visual appearance accordingly.
 */

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;       // The base counter associated with this visual.
    [SerializeField] private GameObject[] visualGameObjectArray;    // Array of GameObjects that represent the visual elements of the counter.

    /**
     * @brief Adds a subscriber to the player's selected counter change event.
     * 
     * @details This method subscribes to the OnSelectedCounterChanged event of the Player instance,
     *          so that it can respond to changes in the selected counter.
     */
    private void Start() {
        //Subscribe to events where players can interact with cabinet changes
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    /**
     * @brief Changes the appearance of the counter when a message is received.
     * 
     * @details This method is called when the OnSelectedCounterChanged event is triggered. 
     *          It updates the visual representation of the counter based on whether it is 
     *          the currently SelectedCounter.
     * 
     * @param render The source of the event.
     * @param e Event arguments containing information about the SelectedCounter.
     */
    public void Player_OnSelectedCounterChanged(object render, Player.OnSelectedCounterChangedEventsArgs e) {
        // If the selected counter is the current baseCounter, call the Show method to display the
        if (e.selectedCounter == baseCounter) {
            Show();
        } else {
            Hide();
        }
    }

    /**
     * @brief Shows all GameObjects in the visualGameObjectArray array.
     * 
     * @details This method sets each GameObject in the visualGameObjectArray to the active state,
     *          making them visible in the game.
     */
    public void Show() {
        // Iterate through the visualGameObjectArray array, setting each GameObject to the active state
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(true);
        }
    }

    /**
     * @brief Hides all GameObjects in the visualGameObjectArray array.
     * 
     * @details This method sets each GameObject in the visualGameObjectArray to the inactive state,
     *          making them invisible in the game.
     */
    public void Hide() {
        // Iterate through the visualGameObjectArray array, setting each GameObject to an inactive state
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(false);
        }
    }
}
