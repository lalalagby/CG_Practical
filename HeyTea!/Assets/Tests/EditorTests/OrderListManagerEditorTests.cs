using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;


public class OrderListManagerEditorTests{
    public static OrderListManager Instance { get; private set; }
    private OrderListManager orderListManager;
    private CupObject cupObject;
    private RecipeListSO recipeListSO;

    [SetUp]
    public void Setup() {
    // ����OrderListManager����
        orderListManager = new GameObject().AddComponent<OrderListManager>();

        // ����RecipeListSO
        recipeListSO = ScriptableObject.CreateInstance<RecipeListSO>();
        recipeListSO.recipeSOList = new List<RecipeSO>();

        // ����CupObject����
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);
        cupObject = cupInstance.GetComponent<CupObject>();

        // ����OrderListManager��RecipeListSO
        orderListManager.SetRecipeListSO(recipeListSO);
    }

    // ����һ�������Ķ���
    private RecipeSO CreateCorrectRecipeSO(int type) {
        HeyTeaObjectSO heyTeaObjectSO;
        RecipeSO recipeSO = ScriptableObject.CreateInstance<RecipeSO>();
        List<HeyTeaObjectSO> heyTeaObjectSOList = new List<HeyTeaObjectSO>();
        if (type == 1) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Sugar.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        } else if(type == 2){
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        } else {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        }
        
        recipeSO.heyTeaObjectSOLists = heyTeaObjectSOList;
        return recipeSO;
    }

    private CupObject CreateCorrectRecipe(int type) {
        HeyTeaObjectSO heyTeaObjectSO;
        if (type == 1) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Sugar.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else if (type == 2) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }

        return cupObject;
    }
    private CupObject CreateWrongRecipe(int type) {
        HeyTeaObjectSO heyTeaObjectSO;

        if (type == 1) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else if (type == 2) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }

        return cupObject;
    }
    
    
    // �ύ����Ķ����������б�û�ж������Ƴ���
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void DeliverIncorrectRecipe_OrderListUnchanged(int type) {
        // ����Waiting����
        RecipeSO correctRecipe1 = CreateCorrectRecipeSO(1);
        RecipeSO correctRecipe2 = CreateCorrectRecipeSO(2);
        RecipeSO correctRecipe3 = CreateCorrectRecipeSO(3);
        recipeListSO.recipeSOList.Add(correctRecipe1);
        recipeListSO.recipeSOList.Add(correctRecipe2);
        recipeListSO.recipeSOList.Add(correctRecipe3);
        orderListManager.SetWaitingRecipeSOList(new List<RecipeSO>());
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe3);
        orderListManager.SetOrdersGenerated();

        // �����ύ�Ķ���
        cupObject = CreateWrongRecipe(type);

        // ��ȡ��ǰ��waiting������
        int waitingRecipeNum = orderListManager.GetWaitingRecipeNum();
        // ��ʼ���ɶ�����
        int ordersGeneratedNum = orderListManager.GetOrdersGenerated();

        // ��������
        orderListManager.DeliverOrder(cupObject);

        // Assert
        Assert.AreEqual(waitingRecipeNum, orderListManager.GetWaitingRecipeNum());
        // ���ɶ���������
        Assert.AreEqual(orderListManager.GetOrdersGenerated(), ordersGeneratedNum);
    }


    // �ύ��ȷ�Ķ��������ҵ�ǰtotalNumOrdersС�����ֵ-1���ö����Ӷ����б���ɾ��������3��������µĶ�����
    [Test]
    public void DeliverCorrectRecipe_RemoveOrderAndGenerateNewOrder() {
        // ����Waiting����
        RecipeSO correctRecipe1 = CreateCorrectRecipeSO(1);
        RecipeSO correctRecipe2 = CreateCorrectRecipeSO(2);
        RecipeSO correctRecipe3 = CreateCorrectRecipeSO(3);
        recipeListSO.recipeSOList.Add(correctRecipe1);
        recipeListSO.recipeSOList.Add(correctRecipe2);
        recipeListSO.recipeSOList.Add(correctRecipe3);
        orderListManager.SetWaitingRecipeSOList(new List<RecipeSO>());
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe3);
        orderListManager.SetOrdersGenerated();

        // �����ύ�Ķ���
        cupObject = CreateCorrectRecipe(2);
        // ��ȡ��ǰ��waiting������
        int waitingRecipeNum = orderListManager.GetWaitingRecipeNum();
        // ��ʼ���ɶ�����
        int ordersGeneratedNum = orderListManager.GetOrdersGenerated();

        // ��������
        orderListManager.DeliverOrder(cupObject);

        // ��ʱ������ɾ�������ҵ�ǰ���ɵĶ�����С�����ֵ-1
        Assert.Less(orderListManager.GetWaitingRecipeNum(), 4);
        // ���ɶ���+1
        Assert.AreEqual(orderListManager.GetOrdersGenerated(), ordersGeneratedNum + 1);
        // waiting��������������
        Assert.AreEqual(waitingRecipeNum, orderListManager.GetWaitingRecipeNum());
    }


    // �ύ��ȷ�Ķ�����ɾ�������б�ĸö��������totalNumOrders������8�����ٲ����¶�����
    public void DeliverCorrectRecipe_RemoveOrderAndCannotGenerateNewOrder() {
        // ����Waiting����
        RecipeSO correctRecipe1 = CreateCorrectRecipeSO(1);
        RecipeSO correctRecipe2 = CreateCorrectRecipeSO(2);
        RecipeSO correctRecipe3 = CreateCorrectRecipeSO(3);
        recipeListSO.recipeSOList.Add(correctRecipe1);
        recipeListSO.recipeSOList.Add(correctRecipe2);
        recipeListSO.recipeSOList.Add(correctRecipe3);
        orderListManager.SetWaitingRecipeSOList(new List<RecipeSO>());
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe3);
        orderListManager.SetOrdersGenerated();

        // �����ύ�Ķ���
        cupObject = CreateCorrectRecipe(1);

        // ��ȡ��ǰ��waiting������
        int waitingRecipeNum = orderListManager.GetWaitingRecipeNum();
        // ��ʼ���ɶ�����
        int ordersGeneratedNum = orderListManager.GetOrdersGenerated();

        // ��������
        orderListManager.DeliverOrder(cupObject);

        // ��ʱ������ɾ�������ҵ�ǰ�Ѿ����������ֵ-1��������
        Assert.IsFalse(orderListManager.GetWaitingRecipeSOList().Contains(correctRecipe1));
        Assert.Less(orderListManager.GetWaitingRecipeNum(), 4);

        // ��ʱwaiting�Ķ�����������1.
        Assert.AreEqual(waitingRecipeNum-1, orderListManager.GetWaitingRecipeNum());
        // ���ɶ�������
        Assert.AreEqual(orderListManager.GetOrdersGenerated(), ordersGeneratedNum);
    }


}
