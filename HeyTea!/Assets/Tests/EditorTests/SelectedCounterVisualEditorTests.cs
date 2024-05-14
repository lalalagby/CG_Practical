using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
File Name : SelectedCounterEditorTests.cs
Author    : Bingyu Guo
Created   : 14.05.2024
*/
public class SelectedCounterVisualEditorTests
{
    private GameObject selectedCounterVisualObject;
    private SelectedCounterVisual selectedCounterVisual;
    private BaseCounter baseCounter;
    private GameObject[] visualGameObjectArray;

    [SetUp]
    public void Setup()
    {
        selectedCounterVisualObject = new GameObject();
        selectedCounterVisual = selectedCounterVisualObject.AddComponent<SelectedCounterVisual>();
        baseCounter = selectedCounterVisualObject.AddComponent<BaseCounter>();
        selectedCounterVisualObject.GetComponent<SelectedCounterVisual>().GetType().GetField("baseCounter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(selectedCounterVisual, baseCounter);

        visualGameObjectArray = new GameObject[2];
        for (int i = 0; i < visualGameObjectArray.Length; i++)
        {
            visualGameObjectArray[i] = new GameObject();
        }
        selectedCounterVisualObject.GetComponent<SelectedCounterVisual>().GetType().GetField("visualGameObjectArray", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(selectedCounterVisual, visualGameObjectArray);
    }

    [Test]
    public void Show_ActivatesVisualGameObjectArray()
    {
        // Act
        selectedCounterVisual.Show();

        // Assert
        foreach (GameObject go in visualGameObjectArray)
        {
            Assert.IsTrue(go.activeSelf);
        }
    }

    [Test]
    public void Hide_DeactivatesVisualGameObjectArray()
    {
        // Act
        selectedCounterVisual.Hide();

        // Assert
        foreach (GameObject go in visualGameObjectArray)
        {
            Assert.IsFalse(go.activeSelf);
        }
    }

    [Test]
    public void Player_OnSelectedCounterChanged_SetsVisualsActive_WhenSelectedCounterIsBaseCounter()
    {
        // Arrange
        var eventArgs = new Player.OnSelectedCounterChangedEventsArgs { selectedCounter = baseCounter };

        // Act
        selectedCounterVisual.Player_OnSelectedCounterChanged(null, eventArgs);

        // Assert
        foreach (GameObject go in visualGameObjectArray)
        {
            Assert.IsTrue(go.activeSelf);
        }
    }

    [Test]
    public void Player_OnSelectedCounterChanged_SetsVisualsInactive_WhenSelectedCounterIsNotBaseCounter()
    {
        // Arrange
        var eventArgs = new Player.OnSelectedCounterChangedEventsArgs { selectedCounter = new GameObject().AddComponent<BaseCounter>() };

        // Act
        selectedCounterVisual.Player_OnSelectedCounterChanged(null, eventArgs);

        // Assert
        foreach (GameObject go in visualGameObjectArray)
        {
            Assert.IsFalse(go.activeSelf);
        }
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(selectedCounterVisualObject);
    }
}
