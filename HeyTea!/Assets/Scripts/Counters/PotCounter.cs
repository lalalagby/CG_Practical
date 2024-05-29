using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IKichenwareObejct;

/*
 * Glossary
 * 
 * HeyTeaObject: teaBase, basicAdd, ingredients, fruit, none, un treat
 *      - teaBase: tea, milk, milktea
 *      - basicAdd: sugar, ice
 *      - ingredients: red bean cooked, pearl cooked
 *      - fruit: orange slice, grape slice, strawberry slice
 *      - none: pot, cup
 *      - un treat: bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry, 
 */

public class PotCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public bool isCooking;
    }

    //[SerializeField] private HeyTeaObjectSO heyTeaObjectSO;
    [SerializeField] private List<CookingRecipeSO> cookingRecipeSOList = new List<CookingRecipeSO>();
    [SerializeField] private List<MilkTeaMaterialQuota> milkTeaMaterialQuotaList;
    [SerializeField] private List<HeyTeaObjectTransform> heyTeaObjectTransformList;
    [SerializeField] private int totalInpotNum;

    private bool isCooking;
    private float setInterval = 0.2f;
    private float allTime;
    private float currentProgressTime;

    private void Start()
    {
        //HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, this);
        isCooking = false;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
    }

    private void Update()
    {
        if (isCooking && HasHeyTeaObject())
        {
            allTime += Time.deltaTime;
            if (AddCurrentCookingProgress(Time.deltaTime))
            {
                isCooking = false;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
            }
            if (allTime >= setInterval || !isCooking)
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = GetCookingProgressPercentage(),
                    isProcessing = isCooking
                });
                allTime = 0;
            }
        }
    }

    public override void Interact(Player player) {
        // If Player has HeyTeaObject
        if (player.HasHeyTeaObject()) {
            HeyTeaObject playerObject = player.GetHeyTeaObject();
            // Try to add HeyTeaObjet into Pot
            if (TryAddIngredient(playerObject.GetHeyTeaObjectSO(), (MilkTeaMaterialType)playerObject.GetHeyTeaObjectSO().materialType)) {
                player.GetHeyTeaObject().DestroySelf();
                isCooking = false;
            }
        } else if (HasHeyTeaObject()) {     // counter has object but player does not.
            print("counter has object but player does not.");
            GetHeyTeaObject().SetHeyTeaObjectParents(player);
        }
    }

    public override void Operation(Player player) {
        print("HasHeyTeaObject: " + HasHeyTeaObject());
        print("CanCook: " + CanCook());
        if (HasHeyTeaObject() && CanCook()) {
            isCooking = !isCooking;
            allTime = 0;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = isCooking });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
        }
    }
    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        return cookingRecipeSO != null;
    }

    private bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        if (CheckTotalInpotNum() >= totalInpotNum) {
            return false;
        }
        
        foreach (var quota in milkTeaMaterialQuotaList) {
            if (milkTeaMaterialType == quota.milkTeaMaterialType) {
                if (HasRecipeWithInput(heyTeaObjectSO)) {

                    HeyTeaObject heyTeaObject = SpawnHeyTeaObject(GetMidStateForInput(heyTeaObjectSO), milkTeaMaterialType);
                    if (heyTeaObject == null) {
                        Debug.Log("Create object failed");
                        return false;
                    }
                    if (quota.AddHeyTeaObject(heyTeaObject)) {
                        print(heyTeaObject);
                        foreach (var transform in heyTeaObjectTransformList) {
                            transform.ResetLayer(GetLayer());
                        }
                        Debug.Log("add success");
                        currentProgressTime = 0;
                        return true;
                    }
                }
            }
        }
        Debug.Log("add failed");
        return false;
    }

    private HeyTeaObjectSO GetMidStateForInput(HeyTeaObjectSO heyTeaObjectSO) {
        return GetCookingRecipeSOWithInput(heyTeaObjectSO).midState;
    }

    private int CheckTotalInpotNum()
    {
        int sum = 0;
        foreach (var quota in milkTeaMaterialQuotaList)
        {
            foreach (var heyTeaObejctStruct in quota.heyTeaObejctStructArray)
            {
                sum += heyTeaObejctStruct.currentNum;
            }
        }
        return sum;
    }

    private int GetLayer()
    {
        int layer = 0;
        foreach (var quota in milkTeaMaterialQuotaList)
        {
            if (quota.canMixed)
            {
                for (int i = 0; i < quota.heyTeaObejctStructArray.Count(); i++)
                {
                    if (quota.heyTeaObejctStructArray[i].currentNum > 0)
                    {
                        layer = i + 1;
                    }
                }
            }
        }
        return layer;
    }

    public bool CanCook()
    {
        if (CheckTotalInpotNum() == 0)
        {
            return false;
        }
        foreach (var quota in milkTeaMaterialQuotaList)
        {
            if (quota.milkTeaMaterialType == IKichenwareObejct.MilkTeaMaterialType.unTreat)
            {
                foreach (var heyTeaObejctStruct in quota.heyTeaObejctStructArray)
                {
                    if (heyTeaObejctStruct.currentNum > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool AddCurrentCookingProgress(float time)
    {
        currentProgressTime += time;
        if (CheckIsFinish())
        {
            HeyTeaObjectSO output = null;
            foreach (var quota in milkTeaMaterialQuotaList)
            {
                foreach (var heyTeaObejctStruct in quota.heyTeaObejctStructArray)
                {
                    if (heyTeaObejctStruct.currentNum > 0)
                    {
                        output = GetCookingRecipeSOWithInput(heyTeaObejctStruct.heyTeaObjectSO).output;
                    }
                }
            }
            this.DestroySelf();
            foreach (var quota in milkTeaMaterialQuotaList)
            {
                quota.ClearAll();
            }

            if (CheckTotalInpotNum() >= totalInpotNum)
            {
                return false;
            }
            foreach (var quota in milkTeaMaterialQuotaList)
            {
                if ((IKichenwareObejct.MilkTeaMaterialType)output.materialType == quota.milkTeaMaterialType)
                {
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObject(output, quota.milkTeaMaterialType);
                    if (heyTeaObject == null)
                    {
                        Debug.Log("Create object failed");
                        return false;
                    }
                    if (quota.AddHeyTeaObject(heyTeaObject))
                    {
                        foreach (var transform in heyTeaObjectTransformList)
                        {
                            transform.ResetLayer(GetLayer());
                        }
                        Debug.Log("add success");
                        currentProgressTime = 0;
                        return true;
                    }
                }
            }
            Debug.Log("add failed");
            return false;
        }
        return false;
    }

    public float GetCookingProgressPercentage()
    {
        return currentProgressTime / GetCookingTimeMax();
    }

    private bool CheckIsFinish()
    {
        return currentProgressTime >= GetCookingTimeMax();
    }

    private float GetCookingTimeMax()
    {
        foreach (var quota in milkTeaMaterialQuotaList)
        {
            foreach (var heyTeaObejctStruct in quota.heyTeaObejctStructArray)
            {
                if (heyTeaObejctStruct.currentNum > 0)
                {
                    var recipe = GetCookingRecipeSOWithInput(heyTeaObejctStruct.heyTeaObjectSO);
                    if (recipe != null)
                    {
                        return recipe.cookingTimerMax;
                    }
                }
            }
        }
        return 0;
    }

    private CookingRecipeSO GetCookingRecipeSOWithInput(HeyTeaObjectSO input) {
        foreach (var recipe in cookingRecipeSOList)  {
            if (recipe.input == input || recipe.midState == input) {
                return recipe;
            }
        }
        return null;
    }
    
    public HeyTeaObject SpawnHeyTeaObject(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        Transform transform = Instantiate(heyTeaObjectSO.prefab);
        HeyTeaObject heyTeaObject = transform.GetComponent<HeyTeaObject>();

        for (int i = 0; i < heyTeaObjectTransformList.Count(); i++) {
            if (heyTeaObjectTransformList[i].CanSetParent(milkTeaMaterialType)) {
                heyTeaObjectTransformList[i].SetTransform(heyTeaObject.transform);
                return heyTeaObject;
            }
        }

        return null;
    }

    public void DestroySelf()
    {
        foreach (MilkTeaMaterialQuota quota in milkTeaMaterialQuotaList)
        {
            foreach (HeyTeaObejctStruct heyTeaObjectStruct in quota.heyTeaObejctStructArray)
            {
                if (heyTeaObjectStruct.heyTeaObject != null)
                {
                    DestroyImmediate(heyTeaObjectStruct.heyTeaObject.gameObject);
                }
            }
        }
    }
}
