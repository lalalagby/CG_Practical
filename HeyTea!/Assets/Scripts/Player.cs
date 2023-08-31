using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
File Name : Player.cs
Function  : user control
Author    : Yong Wu
Data      : 16.08.2023

*/


public class Player : MonoBehaviour,IHeyTeaObjectParents
{
    //singleton mode
    public static Player Instance {get;private set;}

    //Define the publish function type
    public event EventHandler<OnSelectedCounterChangedEventsArgs> OnSelectedCounterChanged;

    //Modify the template class of the publish function to determine which cabinet is selected by the player,
    //and propagate the cabinet using publish.
    public class OnSelectedCounterChangedEventsArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    //define the player's move speed
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform heyTeaObjectHoldPoint;

    //Determine the state change of the animation state machine through iswalking.
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private HeyTeaObject heyTeaObject;

    private void Awake() {
        if (Instance!=null)
        {
            Debug.LogError("more than one player instance"); 
        }
        Instance = this;
    }
    public void Start() {
        //As a subscriber to the game input event
        gameInput.OnInteractAction += GameInput_OnInteractioAction;
    }

    //The response function after receiving subscribed events.
    private void GameInput_OnInteractioAction(object sender,System.EventArgs e) {
        //receive the board message, and take actions
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    //callback each update
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    //Interaction function
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
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                moveDir = moveDirX;
            } else {
                //try z axis
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
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

    private void SetSelectedCounter(BaseCounter baseCounter) {
        this.selectedCounter = baseCounter;

        //When the cabinets that can be interacted with change, issue an event to change the selected cabinet.
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventsArgs {
            selectedCounter = baseCounter
        });
    }

    public Transform GetHeyTeaObjectFollowTransform() {
        return heyTeaObjectHoldPoint;
    }
    public void SetHeyTeaObject(HeyTeaObject heyTeaObject) {
        this.heyTeaObject = heyTeaObject;
    }

    public HeyTeaObject GetHeyTeaObject() {
        return heyTeaObject;
    }

    public void ClearHeyTeaObject() {
        heyTeaObject = null;
    }

    public bool HasHeyTeaObject() {
        return heyTeaObject != null;
    }
}
