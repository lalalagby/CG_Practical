using UnityEngine;
using UnityEngine.UI;


/**
 * @author Bingyu Guo
 * 
 * @date 2024-07-05
 * 
 * @brief Manages the display of the score and handles the game over UI in the game.
 * 
 * @details This class is responsible for updating the score display in the UI 
 *          and handling the game over logic when the target score is reached. 
 *          It subscribes to the ScoreSystem events to update the score text and show the game over panel.
 * 
 */
public class ScoreSystemUI : MonoBehaviour {
    [SerializeField] private Text scoreText;    //!< The UI Text component that displays the current score.
    [SerializeField] private Image backgroundCircle;    //!< The UI Image component representing the background circle.
    //[SerializeField] private GameObject gameOverPanel; //!< A panel to display when the game is over

    /**
     * @brief Initializes the ScoreSystemUI.
     * 
     * @details This method is called when the script instance is being loaded. 
     *          It sets the game over panel to inactive, subscribes to the ScoreSystem events, 
     *          and updates the score display.
     */
    private void Start() {
        //gameOverPanel.SetActive(false);

        ScoreSystem.Instance.OnScoreChanged += UpdateScoreDisplay;
        //ScoreSystem.Instance.OnTargetScoreReached += HandleTargetScoreReached;

        UpdateScoreDisplay(ScoreSystem.Instance.GetCurrentScore());
    }

    /**
     * @brief Updates the score display in the UI.
     * 
     * @details This method is called whenever the score changes to update the score text in the UI.
     * 
     * @param score The current score to be displayed.
     */
    private void UpdateScoreDisplay(int score) {
        scoreText.text = score.ToString();
    }

    ///**
    // * @brief Handles the game over logic when the target score is reached.
    // * 
    // * @details This method is called when the target score is reached.
    // *          It handles the game over logic and displays the game over panel.
    // */
    //private void HandleTargetScoreReached() {
    //    // Handle game over logic here
    //    Debug.Log("Target score reached! Game over!");

    //    // Show game over panel
    //    gameOverPanel.SetActive(true);
    //}
}
