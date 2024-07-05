using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Manages material categories and their interactions.
 * @details This class represents an object in the HeyTea game, managing its state, parent relationships, and interactions.
 * 
 * @date 28.08.2023
 * @author Yong Wu
 * @date Last Modified 14.05.2024
 * @last Modified by Bingyu Guo
 */

/**
 * @class HeyTeaObject
 * @brief Represents an object in the HeyTea game.
 * 
 * This class manages the state, parent relationships, and interactions of an object in the HeyTea game.
 */
public class HeyTeaObject : MonoBehaviour
{
    /**
     * @brief The ScriptableObject associated with this HeyTeaObject.
     */
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    /**
     * @brief The parent of this HeyTeaObject.
     */
    private IHeyTeaObjectParents heyTeaObjectParent;

    /**
     * @brief Sets the HeyTeaObjectSO for this object.
     * @param heyTeaObjectSO The ScriptableObject to be set.
     */
    public void SetHeyTeaObjectSO(HeyTeaObjectSO heyTeaObjectSO)
    {
        this.heyTeaObjectSO = heyTeaObjectSO;
    }

    /**
     * @brief Gets the HeyTeaObjectSO associated with this object.
     * @return The associated HeyTeaObjectSO.
     */
    public HeyTeaObjectSO GetHeyTeaObjectSO()
    {
        return heyTeaObjectSO;
    }

    /**
     * @brief Sets the parent of this HeyTeaObject.
     * @param heyTeaObjectParent The new parent of this object.
     * 
     * This method changes the parent of the object and updates its position and state accordingly.
     */
    public void SetHeyTeaObjectParents(IHeyTeaObjectParents heyTeaObjectParent)
    {
        // Clear the record of the old counter 
        if (this.heyTeaObjectParent != null)
        {
            this.heyTeaObjectParent.ClearHeyTeaObject();
        }
        // Set the record of the new counter
        this.heyTeaObjectParent = heyTeaObjectParent;

        // If heyTeaObjectParent has HeyTeaObject
        if (heyTeaObjectParent.HasHeyTeaObject())
        {
            // The HeyTeaObject is not Kitchenware
            if ((!heyTeaObjectParent.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct)))
            {
                Debug.LogError("Counter already has a object");
            }
        }

        heyTeaObjectParent.SetHeyTeaObject(this);

        // Refresh the visual
        transform.parent = heyTeaObjectParent.GetHeyTeaObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    /**
     * @brief Gets the parent of this HeyTeaObject.
     * @return The parent of this HeyTeaObject.
     */
    public IHeyTeaObjectParents GetHeyTeaObjectParents()
    {
        return heyTeaObjectParent;
    }

    /**
     * @brief Destroys this HeyTeaObject.
     */
    public void DestroySelf()
    {
        heyTeaObjectParent.ClearHeyTeaObject();
        DestroyImmediate(gameObject);
    }

    /**
     * @brief Tries to get the kitchenware interface from this object.
     * @param kichenwareObejct The kitchenware object if it exists.
     * @return True if the object is a kitchenware object, otherwise false.
     */
    public bool TryGetKichenware(out IKichenwareObejct kichenwareObejct)
    {
        if (this is IKichenwareObejct)
        {
            kichenwareObejct = this as IKichenwareObejct;
            return true;
        }
        else
        {
            kichenwareObejct = null;
            return false;
        }
    }

    /**
     * @brief Spawns a new HeyTeaObject.
     * @param heyTeaObjectSO The ScriptableObject to spawn.
     * @param heyTeaObjectParents The parent for the new HeyTeaObject.
     * @return The newly spawned HeyTeaObject.
     */
    public static HeyTeaObject SpawnHeyTeaObejct(HeyTeaObjectSO heyTeaObjectSO, IHeyTeaObjectParents heyTeaObjectParents)
    {
        Transform heyTeaTransform = Instantiate(heyTeaObjectSO.prefab);

        HeyTeaObject heyTeaObject = heyTeaTransform.GetComponent<HeyTeaObject>();

        heyTeaObject.SetHeyTeaObjectParents(heyTeaObjectParents);

        return heyTeaObject;
    }
}
