using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IKichenwareObejct;

/**
 * @brief Represents a cup object that can hold ingredients and manage their visual and logical representation.
 * @details This class extends HeyTeaObject and implements IKichenwareObejct. It manages adding ingredients,
 * combining them, and updating the visual representation of the cup's contents.
 * 
 * @date 05.07.2024
 * @author Yong Wu, Xinyue Cheng
 */

/**
 * @class CupObject
 * @brief A class for handling cup objects in the game.
 * 
 * This class allows for adding ingredients to the cup, managing their layers, and updating the visual representation.
 * It also handles events for when ingredients are added.
 */
[System.Serializable]
public class CupObject : HeyTeaObject, IKichenwareObejct
{
    /**
     * @brief Event triggered when an ingredient is added to the cup.
     */
    public event EventHandler OnIngredinetAdded;

    /**
     * @brief Array of adding recipes for the cup.
     */
    [SerializeField] public List<AddingRecipeSO> AddingRecipeSOArray;

    /**
     * @brief List of milk tea material quotas for the cup.
     */
    [SerializeField] public List<MilkTeaMaterialQuota> milkTeaMaterialQuotaList;

    /**
     * @brief List of transforms for positioning the objects within the cup.
     */
    [SerializeField] private List<HeyTeaObjectTransform> heyTeaObjectTransformList;

