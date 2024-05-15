using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HeyTeaObjectTests
{
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

    public class MockHeyTeaObjectParent : MonoBehaviour, IHeyTeaObjectParents
    {
        private HeyTeaObject heyTeaObject;

        public void ClearHeyTeaObject()
        {
            heyTeaObject = null;
        }

        public Transform GetHeyTeaObjectFollowTransform()
        {
            return transform;
        }

        public bool HasHeyTeaObject()
        {
            return heyTeaObject != null;
        }

        public void SetHeyTeaObject(HeyTeaObject heyTeaObject)
        {
            this.heyTeaObject = heyTeaObject;
        }

        public HeyTeaObject GetHeyTeaObject()
        {
            return heyTeaObject;
        }
    }

    // Mock implementation of IKichenwareObejct interface
    public class MockKichenwareObject : HeyTeaObject, IKichenwareObejct
    {
        public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, IKichenwareObejct.MilkTeaMaterialType materialType)
        {
            return true;
        }
    }
}
