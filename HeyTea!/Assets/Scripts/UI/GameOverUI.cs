using TMPro;
using UnityEngine;

/**
 * @brief Handles the display of the Game Over UI in the HeyTea game.
 * 
 * @details The GameOverUI class is responsible for showing or hiding the Game Over screen
 * based on the state of the game. It listens to the state changes from the KitchenGameManager
 * and updates the UI accordingly.
 * 
 * @date 30.08.2024
 * @author Xinyue Cheng
 */
public class GameOverUI : MonoBehaviour
{
    /**
     * @brief UI element to display the number of recipes delivered.
     * 
     * @details This serialized field references the TextMeshProUGUI component that
     * will display the number of recipes delivered by the player when the game ends.
     */
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    /**
     * @brief Initializes the GameOverUI by subscribing to the game state change event.
     * 
     * @details The Start method subscribes to the OnStateChanged event from the KitchenGameManager,
     * so the UI can respond to game state changes. Initially, the Game Over UI is hidden.
     */
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnGameStateChanged;
        Hide();
    }

    /**
     * @brief Handles the game state change event.
     * 
     * @details This method is called whenever the game state changes. It checks if the game is over and
     * shows the Game Over UI if it is, or hides it otherwise.
     * 
     * @param sender The source of the event.
     * @param e The event data.
     */
    private void KitchenGameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    /**
     * @brief Shows the Game Over UI.
     * 
     * @details This method activates the Game Over UI, making it visible to the player.
     */
    private void Show()
    {
        gameObject.SetActive(true);
    }

    /**
     * @brief Hides the Game Over UI.
     * 
     * @details This method deactivates the Game Over UI, making it invisible to the player.
     */
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
