using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject steamParticleGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object render, StoveCounter.OnStateChangedEventArgs e) {
        stoveOnGameObject.SetActive(e.isCooking);
        steamParticleGameObject.SetActive(e.isCooking);
    }
}
