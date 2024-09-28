/**
 * @brief Manages the pause UI for the game.
 * 
 * @details The GamePauseUI class is responsible for showing and hiding the pause menu when the game is paused or resumed.
 * It handles user input for resuming the game or returning to the main menu.
 * 
 * @author Xinyue Cheng
 * @date 29.09.2024
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * @class GamePauseUI
 * @brief Handles the pause menu UI in the game.
 * 
 * @details This class manages the pause menu, allowing players to resume the game or return to the main menu.
 * It listens for events from the KitchenGameManager to show or hide the pause menu depending on the game's state.
 */
public class GamePauseUI : MonoBehaviour
{
    /**
     * @brief The button to resume the game.
     */
    [SerializeField] private Button resumeButton;

    /**
     * @brief The button to return to the main menu.
     */
    [SerializeField] private Button mainMenuButton;

    /**
     * @brief Initializes button click listeners and subscribes to game pause events.
     * 
     * @details Sets up listeners for the resume button and main menu button. The resume button calls 
     * the TogglePauseGame function in the KitchenGameManager, while the main menu button loads the main menu scene.
     */
    private void Awake()
    {
        // Listener for the resume button.
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });

        // Listener for the main menu button.
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    /**
     * @brief Subscribes to the pause and unpause events at the start of the game.
     * 
     * @details Subscribes to OnGamePaused and OnGameUnpaused events from the KitchenGameManager to show
     * or hide the pause menu accordingly.
     */
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        Hide();
    }

    /**
     * @brief Event handler for when the game is paused.
     * 
     * @details This method is called when the game is paused, and it shows the pause menu.
     * 
     * @param sender The sender of the event.
     * @param e Event arguments.
     */
    private void KitchenGameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    /**
     * @brief Event handler for when the game is unpaused.
     * 
     * @details This method is called when the game is unpaused, and it hides the pause menu.
     * 
     * @param sender The sender of the event.
     * @param e Event arguments.
     */
    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    /**
     * @brief Shows the pause menu UI.
     * 
     * @details This method activates the pause menu game object to make it visible.
     */
    private void Show()
    {
        gameObject.SetActive(true);
    }

    /**
     * @brief Hides the pause menu UI.
     * 
     * @details This method deactivates the pause menu game object to hide it from view.
     */
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
