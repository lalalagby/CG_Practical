using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
File Name : ProgressBarUI.cs
Function  : cutting progress bar
Author    : Yong Wu
Created      : 01.09.2023
Last Modified by: Bingyu Guo
Last Modification Date  :   14.05.2024
*/

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start() {
        Initialize();
    }

    // Used to initialize the progress bar UI
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

    // Event handler method, called when the progress changes.
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

    // A method to display a progress bar that sets the current game object as active.
    public void Show() {
        gameObject.SetActive(true);
    }

    // Hide the progress bar by setting the current game object to inactive.
    public void Hide() {
        gameObject.SetActive(false);
    }
}
