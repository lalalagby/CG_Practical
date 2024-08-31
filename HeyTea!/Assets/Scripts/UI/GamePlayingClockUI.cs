using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @brief Handles the UI for the game-playing clock in the HeyTea game.
 * 
 * @details The GamePlayingClockUI class is responsible for updating the visual representation of the
 * game-playing timer. It uses a UI Image component to display the remaining time as a filled amount,
 * which is updated every frame during gameplay.
 * 
 * @date 30.08.2024
 * @author Xinyue Cheng
 */
public class GamePlayingClockUI : MonoBehaviour
{
    /**
     * @brief Reference to the UI Image that represents the timer.
     * 
     * @details This serialized field is used to connect the Image component in the Unity Editor,
     * which visually shows the remaining game time as a fill amount.
     */
    [SerializeField] private Image timerImage;

    /**
     * @brief Updates the timer UI every frame.
     * 
     * @details The Update method continuously updates the fill amount of the timerImage
     * based on the normalized game-playing timer from the KitchenGameManager. The fill amount
     * decreases as the game progresses, visually indicating the remaining time to the player.
     */
    private void Update()
    {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
