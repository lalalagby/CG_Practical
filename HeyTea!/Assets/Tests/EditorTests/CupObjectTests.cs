using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static HeyTeaObjectSO;
using static IKichenwareObejct;
using MilkTeaMaterialType = IKichenwareObejct.MilkTeaMaterialType;

public class CupObjectTests
{
    [Test]
    public void TryAddIngredient_WhenMaterialQuotaCanAddIngredient_ShouldReturnTrue()
    {
        // Arrange
        CupObject cupObject = new GameObject().AddComponent<CupObject>(); // Create an instance of CupObject
        HeyTeaObjectSO heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>(); // Create a ScriptableObject instance
        MilkTeaMaterialType milkTeaMaterialType = MilkTeaMaterialType.teaBase; // Mock MilkTeaMaterialType
        cupObject.milkTeaMaterialQuotaList = new System.Collections.Generic.List<MilkTeaMaterialQuota>(); // Initialize list

        // Create a mock MilkTeaMaterialQuota that allows adding an ingredient
        MilkTeaMaterialQuota mockQuota = new MilkTeaMaterialQuota();
        mockQuota.milkTeaMaterialType = milkTeaMaterialType;
        mockQuota.canMixed = true; // Mocking mixing allowed
        mockQuota.heyTeaObejctStructArray = new HeyTeaObejctStruct[] { new HeyTeaObejctStruct() { heyTeaObjectSO = heyTeaObjectSO, maxNum = 2, currentNum = 1 } }; // Mock existing ingredient
        cupObject.milkTeaMaterialQuotaList.Add(mockQuota);

        // Act
        bool result = cupObject.TryAddIngredient(heyTeaObjectSO, milkTeaMaterialType);

        // Assert
        Assert.IsTrue(result);
    }
}
