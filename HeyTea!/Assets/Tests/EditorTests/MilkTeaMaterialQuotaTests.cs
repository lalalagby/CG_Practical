using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;

/**
 * @class MilkTeaMaterialQuotaTests
 * @brief Unit tests for the MilkTeaMaterialQuota class, which handles the functionalities related to managing quotas of milk tea materials.
 * @author Xinyue Cheng
 * @details
 * The MilkTeaMaterialQuotaTests class includes various test cases to validate the functionalities of the MilkTeaMaterialQuota class,
 * such as adding ingredients, checking mix status, and clearing all objects.
 */
public class MilkTeaMaterialQuotaTests
{
    /**
     * @brief [TC0301] Test case to validate if CanAdd returns true when canMixed is true and the current number is below the max limit.
     */
    [Test]
    public void CanAdd_WhenCanMixedIsTrueAndCurrentNumberIsBelowMax_ReturnsTrue()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            canMixed = true,
            totalSum = 10,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 3, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 2, maxNum = 5 }
            }
        };
        HeyTeaObjectSO heyTeaObjectSO = new HeyTeaObjectSO(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsTrue(result);
    }

    /**
     * @brief [TC0302] Test case to validate if CanAdd returns false when canMixed is true and the current number is at the max limit.
     */
    [Test]
    public void CanAdd_WhenCanMixedIsTrueAndCurrentNumberIsAtMax_ReturnsFalse()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            canMixed = true,
            totalSum = 10,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 }
            }
        };
        HeyTeaObjectSO heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsFalse(result);
    }

    /**
     * @brief [TC0303] Test case to validate if CanAdd returns true when canMixed is false and the current number is below the max limit.
     */
    [Test]
    public void CanAdd_WhenCanMixedIsFalseAndCurrentNumberIsBelowMax_ReturnsTrue()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            canMixed = false,
            totalSum = 10,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 3, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 2, maxNum = 5 }
            }
        };
        HeyTeaObjectSO heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsTrue(result);
    }

    /**
     * @brief [TC0304] Test case to validate if CanAdd returns false when the total sum is reached.
     */
    [Test]
    public void CanAdd_WhenTotalSumIsReached_ReturnsFalse()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            totalSum = 10,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 }
            }
        };
        HeyTeaObjectSO heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsFalse(result);
    }

    /**
     * @brief [TC0305] Test case to validate if CheckMixed returns true when canMixed is true and all current numbers equal max number.
     */
    [Test]
    public void CheckMixed_WhenCanMixedIsTrueAndAllCurrentNumsEqualMaxNum_ReturnsTrue()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            canMixed = true,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 }
            }
        };

        // Act
        bool result = quota.CheckMixed();

        // Assert
        Assert.IsTrue(result);
    }

    /**
     * @brief [TC0306] Test case to validate if CheckMixed returns false when canMixed is true and not all current numbers equal max number.
     */
    [Test]
    public void CheckMixed_WhenCanMixedIsTrueAndNotAllCurrentNumsEqualMaxNum_ReturnsFalse()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            canMixed = true,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 4, maxNum = 5 }
            }
        };

        // Act
        bool result = quota.CheckMixed();

        // Assert
        Assert.IsFalse(result);
    }

    /**
     * @brief [TC0307] Test case to validate if CheckMixed returns false when canMixed is false.
     */
    [Test]
    public void CheckMixed_WhenCanMixedIsFalse_ReturnsFalse()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            canMixed = false,
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 },
                new HeyTeaObejctStruct { currentNum = 5, maxNum = 5 }
            }
        };

        // Act
        bool result = quota.CheckMixed();

        // Assert
        Assert.IsFalse(result);
    }

    /**
     * @brief [TC0308] Test case to validate if ClearAll clears all HeyTeaObjects and resets current numbers to zero.
     */
    [Test]
    public void ClearAll_ClearsAllHeyTeaObjectsAndResetsCurrentNumbersToZero()
    {
        // Arrange
        MilkTeaMaterialQuota quota = new MilkTeaMaterialQuota
        {
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { heyTeaObject = new HeyTeaObject(), currentNum = 3 },
                new HeyTeaObejctStruct { heyTeaObject = new HeyTeaObject(), currentNum = 2 }
            }
        };

        // Act
        quota.ClearAll();

        // Assert
        foreach (var item in quota.heyTeaObejctStructArray)
        {
            Assert.IsNull(item.heyTeaObject);
            Assert.AreEqual(0, item.currentNum);
        }
    }

    /**
     * @brief [TC0309] Test case to validate if AddHeyTeaObject adds the HeyTeaObject and updates the current number when HeyTeaObjectSO exists in the array.
     */
    [Test]
    public void AddHeyTeaObject_AddsHeyTeaObjectAndUpdateCurrentNum_WhenHeyTeaObjectSOExistsInArray()
    {
        // Arrange
        var heyTeaObjectSO = new HeyTeaObjectSO();
        var heyTeaObject = new HeyTeaObject();
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        var quota = new MilkTeaMaterialQuota
        {
            heyTeaObejctStructArray = new HeyTeaObejctStruct[]
            {
                new HeyTeaObejctStruct { heyTeaObjectSO = heyTeaObjectSO, currentNum = 2 }
            }
        };

        // Act
        bool result = quota.AddHeyTeaObject(heyTeaObject);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3, quota.heyTeaObejctStructArray[0].currentNum);
        Assert.AreEqual(heyTeaObject, quota.heyTeaObejctStructArray[0].heyTeaObject);
    }
}
