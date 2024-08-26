using TMPro;
using UnityEngine;

/**
 * @brief Manages the UI countdown display before the game starts.
 * 
 * @details This class is responsible for updating and showing/hiding the countdown timer
 * on the UI before the game begins. It listens for state changes in the KitchenGameManager
 * and updates the countdown text accordingly.
 * 
 * @date 26.08.2024
 * @author Xinyue Cheng
 */
public class GameStartCountdownUI : MonoBehaviour
{
    /**
     * @brief Text component for displaying the countdown.
     * 
     * @details This serialized field references the TextMeshProUGUI component
     * where the countdown time will be displayed.
     */
    [SerializeField] private TextMeshProUGUI countdownText;

    /**
     * @brief Subscribes to the KitchenGameManager state change event and hides the countdown UI on start.
     * 
     * @details This method subscribes to the `OnStateChanged` event from the KitchenGameManager
     * and initially hides the countdown UI.
     */
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    /**
     * @brief Handles the state change event from the KitchenGameManager.
     * 
     * @details This method is called whenever the KitchenGameManager changes its state.
     * It checks if the countdown to start is active and shows or hides the countdown UI accordingly.
     * 
     * @param sender The event sender.
     * @param e The event arguments.
     */
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    /**
     * @brief Updates the countdown timer text.
     * 
     * @details This method is called every frame to update the countdown timer text on the UI
     * with the remaining time until the game starts.
     */
    private void Update()
    {
        countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
    }

    /**
     * @brief Shows the countdown UI.
     * 
     * @details This method sets the countdown UI GameObject to active, making it visible.
     */
    private void Show()
    {
        gameObject.SetActive(true);
    }

    /**
     * @brief Hides the countdown UI.
     * 
     * @details This method sets the countdown UI GameObject to inactive, hiding it from view.
     */
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
