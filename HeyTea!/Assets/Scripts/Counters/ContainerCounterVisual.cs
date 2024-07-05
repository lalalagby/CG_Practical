using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Container interaction animation class.
 * @details This class handles the visual animations for the ContainerCounter, specifically the open and close animations when a player interacts with the container.
 * 
 * @date 28.08.2023
 * @author Yong Wu
 */

/**
 * @class ContainerCounterVisual
 * @brief Handles the visual animations for the ContainerCounter.
 * 
 * This class manages the animations for the ContainerCounter, subscribing to events triggered when the player grabs an object from the container and starting the appropriate animations.
 */
public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose"; /**< The trigger name for the open/close animation. */

    /**
     * @brief The ContainerCounter associated with this visual component.
     */
    [SerializeField] private ContainerCounter containerCounter;

    /**
     * @brief The Animator component used to control animations.
     */
    private Animator animator;

    /**
     * @brief Initializes the animator component.
     */
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /**
     * @brief Subscribes to the OnPlayerGrabbedObject event.
     * 
     * This method subscribes to the OnPlayerGrabbedObject event of the ContainerCounter to trigger animations when the player interacts with the container.
     */
    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    /**
     * @brief Handles the OnPlayerGrabbedObject event.
     * 
     * @details This method is called when the OnPlayerGrabbedObject event is triggered. It starts the open/close animation.
     * 
     * @param sender The event sender.
     * @param e The event arguments.
     */
    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
