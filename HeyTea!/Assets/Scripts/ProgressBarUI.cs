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
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start() {
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender,CuttingCounter.OnProgressChangedEventArgs e) {
        //Get the cutting time progress of the subscription
        barImage.fillAmount = e.progressNormalized;

        //When there is no cutting and the cutting is completed, the progress bar disappears, otherwise it keeps appearing.
        if (e.progressNormalized == 0f || e.progressNormalized >= 1f) {
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
