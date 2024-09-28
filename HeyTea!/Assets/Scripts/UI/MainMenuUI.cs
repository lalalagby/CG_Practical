/**
 * @brief Handles the functionality of the main menu in the game.
 * 
 * @details The MainMenuUI class manages the main menu's play and quit buttons. When the player clicks 
 * the play button, it loads the main game scene. When the quit button is pressed, it exits the game.
 * Additionally, it resets the game's time scale to 1 in case it was modified during gameplay.
 * 
 * @author Xinyue Cheng
 * @date 29.09.2024
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * @class MainMenuUI
 * @brief A class responsible for managing the main menu UI in the game.
 * 
 * @details The class manages user interactions in the main menu, specifically starting the game or quitting the application.
 * It listens for button click events and reacts accordingly. 
 */
public class MainMenuUI : MonoBehaviour
{
    /**
     * @brief Button to start the game.
     */
    [SerializeField] private Button playButton;

    /**
     * @brief Button to quit the game.
     */
    [SerializeField] private Button quitButton;

    /**
     * @brief Initializes button click listeners and resets time scale.
     * 
     * @details This method assigns listeners to the play and quit buttons. The play button loads the 
     * main game scene, while the quit button closes the application. It also ensures that the time scale is set to normal (1).
     */
    private void Awake()
    {
        // Listener for the play button to load the main game scene.
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1); //!< Loads the main game scene (scene index 1).
        });

        // Listener for the quit button to exit the game.
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit(); //!< Quits the application.
        });

        // Resets the time scale to ensure the game runs at normal speed when starting.
        Time.timeScale = 1f;
    }
}
