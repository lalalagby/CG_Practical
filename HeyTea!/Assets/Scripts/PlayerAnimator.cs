using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief Controls the player's animation state machine.
 * 
 * @details This class changes the animation state of the Player based on whether they are walking or not.
 */

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";  //!< Constant string representing the walking state in the animator.

    [SerializeField] private Player player; //!< Reference to the Player component.

    private Animator animator;  //!< Reference to the Animator component.

    /**
     * @brief Initializes the PlayerAnimator.
     * 
     * @details This method is called when the script instance is being loaded. 
     *          It obtains the Animator component and sets the initial walking state based on the Player's state.
     */
    private void Awake() {
        //Obtain the animation state machine of the player
        animator = GetComponent<Animator>();
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

    /**
     * @brief Updates the animation state.
     * 
     * @details This method is called once per frame and updates the animation state 
     *          based on whether the Player is walking.
     */
    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
