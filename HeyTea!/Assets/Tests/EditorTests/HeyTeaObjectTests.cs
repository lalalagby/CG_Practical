using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
 * @class HeyTeaObjectTests
 * @brief Unit tests for the HeyTeaObject class, which handles various functionalities of HeyTeaObject instances.
 * @author Xinyue Cheng
 * @details
 * The HeyTeaObjectTests class includes various test cases to validate the functionalities of the HeyTeaObject class,
 * such as setting parents, interface implementations, and other interactions.
 */
public class HeyTeaObjectTests
{
    /**
     * @brief [TC0201] Test case to validate if SetHeyTeaObjectParents sets the parent correctly.
     */
    [Test]
    public void SetHeyTeaObjectParents_SetsParentCorrectly()
    {
        // Create a HeyTeaObject instance
        GameObject gameObject = new GameObject();
        HeyTeaObject heyTeaObject = gameObject.AddComponent<HeyTeaObject>();

        // Create a virtual parent object
        GameObject parentObject = new GameObject();
        MockHeyTeaObjectParent parent = parentObject.AddComponent<MockHeyTeaObjectParent>();

        // Set the parent object
        heyTeaObject.SetHeyTeaObjectParents(parent);

        // Assert if the parent of the HeyTeaObject is set correctly
        Assert.AreEqual(parentObject.transform, heyTeaObject.transform.parent);
    }

    /**
     * @brief [TC0202] Test case to validate if TryGetKichenware returns true and sets the value correctly when implementing the interface.
     */
    [Test]
    public void TryGetKichenware_WhenImplementingInterface_ReturnsTrueAndSetsValue()
    {
        // Create a HeyTeaObject instance
        GameObject gameObject = new GameObject();
        HeyTeaObject heyTeaObject = gameObject.AddComponent<HeyTeaObject>();

        // Add IKichenwareObejct interface implementation
        MockKichenwareObject kichenwareObject = new MockKichenwareObject();
        heyTeaObject = kichenwareObject;

        // Try to get the kitchenware object
        IKichenwareObejct result;
        bool success = heyTeaObject.TryGetKichenware(out result);

        // Assert if the return value is true
        Assert.IsTrue(success);
        // Assert if the returned object reference is correct
        Assert.AreEqual(kichenwareObject, result);
    }

    /**
     * @brief [TC0203] Test case to validate if TryGetKichenware returns false and sets the value to null when not implementing the interface.
     */
    [Test]
    public void TryGetKichenware_WhenNotImplementingInterface_ReturnsFalseAndSetsNull()
    {
        // Create a HeyTeaObject instance
        GameObject gameObject = new GameObject();
        HeyTeaObject heyTeaObject = gameObject.AddComponent<HeyTeaObject>();

        // Try to get the kitchenware object
        IKichenwareObejct result;
        bool success = heyTeaObject.TryGetKichenware(out result);

        // Assert if the return value is false
        Assert.IsFalse(success);
        // Assert if the returned object reference is null
        Assert.IsNull(result);
    }

    /**
     * @class MockHeyTeaObjectParent
     * @brief Mock implementation of the IHeyTeaObjectParents interface for testing purposes.
     */
    public class MockHeyTeaObjectParent : MonoBehaviour, IHeyTeaObjectParents
    {
        private HeyTeaObject heyTeaObject;

        /**
         * @brief Clears the HeyTeaObject reference.
         */
        public void ClearHeyTeaObject()
        {
            heyTeaObject = null;
        }

        /**
         * @brief Gets the transform to follow for HeyTeaObject.
         * @return The transform of the parent object.
         */
        public Transform GetHeyTeaObjectFollowTransform()
        {
            return transform;
        }

        /**
         * @brief Checks if the parent has a HeyTeaObject.
         * @return True if the parent has a HeyTeaObject, false otherwise.
         */
        public bool HasHeyTeaObject()
        {
            return heyTeaObject != null;
        }

        /**
         * @brief Sets the HeyTeaObject reference.
         * @param heyTeaObject The HeyTeaObject to set.
         */
        public void SetHeyTeaObject(HeyTeaObject heyTeaObject)
        {
            this.heyTeaObject = heyTeaObject;
        }

        /**
         * @brief Gets the HeyTeaObject reference.
         * @return The HeyTeaObject instance.
         */
        public HeyTeaObject GetHeyTeaObject()
        {
            return heyTeaObject;
        }
    }

    /**
     * @class MockKichenwareObject
     * @brief Mock implementation of the IKichenwareObejct interface for testing purposes.
     */
    public class MockKichenwareObject : HeyTeaObject, IKichenwareObejct
    {
        /**
         * @brief Tries to add an ingredient to the kitchenware object.
         * @param heyTeaObjectSO The HeyTeaObjectSO to add.
         * @param materialType The material type of the ingredient.
         * @return True if the ingredient was added successfully, false otherwise.
         */
        public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, IKichenwareObejct.MilkTeaMaterialType materialType)
        {
            return true;
        }
    }
}
