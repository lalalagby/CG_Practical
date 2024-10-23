using System;
using UnityEngine;

/**
 * @brief Manages the overall game state in the HeyTea game.
 * 
 * @details The KitchenGameManager class is responsible for managing the different states of the game, 
 * such as waiting to start, counting down to the start, gameplay, and game over. It also triggers events
 * when the game state changes, allowing other components to react accordingly.
 * 
 * @date 26.08.2024
 * @author Xinyue Cheng
 */
public class KitchenGameManager : MonoBehaviour
{
    /**
     * @brief Singleton instance of the KitchenGameManager.
     * 
     * @details This static property holds the singleton instance of the KitchenGameManager,
     * allowing it to be accessed globally within the game.
     */
    public static KitchenGameManager Instance { get; private set; }

    /**
     * @brief Event triggered when the game state changes.
     * 
     * @details Components can subscribe to this event to be notified whenever the game state changes.
     */
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    /**
     * @brief Represents the different states of the game.
     * 
     * @details The State enum defines the various states the game can be in: 
     * WaitingToStart, CountdownToStart, GamePlaying, and GameOver.
     */
    private enum State
    {
        WaitingToStart,   ///< The game is waiting to start.
        CountdownToStart, ///< The game is counting down to the start.
        GamePlaying,      ///< The game is in progress.
        GameOver,         ///< The game has ended.
    }

    private State state; ///< The current state of the game.
    private float waitingToStartTimer = 1f; ///< Timer for the WaitingToStart state.
    private float countdownToStartTimer = 3f; ///< Timer for the CountdownToStart state.
    private float gamePlayingTimer; ///< Timer for the GamePlaying state.
    private float gamePlayingTimerMax = 100f;
    private bool isGamePaused = false;

    /**
     * @brief Initializes the singleton instance and sets the initial game state.
     * 
     * @details The Awake method is called when the script instance is being loaded. It initializes
     * the singleton instance and sets the initial game state to WaitingToStart.
     */
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction; 
    }
    private void GameInput_OnPauseAction(object sender,EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }

    }

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    /**
     * @brief Updates the game state based on timers.
     * 
     * @details The Update method is called once per frame. It handles the transitions between different
     * game states based on the countdown timers, and triggers the OnStateChanged event when the state changes.
     */
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                // Game over state logic can be added here.
                break;
        }

        Debug.Log(state);
    }

    /**
     * @brief Checks if the game is currently in the GamePlaying state.
     * 
     * @return True if the game is in the GamePlaying state; otherwise, false.
     */
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    /**
     * @brief Checks if the CountdownToStart state is active.
     * 
     * @return True if the CountdownToStart state is active; otherwise, false.
     */
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    /**
     * @brief Gets the remaining time for the countdown to start.
     * 
     * @return The remaining time in seconds for the CountdownToStart state.
     */
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - gamePlayingTimer / gamePlayingTimerMax;
    }
}
