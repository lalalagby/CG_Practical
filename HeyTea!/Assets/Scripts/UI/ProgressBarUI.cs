using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 
  * @author Yong Wu
  * 
  * @brief Handles the display and update of a progress bar UI element.
  * 
  * @details The ProgressBarUI class is responsible for displaying and updating a progress bar UI element 
  *          based on the progress reported by an object implementing the IHasProgress interface.
  */
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;  // !< The GameObject that has the IHasProgress component.
    [SerializeField] private Image barImage;    //!< The Image component representing the progress bar.

    private IHasProgress hasProgress;       //!< The IHasProgress component of the hasProgressGameObject.

    /**
     * @brief Initializes the progress bar UI on start.
     * 
     * @details This method is called before the first frame update and initializes the progress bar UI.
     */
    private void Start() {
        Initialize();
    }

    /**
     * @brief Initializes the progress bar UI.
     * 
     * @details This method sets up the progress bar UI by getting the IHasProgress component, 
     *          subscribing to its OnProgressChanged event, and initializing the progress bar fill amount to 0.
     */
    public void Initialize()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game object" + hasProgressGameObject + "not have IHasProgress interface");
        }

        // Subscribe to the OnProgressChanged event of hasProgress and add the Counter_OnProgressChanged method as an event handler.
        hasProgress.OnProgressChanged += Counter_OnProgressChanged;

        // Initialize the progress bar fill to 0
        barImage.fillAmount = 0f;

        // Call the Hide method to hide the progress bar
        Hide();
    }

    /**
     * @brief Event handler method called when the progress changes.
     * 
     * @details This method updates the progress bar fill amount based on the normalized progress value 
     *          and shows or hides the progress bar based on the progress state.
     * 
     * @param sender The source of the event.
     * @param e Event arguments containing the progress data.
     */
    public void Counter_OnProgressChanged(object sender,IHasProgress.OnProgressChangedEventArgs e) {
        //Get the cutting time progress of the subscription
        barImage.fillAmount = e.progressNormalized;

        //When there is no cutting and the cutting is completed, the progress bar disappears, otherwise it keeps appearing.
        if (!e.isProcessing||e.progressNormalized<=0||e.progressNormalized>=1f) {
            Hide();
        } else {
            Show();
        }
    }

    /**
     * @brief Shows the progress bar by setting the current game object to active.
     */
    public void Show() {
        gameObject.SetActive(true);
    }

    /**
     * @brief Hides the progress bar by setting the current game object to inactive.
     */
    public void Hide() {
        gameObject.SetActive(false);
    }
}