    /**
     * @brief Tries to add an ingredient to the cup.
     * @param heyTeaObjectSO The HeyTeaObjectSO representing the ingredient to be added.
     * @param milkTeaMaterialType The type of milk tea material.
     * @return True if the ingredient was successfully added, otherwise false.
     */
    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType)
    {
        for (int i = 0; i < milkTeaMaterialQuotaList.Count(); i++)
        {
            if (milkTeaMaterialType == milkTeaMaterialQuotaList[i].milkTeaMaterialType)
            {
                if (milkTeaMaterialQuotaList[i].CanAdd(heyTeaObjectSO))
                {
                    if (HasRecipeWithInput(heyTeaObjectSO))
                    {
                        heyTeaObjectSO = GetOutputForInput(heyTeaObjectSO);
                    }
                    HeyTeaObject heyTeaObject = SpawnHeyTeaObejct(heyTeaObjectSO, milkTeaMaterialType);
                    if (heyTeaObject == null)
                    {
                        Debug.Log("Create object failed");
                        return false;
                    }
                    if (milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject))
                    {
                        if (milkTeaMaterialQuotaList[i].CheckMixed())
                        {
                            heyTeaObject = CombineObject(milkTeaMaterialQuotaList[i]);
                            if (!milkTeaMaterialQuotaList[i].AddHeyTeaObject(heyTeaObject))
                            {
                                Debug.LogError("Mixed Failed");
                            }
                        }

                        for (int j = 0; j < heyTeaObjectTransformList.Count; j++)
                        {
                            heyTeaObjectTransformList[j].ResetLayer(GetLayer());
                        }

                        OnIngredinetAdded?.Invoke(this, EventArgs.Empty);
                        Debug.Log("add success");
                        return true;
                    }
                }
            }
        }
        Debug.Log("add failed");
        return false;
    }

    /**
     * @brief Destroys the child objects of a given HeyTeaObjectSO.
     * @param heyTeaObjectSO The HeyTeaObjectSO whose child objects need to be destroyed.
     */
    public void DestroyChild(HeyTeaObjectSO heyTeaObjectSO)
    {
        if (heyTeaObjectSO == null)
        {
            return;
        }
        for (int i = 0; i < milkTeaMaterialQuotaList.Count; i++)
        {
            for (int j = 0; j < milkTeaMaterialQuotaList[i].heyTeaObejctStructArray.Count(); j++)
            {
                if (milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObjectSO == heyTeaObjectSO && milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject != null)
                {
                    DestroyImmediate(milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject.transform.gameObject);
                    milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].heyTeaObject = null;
                    milkTeaMaterialQuotaList[i].heyTeaObejctStructArray[j].currentNum = 0;
                }
            }
        }
    }

    /**
     * @brief Gets the current layer to set the child objects transform.
     * @details This method calculates the layer based on the current ingredients in the cup.
     * @return The current layer.
     */
    private int GetLayer()
    {
        int layer = 0;
        foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList)
        {
            if (milkTeaMaterialQuota.canMixed)
            {
                for (int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count(); i++)
                {
                    if (milkTeaMaterialQuota.heyTeaObejctStructArray[i].currentNum > 0)
                    {
                        layer = i + 1;
                    }
                }
            }
        }
        return layer;
    }

    /**
     * @brief Combines the objects in the given MilkTeaMaterialQuota to create a new object.
     * @param milkTeaMaterialQuota The MilkTeaMaterialQuota to combine.
     * @return The new HeyTeaObject created from the combination.
     */
    private HeyTeaObject CombineObject(MilkTeaMaterialQuota milkTeaMaterialQuota)
    {
        for (int i = 0; i < milkTeaMaterialQuota.heyTeaObejctStructArray.Count() - 1; i++)
        {
            DestroyImmediate(milkTeaMaterialQuota.heyTeaObejctStructArray[i].heyTeaObject.gameObject);
        }
        milkTeaMaterialQuota.ClearAll();

        return SpawnHeyTeaObejct(milkTeaMaterialQuota.heyTeaObejctStructArray[milkTeaMaterialQuota.heyTeaObejctStructArray.Count() - 1].heyTeaObjectSO, milkTeaMaterialQuota.milkTeaMaterialType);
    }

    /**
     * @brief Spawns a new HeyTeaObject and sets its parent transform.
     * @param heyTeaObjectSO The HeyTeaObjectSO to spawn.
     * @param milkTeaMaterialType The type of milk tea material.
     * @return The newly spawned HeyTeaObject.
     */
    public HeyTeaObject SpawnHeyTeaObejct(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType)
    {
        Transform transform = Instantiate(heyTeaObjectSO.prefab);

        HeyTeaObject heyTeaObject = transform.GetComponent<HeyTeaObject>();

        for (int i = 0; i < heyTeaObjectTransformList.Count(); i++)
        {
            if (heyTeaObjectTransformList[i].CanSetParent(milkTeaMaterialType))
            {
                heyTeaObjectTransformList[i].SetTransform(heyTeaObject.transform);
                return heyTeaObject;
            }
        }

        return null;
    }

    /**
     * @brief Gets the list of milk tea material quotas.
     * @return The list of milk tea material quotas.
     */
    public List<MilkTeaMaterialQuota> GetMilkTeaMaterialQuota()
    {
        return milkTeaMaterialQuotaList;
    }

    /**
     * @brief Gets the list of HeyTeaObjectSO from the cup.
     * @return The list of HeyTeaObjectSO in the cup.
     */
    public List<HeyTeaObjectSO> GetOutputHeyTeaObejctSOList()
    {
        List<HeyTeaObjectSO> heyTeaObjectSOList = new List<HeyTeaObjectSO>();

        foreach (MilkTeaMaterialQuota milkTeaMaterialQuota in milkTeaMaterialQuotaList)
        {
            foreach (HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray)
            {
                if (heyTeaObejctStruct.currentNum > 0)
                {
                    heyTeaObjectSOList.Add(heyTeaObejctStruct.heyTeaObjectSO);
                }
            }
        }
        return heyTeaObjectSOList;
    }

    /**
     * @brief Gets the list of HeyTeaObjectTransform from the cup.
     * @return The list of HeyTeaObjectTransform in the cup.
     */
    public List<HeyTeaObjectTransform> GetHeyTeaObjectTransformList()
    {
        return heyTeaObjectTransformList;
    }

    /**
     * @brief Checks if there is an adding recipe for the given input HeyTeaObjectSO.
     * @param inputHeyTeaObjectSO The input HeyTeaObjectSO to check.
     * @return True if a recipe exists, otherwise false.
     */
    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO)
    {
        AddingRecipeSO addingRecipeSO = GetAddingRecipeSOWithInput(inputHeyTeaObjectSO);
        return addingRecipeSO != null;
    }

    /**
     * @brief Gets the AddingRecipeSO for a given input HeyTeaObjectSO.
     * @param inputHeyTeaObjectSO The input HeyTeaObjectSO to check.
     * @return The AddingRecipeSO if a recipe exists, otherwise null.
     */
    private AddingRecipeSO GetAddingRecipeSOWithInput(HeyTeaObjectSO inputHeyTeaObjectSO)
    {
        foreach (AddingRecipeSO addingRecipeSO in AddingRecipeSOArray)
        {
            if (addingRecipeSO.input == inputHeyTeaObjectSO)
            {
                return addingRecipeSO;
            }
        }
        return null;
    }

    /**
     * @brief Gets the output HeyTeaObjectSO for a given input HeyTeaObjectSO.
     * @param heyTeaObjectSO The input HeyTeaObjectSO.
     * @return The output HeyTeaObjectSO if a recipe exists, otherwise null.
     */
    private HeyTeaObjectSO GetOutputForInput(HeyTeaObjectSO heyTeaObjectSO)
    {
        return GetAddingRecipeSOWithInput(heyTeaObjectSO).output;
    }
}
