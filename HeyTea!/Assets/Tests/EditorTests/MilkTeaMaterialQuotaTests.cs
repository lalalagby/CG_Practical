using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;

public class MilkTeaMaterialQuotaTests
{
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
                new HeyTeaObejctStruct { currentNum = 4, maxNum = 5 }
            }
        };
        HeyTeaObjectSO heyTeaObjectSO = new HeyTeaObjectSO(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsFalse(result);
    }

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
        HeyTeaObjectSO heyTeaObjectSO = new HeyTeaObjectSO(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsTrue(result);
    }

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
        HeyTeaObjectSO heyTeaObjectSO = new HeyTeaObjectSO(); // Provide necessary setup for HeyTeaObjectSO

        // Act
        bool result = quota.CanAdd(heyTeaObjectSO);

        // Assert
        Assert.IsFalse(result);
    }

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
