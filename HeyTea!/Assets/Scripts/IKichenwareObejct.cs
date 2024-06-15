using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static IKichenwareObejct;

/**
 * @brief Interface for kitchenware objects in the HeyTea game.
 * @details This interface defines methods and structures for managing ingredients and interactions with kitchenware objects.
 * 
 * @date 28.08.2023
 */

/**
 * @interface IKichenwareObejct
 * @brief An interface for kitchenware objects.
 * 
 * This interface provides methods for adding ingredients, getting output objects, and interacting with other kitchenware objects.
 */
public interface IKichenwareObejct
{
    /**
     * @enum MilkTeaMaterialType
     * @brief Enum for the types of milk tea materials.
     * 
     * This enum defines the various types of materials that can be used in milk tea.
     */
    public enum MilkTeaMaterialType
    {
        none,       /**< No material */
        teaBase,    /**< Tea or milk tea base */
        basicAdd,   /**< Basic additions like sugar and ice */
        fruit,      /**< Fruit additions */
        ingredients,/**< Ingredients like red bean and pearl */
        unTreat,    /**< Untreated materials */
    }

    /**
     * @struct HeyTeaObejctStruct
     * @brief Structure representing a HeyTeaObject and its properties.
     * 
     * This structure holds information about a HeyTeaObject, including its associated ScriptableObject, the object itself, and its quantity limits.
     */
    [System.Serializable]
    public struct HeyTeaObejctStruct
    {
        public HeyTeaObjectSO heyTeaObjectSO;  /**< The ScriptableObject associated with this HeyTeaObject */
        public HeyTeaObject heyTeaObject;      /**< The HeyTeaObject instance */
        public int maxNum;                     /**< The maximum number of this object that can be held */
        public int currentNum;                 /**< The current number of this object that is held */
    }

    /**
     * @struct MilkTeaMaterialQuota
     * @brief Structure representing a quota for milk tea materials.
     * 
     * This structure manages the quotas for various milk tea materials, including their quantities and whether they can be mixed.
     */
    [System.Serializable]
    public struct MilkTeaMaterialQuota
    {
        public HeyTeaObejctStruct[] heyTeaObejctStructArray;  /**< Array of HeyTeaObjectStructs representing the materials */
        public int totalSum;                                  /**< The total allowable quantity of materials */
        public bool canMixed;                                 /**< Flag indicating if materials can be mixed */
        public MilkTeaMaterialType milkTeaMaterialType;       /**< The type of milk tea material */

        /**
         * @brief Checks if a HeyTeaObjectSO can be added.
         * @param heyTeaObjectSO The HeyTeaObjectSO to check.
         * @return True if the object can be added, otherwise false.
         */
        public bool CanAdd(HeyTeaObjectSO heyTeaObjectSO)
        {
            if (canMixed && heyTeaObejctStructArray[heyTeaObejctStructArray.Length - 1].currentNum >= heyTeaObejctStructArray[heyTeaObejctStructArray.Length - 1].maxNum)
            {
                return false;
            }

            foreach (HeyTeaObejctStruct heyTeaObejctStruct in heyTeaObejctStructArray)
            {
                if (heyTeaObjectSO == heyTeaObejctStruct.heyTeaObjectSO && heyTeaObejctStruct.currentNum >= heyTeaObejctStruct.maxNum)
                {
                    return false;
                }
            }

            int sum = 0;
            foreach (HeyTeaObejctStruct heyTeaObejctStruct in heyTeaObejctStructArray)
            {
                sum += heyTeaObejctStruct.currentNum;
            }

            if (sum >= totalSum)
            {
                return false;
            }

            return true;
        }

        /**
         * @brief Checks if the materials can be mixed.
         * @return True if the materials can be mixed, otherwise false.
         */
        public bool CheckMixed()
        {
            bool tag = true;
            if (canMixed)
            {
                for (int i = 0; i < heyTeaObejctStructArray.Count(); i++)
                {
                    if (heyTeaObejctStructArray[i].currentNum < heyTeaObejctStructArray[i].maxNum)
                    {
                        tag = false;
                    }
                }
            }
            else
            {
                tag = false;
            }
            return tag;
        }

        /**
         * @brief Clears all materials.
         */
        public void ClearAll()
        {
            for (int i = 0; i < heyTeaObejctStructArray.Count(); i++)
            {
                heyTeaObejctStructArray[i].heyTeaObject = null;
                heyTeaObejctStructArray[i].currentNum = 0;
            }
        }

