using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StoveCounterVisualTests
{
    GameObject stoveOnGameObject;
    GameObject steamParticleGameObject;
    StoveCounter stoveCounter;
    StoveCounterVisual stoveCounterVisual;

    [SetUp]
    public void Setup()
    {
        // Create game objects for testing
        stoveOnGameObject = new GameObject();
        steamParticleGameObject = new GameObject();

        // Create a stoveCounter object
        stoveCounter = new GameObject().AddComponent<StoveCounter>();

        // Create an instance of StoveCounterVisual
        stoveCounterVisual = stoveOnGameObject.AddComponent<StoveCounterVisual>();
        stoveCounterVisual.stoveOnGameObject = stoveOnGameObject;
        stoveCounterVisual.steamParticleGameObject = steamParticleGameObject;
        stoveCounterVisual.stoveCounter = stoveCounter;
    }

    [Test]
    public void StoveVisual_TurnOnStove_ShouldActivateVisuals()
    {
        // Arrange
        StoveCounter.OnStateChangedEventArgs eventArgs = new StoveCounter.OnStateChangedEventArgs(true);

        // Act
        stoveCounterVisual.StoveCounter_OnStateChanged(null, eventArgs);

        // Assert
        Assert.IsTrue(stoveOnGameObject.activeSelf);
        Assert.IsTrue(steamParticleGameObject.activeSelf);
    }

    [Test]
    public void StoveVisual_TurnOffStove_ShouldDeactivateVisuals()
    {
        // Arrange
        StoveCounter.OnStateChangedEventArgs eventArgs = new StoveCounter.OnStateChangedEventArgs(false);

        // Act
        stoveCounterVisual.StoveCounter_OnStateChanged(null, eventArgs);

        // Assert
        Assert.IsFalse(stoveOnGameObject.activeSelf);
        Assert.IsFalse(steamParticleGameObject.activeSelf);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up objects after each test
        Object.Destroy(stoveOnGameObject);
        Object.Destroy(steamParticleGameObject);
        Object.Destroy(stoveCounter.gameObject);
    }
}

