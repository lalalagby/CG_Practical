using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
File Name : ProgressBarUI.cs
Function  : cutting progress bar
Author    : Yong Wu
Data      : 01.09.2023

*/

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError("Game object" + hasProgressGameObject + "not have IHasProgress interface");
        }

        hasProgress.OnProgressChanged += Counter_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }

    private void Counter_OnProgressChanged(object sender,IHasProgress.OnProgressChangedEventArgs e) {
        //Get the cutting time progress of the subscription
        barImage.fillAmount = e.progressNormalized;

        //When there is no cutting and the cutting is completed, the progress bar disappears, otherwise it keeps appearing.
        if (!e.isProcessing||e.progressNormalized<=0||e.progressNormalized>=1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