        /**
         * @brief Adds a HeyTeaObject to the quota.
         * @param heyTeaObject The HeyTeaObject to add.
         * @return True if the object was added, otherwise false.
         */
        public bool AddHeyTeaObject(HeyTeaObject heyTeaObject)
        {
            for (int i = 0; i < heyTeaObejctStructArray.Count(); i++)
            {
                if (heyTeaObject.GetHeyTeaObjectSO() == heyTeaObejctStructArray[i].heyTeaObjectSO)
                {
                    heyTeaObejctStructArray[i].heyTeaObject = heyTeaObject;
                    heyTeaObejctStructArray[i].currentNum++;
                    return true;
                }
            }
            return false;
        }
    }

    /**
     * @struct HeyTeaObjectTransform
     * @brief Structure representing the transform properties for a HeyTeaObject.
     * 
     * This structure manages the transform properties (position, rotation, scale) for HeyTeaObjects.
     */
    [System.Serializable]
    public struct HeyTeaObjectTransform
    {
        public Transform parentTransform;  /**< The parent transform for this object */
        public Vector3[] postionBiasList;  /**< The list of position biases */
        public Vector3[] rotationBiasList; /**< The list of rotation biases */
        public Vector3[] scaleFactorList;  /**< The list of scale factors */
        public MilkTeaMaterialType milkTeaMaterialType; /**< The type of milk tea material */
        public int maxShow; /**< The maximum number of objects to show */
        public int maxStore; /**< The maximum number of objects to store */

        /**
         * @brief Sets the transform properties for a given transform.
         * @param transform The transform to set properties for.
         */
        public void SetTransform(Transform transform)
        {
            transform.parent = parentTransform;
            transform.localPosition = postionBiasList[0];
            transform.localEulerAngles = rotationBiasList[0];
            transform.localScale = scaleFactorList[0];

            if (parentTransform.childCount > maxShow)
            {
                transform.gameObject.SetActive(false);
                Debug.Log(transform.name + " is set " + false);
            }
            if (transform.childCount > 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject child = transform.GetChild(i).gameObject;
                    bool tag = child.activeSelf ? false : true;
                    child.gameObject.SetActive(tag);
                    Debug.Log(child.name + " is set " + tag);
                }
            }
        }

        /**
         * @brief Gets the milk tea material type.
         * @return The milk tea material type.
         */
        public MilkTeaMaterialType GetMilkTeaMaterialType()
        {
            return milkTeaMaterialType;
        }

        /**
         * @brief Resets the layer of the children transforms.
         * @param layer The layer to set the children to.
         */
        public void ResetLayer(int layer)
        {
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                child.localPosition = postionBiasList[layer];
                child.localEulerAngles = rotationBiasList[layer];
                child.localScale = scaleFactorList[layer];
                child.parent = parentTransform;
            }
        }

        /**
         * @brief Gets the parent transform.
         * @return The parent transform.
         */
        public Transform GetParentTransform()
        {
            return parentTransform;
        }

        /**
         * @brief Checks if the parent can be set for a given milk tea material type.
         * @param milkTeaMaterialType The milk tea material type to check.
         * @return True if the parent can be set, otherwise false.
         */
        public bool CanSetParent(MilkTeaMaterialType milkTeaMaterialType)
        {
            if (this.milkTeaMaterialType == milkTeaMaterialType && parentTransform.childCount < maxStore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /**
     * @brief Tries to add an ingredient to the kitchenware object.
     * @param heyTeaObjectSO The HeyTeaObjectSO representing the ingredient.
     * @param milkTeaMaterialType The type of milk tea material.
     * @return True if the ingredient was added, otherwise false.
     */
    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType);

    /**
     * @brief Gets the output HeyTeaObjectSO.
     * @param heyTeaObjectSO The output HeyTeaObjectSO.
     * @return True if an output object was obtained, otherwise false.
     */
    public bool GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO) { heyTeaObjectSO = null; return false; }

    /**
     * @brief Interacts with another kitchenware object to transfer HeyTeaObjects.
     * @param kichenwareObejct The other kitchenware object to interact with.
     * @return True if the interaction was successful, otherwise false.
     */
    public bool InteractWithOtherKichenware(IKichenwareObejct kichenwareObejct)
    {
        if (kichenwareObejct.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO))
        {
            return TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }
        else
        {
            return false;
        }
    }

    /**
     * @brief Destroys the child objects of a given HeyTeaObjectSO.
     * @param heyTeaObjectSO The HeyTeaObjectSO whose child objects need to be destroyed.
     */
    public void DestroyChild(HeyTeaObjectSO heyTeaObjectSO)
    {
        // Implementation to be provided
    }
}
