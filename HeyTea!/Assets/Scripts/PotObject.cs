using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PotObject : HeyTeaObject,IKichenwareObejct,IHeyTeaObjectParents
{
    [SerializeField] private List<CookingRecipeSO> cookingRecipeSOList = new List<CookingRecipeSO>();
    [SerializeField] private Transform counterTopPoint;

    private float currentProgressTime;

    private HeyTeaObject inPotHeyTeaObject;
    

    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, IKichenwareObejct.MilkTeaMaterialType milkTeaMaterialType) {
        if (HasRecipeWithInput(heyTeaObjectSO)) {
            SpawnHeyTeaObejct(GetMidStateForInput(heyTeaObjectSO), this);
            currentProgressTime = 0;
            return true;
        } else {
            return false;
        }
    }

    public HeyTeaObjectSO GetOutputForInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        if (cookingRecipeSO != null&& CheckIsFinish()) {
            currentProgressTime = 0;
            return cookingRecipeSO.output;
        } else {
            return null;
        }
    }

    public bool GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO) {
        if (inPotHeyTeaObject!=null&& CheckIsFinish()) {
            heyTeaObjectSO = inPotHeyTeaObject.GetHeyTeaObjectSO();
            return true;
        } else {
            heyTeaObjectSO = null;
            return false;
        }
    }

    public bool AddCurrentCookingProgress(float time) {
        currentProgressTime += time;
        if (CheckIsFinish()) {
            HeyTeaObjectSO heyTeaObjectSOClone =( HeyTeaObjectSO) GetOutputForInput(inPotHeyTeaObject.GetHeyTeaObjectSO()).Clone();
            this.DestroySelf();
            SpawnHeyTeaObejct(heyTeaObjectSOClone, this);
            return true;
        } else {
            return false;
        }
        
    }

    public float GetCookingProgressPercentage() {
        return currentProgressTime/ GetCookingTimeMax(inPotHeyTeaObject.GetHeyTeaObjectSO());
    }



    public new void DestroySelf() {
        Destroy(inPotHeyTeaObject.gameObject);
        inPotHeyTeaObject = null;
    }

    private bool CheckIsFinish() {
        return currentProgressTime >= GetCookingTimeMax(inPotHeyTeaObject.GetHeyTeaObjectSO());
    }

    public HeyTeaObjectSO GetMidStateForInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        if (cookingRecipeSO != null) {
            return cookingRecipeSO.midState;
        } else {
            return null;
        }
    }

    private float GetCookingTimeMax(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        if (cookingRecipeSO != null) {
            return cookingRecipeSO.cookingTimerMax;
        } else {
            return 0f;
        }
    }

    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        return cookingRecipeSO != null;
    }

    

    private CookingRecipeSO GetCookingRecipeSOWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOList) {
            if (cookingRecipeSO.input == inputHeyTeaObjectSO| cookingRecipeSO.midState==inputHeyTeaObjectSO) {
                return cookingRecipeSO;
            }
        }
        return null;
    }

    public Transform GetHeyTeaObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetHeyTeaObject(HeyTeaObject heyTeaObject) {
        this.inPotHeyTeaObject = heyTeaObject;
    }

    public HeyTeaObject GetHeyTeaObject() {
        return this.inPotHeyTeaObject;
    }

    public void ClearHeyTeaObject() {
        this.inPotHeyTeaObject = null;
    }

    public bool HasHeyTeaObject() {
        return inPotHeyTeaObject != null;
    }
}
