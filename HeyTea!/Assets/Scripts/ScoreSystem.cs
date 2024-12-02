using System;
using UnityEngine;

/**
 * @author Bingyu Guo
 * 
 * @date 2024-07-05
 * 
 * @brief Manages the scoring system in the game.
 * 
 * This class handles the player's score and triggers relevant events.
 */
[ExecuteInEditMode]
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }    //!< Singleton instance of the ScoreSystem class.

    private int currentScore;   //!< The current score of the player.
    private int targetScore = 30;   //!< The target score to reach to end the game.

    public event System.Action<int> OnScoreChanged;     //!< Event triggered when the score changes.
    //public event System.Action OnTargetScoreReached;    //!< Event triggered when the target score is reached.

    private void Awake() {  
        Instance = this;
    }

    /**
     * @brief Subscribes to the OrderListManager's OnOrderCompleted event.
     * 
     * @details This method is called on the frame when a script is enabled 
     *          just before any of the Update methods are called the first time.
     */
    private void Start() {
        // Subscribe to OrderListManager's OnOrderCompleted event
        OrderListManager.Instance.OnOrderCompleted += HandleOrderCompleted;
    }

    /**
     * @brief Handles the event when an order is completed.
     * 
     * @details This method is called when the OnOrderCompleted event is triggered in OrderListManager. 
     *          It adds 10 points to the score.
     * 
     * @param sender The source of the event.
     * @param e Event arguments.
     */
    public void HandleOrderCompleted(object sender, EventArgs e) {
        AddScore(10);
    }

    /**
     * @brief Adds score to the current score.
     * 
     * @details This method adds the specified score to the current score 
     *          and triggers the OnScoreChanged event. 
     * 
     * @param score The score to be added.
     */
    public void AddScore(int score) {
        currentScore += score;
        OnScoreChanged?.Invoke(currentScore);

        // Reach the target score and the game ends.
        //if (currentScore >= targetScore)  {
        //    OnTargetScoreReached?.Invoke();
        //}
    }

    /**
     * @brief Gets the current score.
     */
    public int GetCurrentScore() {
        return currentScore;
    }

    /**
     * @brief Based on the score to decide if the player win the game.
     */
    public bool GameWin()
    {
        if (currentScore < targetScore)
            return false;
        else
            return true;
    }
}
