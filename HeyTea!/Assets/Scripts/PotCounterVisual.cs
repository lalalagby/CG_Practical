using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotCounterVisual : MonoBehaviour
{
    [SerializeField] public GameObject potOnGameObject;
    [SerializeField] public GameObject steamParticleGameObject;
    [SerializeField] public PotCounter potCounter;

    private void Start()
    {
        potCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object render, PotCounter.OnStateChangedEventArgs e)
    {
        potOnGameObject.SetActive(e.isCooking);
        steamParticleGameObject.SetActive(e.isCooking);
    }
}
