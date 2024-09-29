using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/**
 * @author Bingyu Guo
 * 
 * @date 2024-09-10
 * 
 * @brief Manages the user interface for game settings, including sound and music volume.
 * 
 * @details This class controls the settings panel in the game, allowing players to adjust 
 *          sound effects and music volume. It includes methods for showing and hiding the 
 *          panel, updating the UI based on current volume levels, and reacting to button 
 *          inputs for changing the volume. A singleton pattern is used to ensure only one 
 *          instance of the SettingsUI exists.
 *          
 */
public class SettingsUI : MonoBehaviour
{
    /// Action to be called when the close button is clicked.
    private Action onCloseButtonAction;

    /// Singleton instance of SettingsUI.
    public static SettingsUI Instance { get; private set; }

    [SerializeField] private Button musicButton;                //!< Button to adjust music volume.
    [SerializeField] private Button soundEffectsButton;         //!< Button to adjust sound effects volume.
    [SerializeField] private Button closeButton;                //!< Button to close the settings menu.
    [SerializeField] private TextMeshProUGUI musicText;         //!< Text displaying the current music volume.
    [SerializeField] private TextMeshProUGUI soundEffectsText;  //!< Text displaying the current sound effects volume.

    /**
     * @brief Initializes the singleton instance and button listeners.
     * 
     * @details Adds listeners to the buttons for adjusting sound and music volume,
     *          and closes the settings menu when the close button is clicked.
     */
    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });
    }

    /**
     * @brief Called on start to initialize the UI and hide the settings menu.
     */
    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        UpdateVisual();
        Hide();
    }

    /**
     * @brief Updates the displayed sound and music volume values.
     * 
     * @details Fetches the current volume levels from SoundManager and MusicManager
     *          and updates the corresponding UI text elements.
     */
    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    /**
     * @brief Hides the settings menu when the game is unpaused.
     * 
     * @param sender The object that triggered the event.
     * @param e The event arguments.
     */
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    /**
     * @brief Shows the settings menu and sets the action for the close button.
     * 
     * @param onCloseButtonAction The action to call when the close button is clicked.
     */
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    /**
     * @brief Hides the settings menu.
     */
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
