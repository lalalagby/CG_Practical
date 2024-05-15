using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] public GameObject stoveOnGameObject;
    [SerializeField] public GameObject steamParticleGameObject;
    [SerializeField] public StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object render, StoveCounter.OnStateChangedEventArgs e) {
        stoveOnGameObject.SetActive(e.isCooking);
        steamParticleGameObject.SetActive(e.isCooking);
    }
}
