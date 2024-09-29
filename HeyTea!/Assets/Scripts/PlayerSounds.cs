using UnityEngine;

/**
 * @author Bingyu Guo
 * 
 * @date 2024-09-10
 * 
 * @brief Handles player-related sounds, footsteps.
 * 
 * @details This class is responsible for managing and playing sounds related to the player's movements,
 *          specifically footsteps. Footstep sounds are played at a regular interval when the player is walking.
 */
public class PlayerSounds : MonoBehaviour
{
    private Player player;                  //!< Reference to the Player class, used to check player state (e.g., walking).
    private float footstepTimer;            //!< Timer to control the interval between footstep sounds.
    private float footstepTimerMax = 0.1f;  //!< Maximum time between footstep sounds.

    /**
     * @brief Initializes the Player reference.
     * 
     * @details Called when the script instance is being loaded. This method gets the Player component 
     *          associated with the same GameObject.
     */
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    /**
     * @brief Updates the footstep timer and plays footstep sounds if the player is walking.
     * 
     * @details This method decreases the footstep timer each frame and, when it reaches zero, resets it. 
     *          If the player is walking, a footstep sound is played at the player's current position. 
     *          The sound is played at a fixed volume.
     */
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootStepsSound(player.transform.position, volume);
            }
        }
    }
}