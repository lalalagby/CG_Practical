using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
 * @author Bingyu Guo
 * 
 * @brief Unit tests for the SelectedCounterVisual class.
 * 
 * @details This class contains unit tests for various functionalities of the SelectedCounterVisual class,
 *          ensuring that the visual elements are correctly shown or hidden based on the SelectedCounter.
 */
public class SelectedCounterVisualEditorTests
{
    private GameObject selectedCounterVisualObject;
    private SelectedCounterVisual selectedCounterVisual;
    private BaseCounter baseCounter;
    private GameObject[] visualGameObjectArray;

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the SelectedCounterVisual, BaseCounter, and visual GameObject array for testing.
     */
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

    /**
     * @brief Tests if Show activates all GameObjects in the visualGameObjectArray.
     * 
     * @details This test case verifies that the Show method sets all GameObjects in the visualGameObjectArray to active.
     */
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

    /**
     * @brief Tests if Hide deactivates all GameObjects in the visualGameObjectArray.
     * 
     * @details This test case verifies that the Hide method sets all GameObjects in the visualGameObjectArray to inactive.
     */
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

    /**
     * @brief Tests if Player_OnSelectedCounterChanged sets visuals active when the SelectedCounter is the BaseCounter.
     * 
     * @details This test case verifies that the Player_OnSelectedCounterChanged method activates all GameObjects 
     *          in the visualGameObjectArray when the SelectedCounter is the BaseCounter.
     */
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

    /**
     * @brief Tests if Player_OnSelectedCounterChanged sets visuals inactive when the SelectedCounter is not the BaseCounter.
     * 
     * @details This test case verifies that the Player_OnSelectedCounterChanged method deactivates all GameObjects 
     *          in the visualGameObjectArray when the SelectedCounter is not the BaseCounter.
     */
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
