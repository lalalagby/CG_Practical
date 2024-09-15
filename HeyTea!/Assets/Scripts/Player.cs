using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/**
 * @author Yong Wu, Bingyu Guo, Xinyue Cheng
 * 
 * @brief Controls the Player's behavior and interactions.
 * 
 * @details This class manages the Player's movement, interactions with Counters, and animation state. 
 *          It also handles events related to player actions.
 */
public class Player : MonoBehaviour,IHeyTeaObjectParents
{
    public event EventHandler OnPickUpSomething;
    public static Player Instance {get;private set; }    //!< Singleton instance of the Player class.

    //! Event triggered when the selected counter changes.
    public event EventHandler<OnSelectedCounterChangedEventsArgs> OnSelectedCounterChanged;

    /**
     * @brief Arguments for the OnSelectedCounterChanged event.
     * 
     * @details Modify the template class of the publish function to determine 
     *          which cabinet is selected by the player, and propagate the cabinet using publish.
     */
    public class OnSelectedCounterChangedEventsArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 10f; //!< The Player's movement speed.
    [SerializeField] private GameInput gameInput;   //!< Reference to the GameInput component for handling input.
    [SerializeField] private LayerMask countersLayerMask;   //!< Layer mask for detecting counters.
    [SerializeField] private Transform heyTeaObjectHoldPoint;   //!< Transform representing the point where HeyTeaObjects are held by the Player.

    private bool isWalking;     //!< Indicates whether the Player is currently walking.
    private Vector3 lastInteractDir;    //!< Stores the last direction the player interacted with.
    private BaseCounter selectedCounter;    //!< The currently selected counter.
    private HeyTeaObject heyTeaObject;      //!< The HeyTeaObject currently held by the Player.

    /**
     * @brief Initializes the player instance.
     * 
     * @details This method ensures that only one instance of the player exists.
     */
    private void Awake() {
        if (Instance!=null)
        {
            Debug.LogError("more than one player instance"); 
        }
        Instance = this;
    }

    /**
     * @brief Subscribes to game input events.
     * 
     * @details This method subscribes to various game input events to handle player interactions.
     */
    public void Start() {
        //As a subscriber to the game input event
        gameInput.OnInteractAction += GameInput_OnInteractionAction;
        gameInput.OperationHoldAction += GameInput_OnOperationHoldAction;
        gameInput.OperationAction += GameInput_OnOperationAction;
    }

    /**
     * @brief Handles interaction input from the player.
     * 
     * This method is called when the player interacts with an object.
     * 
     * @param sender The source of the event.
     * @param e Event arguments.
     */
    private void GameInput_OnInteractionAction(object sender, System.EventArgs e) {
        //receive the board message, and take actions
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    /**
     * @brief Handles operation input from the Player.
     * 
     * @details This method is called when the Player performs an operation on an object.
     * 
     * @param sender The source of the event.
     * @param e Event arguments.
     */
    private void GameInput_OnOperationAction(object sender, System.EventArgs e) {
        //receive the board message, and take actions
        if (selectedCounter != null) {
            selectedCounter.Operation(this);
        }
    }

    /**
     * @brief Handles hold operation input from the Player.
     * 
     * @details This method is called when the Player performs a hold operation on an object.
     * 
     * @param sender The source of the event.
     * @param e Event arguments containing the hold time.
     */
    private void GameInput_OnOperationHoldAction(object sender, GameInput.OnOperationHoldActionEventArgs e) {
        //receive the board message, and take actions
        if (selectedCounter != null) {
            selectedCounter.OperationHold(this,e.Time);
        }
    }

    /**
     * @brief Updates the Player's state each frame.
     * 
     * @details This method is called once per frame and handles Player movement and interactions.
     */
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    /**
     * @brief Checks if the player is walking.
     * 
     * @return True if the player is walking, false otherwise.
     */
    public bool IsWalking() {
        return isWalking;
    }

    /**
     * @brief Handles player interactions with objects.
     * 
     * @details This method processes input to interact with objects in the game world.
     */
    private void HandleInteractions() {
        //Obtain the character's movement operation through the input class.
        Vector2 inputVector = gameInput.GetMoveVector();

        //Convert to a two-dimensional direction of movement.
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //Because when facing the cabinet, it is necessary to keep the selected state
        //so the direction of the previous movement needs to be saved.
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        //Interaction distance
        float interactDistance = 2f;
        //Determine whether there is a collision object facing in the direction using radiation.
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            //Determine whether the collision object is a cabinet
            if (raycastHit.transform.TryGetComponent(out BaseCounter clearCounter)) {
                //has clear counter
                if(clearCounter != selectedCounter) {
                    SetSelectedCounter( clearCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    /**
     * @brief Handles Player movement.
     * 
     * @details This method processes input to move the Player in the game world.
     */
    private void HandleMovement() {
        //Get location updates through the input system package
        Vector2 inputVector = gameInput.GetMoveVector();

        //transfer to game coordinate
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //collision detection
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        //update player postion
        if (!canMove) {
            //first attempt x axis
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                moveDir = moveDirX;
            } else {
                //try z axis
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 &&!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        //update location
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        //update animation flag
        isWalking = moveDir != Vector3.zero;

        //update dirction
        float rotatedSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotatedSpeed * Time.deltaTime);
    }

    /**
     * @brief Sets the currently selected counter.
     * 
     * @details This method updates the selected counter and triggers the OnSelectedCounterChanged event.
     * 
     * @param baseCounter The new selected counter.
     */
    private void SetSelectedCounter(BaseCounter baseCounter) {
        this.selectedCounter = baseCounter;

        //When the cabinets that can be interacted with change, issue an event to change the selected cabinet.
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventsArgs {
            selectedCounter = baseCounter
        });
    }

    /**
     * @brief Gets the transform where HeyTeaObjects are held by the Player.
     * 
     * @return The transform where HeyTeaObjects are held.
     */
    public Transform GetHeyTeaObjectFollowTransform() {
        return heyTeaObjectHoldPoint;
    }

    /**
     * @brief Sets the HeyTeaObject held by the Player.
     * 
     * @param heyTeaObject The HeyTeaObject to be held by the Player.
     */
    public void SetHeyTeaObject(HeyTeaObject heyTeaObject) {
        this.heyTeaObject = heyTeaObject;
        if (heyTeaObject != null)
        {
            OnPickUpSomething?.Invoke(this, EventArgs.Empty);
        }

    }

    /**
     * @brief Gets the HeyTeaObject held by the Player.
     * 
     * @return The HeyTeaObject held by the Player.
     */
    public HeyTeaObject GetHeyTeaObject() {
        return heyTeaObject;
    }

    /**
     * @brief Clears the HeyTeaObject held by the Player.
     */
    public void ClearHeyTeaObject() {
        heyTeaObject = null;
    }

    /**
     * @brief Checks if the Player is holding a HeyTeaObject.
     * 
     * @return True if the Player is holding a HeyTeaObject, false otherwise.
     */
    public bool HasHeyTeaObject() {
        return heyTeaObject != null;
    }
}
