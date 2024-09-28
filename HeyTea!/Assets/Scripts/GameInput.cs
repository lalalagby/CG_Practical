using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

/**
 * @brief Handles player input using the newer input manager.
 * @details This class manages player input for interactions and movements in the game using the newer input manager.
 * It broadcasts events for interaction, operation hold, and operation actions.
 * 
 */

/**
 * @class GameInput
 * @brief Manages player input and broadcasts related events.
 * 
 * This class handles player input using the newer input manager, broadcasting events for interaction, operation hold,
 * and operation actions. It also provides methods to get the movement vector.
 */
public class GameInput : MonoBehaviour
{
    /** 
     * @brief Event triggered when the interact action is performed.
     */
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;
    /**
     * @brief Event triggered when the operation hold action is performed.
     */
    public event EventHandler<OnOperationHoldActionEventArgs> OperationHoldAction;

    /**
     * @brief Event arguments for the operation hold action.
     */
    public class OnOperationHoldActionEventArgs : EventArgs
    {
        public float Time;
    }

    /** 
     * @brief Event triggered when the operation action is performed.
     */
    public event EventHandler OperationAction;

    /**
     * @brief Player input actions instance.
     */
    private PlayerInputActions playerInputActions;

    /**
     * @brief Flag to check if the operation hold action is being performed.
     */
    private bool isHolding;

    /**
     * @brief Interval for sending the operation hold action.
     */
    private float sendInterval = 0.1f;

    /**
     * @brief Cumulative time for the operation hold action interval.
     */
    private float cumulativeTime = 0f;

    /**
     * @brief Initializes the player input actions and subscribes to input events.
     */
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Subscribe to user input events
        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.Operation.performed += Operation_Performed;
        playerInputActions.Player.Operation.canceled += Operation_Canceled;
        playerInputActions.Player.Pause.performed += Pause_Performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_Performed;
        playerInputActions.Player.Operation.performed -= Operation_Performed;
        playerInputActions.Player.Pause.performed -= Pause_Performed;

        playerInputActions.Dispose();
    }

    private void Pause_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    /**
     * @brief Updates the operation hold action based on the cumulative time.
     */
    private void Update()
    {
        // If user is holding the interact key, we will send subscription per interval
        if (isHolding)
        {
            if (cumulativeTime >= sendInterval)
            {
                OperationHoldAction?.Invoke(this, new OnOperationHoldActionEventArgs { Time = sendInterval });
                cumulativeTime = 0f;
            }
            else
            {
                cumulativeTime += Time.deltaTime;
            }

        }
    }

    /**
     * @brief Handles the interact action performed event.
     * @param obj The callback context of the performed action.
     */
    public void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // If the subscriber number is not zero, then we can board this message
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    /**
     * @brief Handles the operation action performed event.
     * @param obj The callback context of the performed action.
     */
    public void Operation_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // If the subscriber number is not zero, then we can board this message
        if (obj.interaction is HoldInteraction)
        {
            isHolding = true;
        }
        OperationAction?.Invoke(this, EventArgs.Empty);

    }

    /**
     * @brief Handles the operation action canceled event.
     * @param obj The callback context of the canceled action.
     */
    public void Operation_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // If the subscriber number is not zero, then we can board this message
        if (obj.interaction is HoldInteraction)
        {
            isHolding = false;
        }
    }

    /**
     * @brief Gets the movement vector based on player input.
     * @return The normalized movement vector.
     */
    public Vector2 GetMoveVector()
    {
        // Input vector, x = x axis move, y = y axis move
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Normalize move to 1
        inputVector = inputVector.normalized;

        return inputVector;
    }

}
