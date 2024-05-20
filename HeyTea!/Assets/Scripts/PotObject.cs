using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static HeyTeaObjectSO;
using static IKichenwareObejct;
using MilkTeaMaterialType = IKichenwareObejct.MilkTeaMaterialType;

public class PotObject : HeyTeaObject,IKichenwareObejct
{
    [SerializeField] private List<CookingRecipeSO> cookingRecipeSOList = new List<CookingRecipeSO>();
    [SerializeField] private List<MilkTeaMaterialQuota> milkTeaMaterialQuotaList;
    [SerializeField] private List<HeyTeaObjectTransform> heyTeaObjectTransformList;
    [SerializeField] private int totalInpotNum;

    private float currentProgressTime;

    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        if (CheckTotalInpotNum() >= totalInpotNum) {
            return false;
        }
        for (int i = 0; i < milkTeaMaterialQuotaList.Count(); i++) {
            if (milkTeaMaterialType == milkTeaMaterialQuotaList[i].milkTeaMaterialType) {
                if (HasRecipeWithInput(heyTeaObjectSO)) {
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObejct(GetMidStateForInput(heyTeaObjectSO), milkTeaMaterialType);
                    if (heyTeaObject == null) {
                        Debug.Log("Create object failed");
                        return false;
                    }
                    if (milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                        for (int j = 0; j < heyTeaObjectTransformList.Count; j++) {
                            heyTeaObjectTransformList[j].ResetLayer(GetLayer());
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

    private int CheckTotalInpotNum() {
        int sum = 0;
        foreach(MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
            foreach(HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray) {
                sum += heyTeaObejctStruct.currentNum;
            }
        }
        return sum;
    }

    private int GetLayer() {
        int layer = 0;
        foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
            if (milkTeaMaterialQuota.canMixed) {
                for (int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count(); i++) {
                    if (milkTeaMaterialQuota.heyTeaObejctStructArray[i].currentNum > 0) {
                        layer = i + 1;
                    }
                }
            }
        }
        return layer;
    }

    public bool CanCook() {
        if (CheckTotalInpotNum() == 0) {
            return false;
        } else {
            int sum = 0;
            foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
                if (milkTeaMaterialQuota.milkTeaMaterialType == MilkTeaMaterialType.unTreat) {
                    foreach (HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray) {
                        sum += heyTeaObejctStruct.currentNum;
                    }
                }
                if (sum>0) {
                    return true;
                } 
            }
            return false;
        }
    }

    public HeyTeaObject SpawnHeyTeaObejct(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
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

    // Get HeyTeaObjectSO from this(Pot)
    public bool GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO) {
        
        foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
            foreach (HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray) {
                if (heyTeaObejctStruct.currentNum > 0) {
                    heyTeaObjectSO = heyTeaObejctStruct.heyTeaObjectSO;
                    return true;
                }
            }
        }
        heyTeaObjectSO = null;
        return false;
    }

    public bool AddCurrentCookingProgress(float time) {
        currentProgressTime += time;
        if (CheckIsFinish()) {
            HeyTeaObjectSO heyTeaObjectSOClone = null;
            foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
                foreach (HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray) {
                    if (heyTeaObejctStruct.currentNum > 0) {
                        heyTeaObjectSOClone= GetCookingRecipeSOWithInput(heyTeaObejctStruct.heyTeaObjectSO).output;
                    }
                }
            }
            this.DestroySelf();
            for(int i = 0; i < milkTeaMaterialQuotaList.Count; i++) {
                milkTeaMaterialQuotaList[i].ClearAll();
            }

            if (CheckTotalInpotNum() >= totalInpotNum) {
                return false;
            }
            for (int i = 0; i < milkTeaMaterialQuotaList.Count(); i++) {
                if ((MilkTeaMaterialType)heyTeaObjectSOClone.materialType == milkTeaMaterialQuotaList[i].milkTeaMaterialType) {
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObejct(heyTeaObjectSOClone, milkTeaMaterialQuotaList[i].milkTeaMaterialType);
                    if (heyTeaObject == null) {
                        Debug.Log("Create object failed");
                        return false;
                    }
                    if (milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                        for (int j = 0; j < heyTeaObjectTransformList.Count; j++) {
                            heyTeaObjectTransformList[j].ResetLayer(GetLayer());
                        }
                        Debug.Log("add success");
                        currentProgressTime = 0;
                        return true;
                    }
                }
            }
            Debug.Log("add failed");
            return false;
        } else {
            return false;
        }
    }

    public float GetCookingProgressPercentage() {
        return currentProgressTime / GetCookingTimeMax();
    }



    public new void DestroySelf() {
        for(int j = 0; j < milkTeaMaterialQuotaList.Count; j++) {
            for (int i = 0; i < milkTeaMaterialQuotaList[j].heyTeaObejctStructArray.Count(); i++) {
                if (milkTeaMaterialQuotaList[j].heyTeaObejctStructArray[i].heyTeaObject != null) {
                    DestroyImmediate(milkTeaMaterialQuotaList[j].heyTeaObejctStructArray[i].heyTeaObject.gameObject);
                }
            }
        }
    }

    private bool CheckIsFinish() {
        float cooktime = GetCookingTimeMax();
        return currentProgressTime >= GetCookingTimeMax();
    }

    private float GetCookingTimeMax() {
        foreach(MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList) {
            foreach(HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray) {
                if (heyTeaObejctStruct.currentNum > 0) {
                    if (GetCookingRecipeSOWithInput(heyTeaObejctStruct.heyTeaObjectSO) != null) {
                        return GetCookingRecipeSOWithInput(heyTeaObejctStruct.heyTeaObjectSO).cookingTimerMax;
                    } else {
                        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOList) {
                            if (cookingRecipeSO.output == heyTeaObejctStruct.heyTeaObjectSO ) {
                                return cookingRecipeSO.cookingTimerMax;
                            }
                        }
                    }
                }
            }
        }
        return 0;
    }

    public void DestroyChild(HeyTeaObjectSO heyTeaObjectSO) {
        if (heyTeaObjectSO == null) {
            return;
        }
        for (int i = 0; i < milkTeaMaterialQuotaList.Count; i++) {
            for (int j = 0; j < milkTeaMaterialQuotaList[i].heyTeaObejctStructArray.Count(); j++) {
                if (milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObjectSO == heyTeaObjectSO && milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject != null) {
                    DestroyImmediate(milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject.transform.gameObject);
                    milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject = null;
                    milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].currentNum = 0;
                }
            }
        }
    }

    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        return cookingRecipeSO != null;
    }


    private CookingRecipeSO GetCookingRecipeSOWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOList) {
            if (cookingRecipeSO.input == inputHeyTeaObjectSO | cookingRecipeSO.midState == inputHeyTeaObjectSO) {
                return cookingRecipeSO;
            }
        }
        return null;
    }

    public List<MilkTeaMaterialQuota> GetMilkTeaMaterialQuota()  {
        return milkTeaMaterialQuotaList;
    }
}
