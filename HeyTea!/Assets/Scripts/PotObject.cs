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

/**
 * @author Yong Wu, Bingyu Guo
 * 
 * @brief Adding uncooked ingredients to the Pot.
 * 
 * @note
 *      milkTeaMaterialType contains six types: teaBase, basicAdd, ingredients, fruit, none, un treat.
 *      HeyTeaObject includes the objects contained in teaBase, basicAdd, ingredients, fruit, none, un treat.
 *          - teaBase: tea, milk, milktea
 *          - basicAdd: sugar, ice
 *          - ingredients: red bean cooked, pearl cooked
 *          - fruit: orange slice, grape slice, strawberry slice
 *          - none: pot, cup
 *          - un treat: bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry
 */

public class PotObject : HeyTeaObject,IKichenwareObejct
{
    [SerializeField] private List<CookingRecipeSO> cookingRecipeSOList = new List<CookingRecipeSO>();   
    [SerializeField] private List<MilkTeaMaterialQuota> milkTeaMaterialQuotaList;
    [SerializeField] private List<HeyTeaObjectTransform> heyTeaObjectTransformList;
    [SerializeField] private int totalInpotNum;     //!< The maximum number of ingredients that can be put into the Pot.

    private float currentProgressTime;

    /**
     * @brief   Try to add HeyTeaObject to the Pot.
     * 
     * @details This method checks if the ingredient can be added to the Pot based on the current number of ingredients, 
     *          the type of the ingredient, and if a recipe exists for the given ingredient.
     * 
     * @param heyTeaObjectSO The scriptable object representing the ingredient to be added.
     * @param milkTeaMaterialType The Type of this HeyTeaObject.
     * 
     * @return Returns \b true if the ingredient is successfully added; otherwise, returns \b false.
     * 
     * The function performs the following steps:
     * - Checks if the total number of ingredients in the Pot has reached the maximum allowed (`totalInpotNum`).
     * - Iterates through the list of milk tea material quotas to find a matching material type.
     * - Validates if a recipe exists for the given ingredient.
     * - Attempts to instantiate an intermediate state object for the ingredient.
     * - Adds the instantiated object to the appropriate quota.
     * - Resets the layers of all transform objects in `heyTeaObjectTransformList`.
     * - Logs the success or failure of the operation.
     * - Resets the current progress time to 0 on successful addition.
     */
    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType) {
        // Check if the number of ingredients in the Pot is greater than or equal to the totalInpotNum.
        // If so, no more ingredients can be added to the Pot. 
        if (CheckTotalInpotNum() >= totalInpotNum) {
            return false;
        }

        // Iterates through the list of milk tea material quotas to find a matching material type.
        for (int i = 0; i < milkTeaMaterialQuotaList.Count(); i++) {
            if (milkTeaMaterialType == milkTeaMaterialQuotaList[i].milkTeaMaterialType) {
                // Check if a recipe exists for the given ingredient.
                if (HasRecipeWithInput(heyTeaObjectSO)) {
                    // Instantiate an intermediate state object for the ingredient.
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObejct(GetMidStateForInput(heyTeaObjectSO), milkTeaMaterialType);

                    // Check if heyTeaObject is instantiated successfully.
                    if (heyTeaObject == null) {
                        Debug.Log("Create object failed");
                        return false;
                    }

                    // Adds the instantiated object to the appropriate quota.
                    if (milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject)) {
                        // Resets the layers of all transform objects in heyTeaObjectTransformList.
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

    /**
     * @brief Calculates the total number of ingredients in the Pot.
     * 
     * @details This function iterates through all the milk tea material quotas and sums up the 
     *          number of ingredients currently present.
     * 
     * @return The total number of ingredients in the Pot.
     */
    public int CheckTotalInpotNum() {
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

    /**
     * @brief Check if the Pot can start cooking.
     * 
     * @details This function checks if there are any untreated ingredients in the Pot. 
     *          If there are untreated ingredients, the Pot can start cooking.
     * 
     * @return return \b true if the Pot can start cooking, \b false otherwise.
     */
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

    /**
     * @brief Spawns a HeyTeaObject based on the provided HeyTeaObjectSO and MilkTeaMaterialType.
     * 
     * @details This function instantiates a HeyTeaObject using the prefab from the HeyTeaObjectSO, 
     *          and sets its parent based on the milk tea material type.
     * 
     * @param heyTeaObjectSO The scriptable object representing the HeyTeaObject to be spawned.
     * @param milkTeaMaterialType The type of milk tea material.
     * 
     * @return The spawned \b heyTeaObject, or \b null if spawning failed.
     */
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

    /**
     * @brief Retrieves a HeyTeaObjectSO from the Pot.
     * 
     * @details This function iterates through all the milk tea material quotas and retrieves the first 
     *          HeyTeaObjectSO that has a non-zero current number.
     * 
     * @param heyTeaObjectSO The output parameter that will hold the retrieved HeyTeaObjectSO.
     * 
     * @return return \b true if a HeyTeaObjectSO is found, \b false otherwise.
     */
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

    /**
     * @brief Adds cooking progress time to the Pot.
     * 
     * @details This function increments the current cooking progress time by the specified amount. 
     *          If the cooking process is finished, it clears the Pot and respawns the output HeyTeaObject.
     * 
     * @param time The amount of time to add to the current cooking progress.
     * 
     * @return return \b true if the cooking progress is successfully added and processed, \b false otherwise.
     */
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

    /**
     * @brief Gets the cooking progress as a percentage.
     * 
     * @details This function calculates the current cooking progress as a percentage of the total cooking time.
     * 
     * @return The cooking progress percentage.
     */
    public float GetCookingProgressPercentage() {
        return currentProgressTime / GetCookingTimeMax();
    }

    /**
     * @brief Destroys the Pot and all its contents.
     * 
     * @details This function iterates through all the milk tea material quotas and destroys all HeyTeaObjects in each quota.
     */
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

    /**
     * @brief Destroys a specific child HeyTeaObject in the Pot.
     * 
     * @details This function finds and destroys the HeyTeaObject corresponding to the given HeyTeaObjectSO.
     * 
     * @param heyTeaObjectSO The scriptable object representing the HeyTeaObject to be destroyed.
     */
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
