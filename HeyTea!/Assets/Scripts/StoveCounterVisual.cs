using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief Manages the visual representation of the StoveCounter.
 * 
 * @details This class handles the visual changes of the StoveCounter, such as turning on the Stove
 *          and activating steam particles when the stove is in use.
 */
public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] public GameObject stoveOnGameObject;   //!< GameObject representing the visual state of the stove being on.
    [SerializeField] public GameObject steamParticleGameObject;     //!< GameObject representing the steam particle effects when the stove is in use.
    [SerializeField] public StoveCounter stoveCounter;  //!< Reference to the StoveCounter component.

    /**
     * @brief Subscribes to the StoveCounter's state change event on start.
     * 
     * @details This method subscribes to the OnStateChanged event of the StoveCounter to update
     *          the visual state based on whether the Stove is cooking.
     */
    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    /**
     * @brief Updates the visual state of the StoveCounter based on its state.
     * 
     * @details This method is called when the OnStateChanged event is triggered. It updates
     *          the active state of the stoveOnGameObject and steamParticleGameObject based on
     *          whether the Stove is cooking.
     * 
     * @param render The source of the event.
     * @param e Event arguments containing the state of the StoveCounter.
     */
    private void StoveCounter_OnStateChanged(object render, StoveCounter.OnStateChangedEventArgs e) {
        stoveOnGameObject.SetActive(e.isCooking);
        steamParticleGameObject.SetActive(e.isCooking);
    }
}
