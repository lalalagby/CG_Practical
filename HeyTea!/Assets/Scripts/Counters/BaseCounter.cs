using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief class for BaseCounters in the game.
 * 
 * @details As the base class of the cabinet, it has virtual interaction functions 
            and interface functions for item ownership processing.
 */
public class BaseCounter : MonoBehaviour, IHeyTeaObjectParents {

    public static event EventHandler OnAnyObjectPlacedHere;


    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform counterTopPoint; //!< The coordinate points of items on the cabinet.

    //Unified category of items
    public HeyTeaObject heyTeaObject;   //!< The unified category of items placed on the counter.

    /**
     * @brief Virtual function for interacting with Player.
     * 
     * @details This function is intended to be overridden by derived classes to implement specific interaction behavior.
     * 
     * @param player The Player interacting with the counter.
     */
    public virtual void Interact(Player player) { }

    
   /**
    * @brief Virtual function for holding operations with a time interval.
    * 
    * @details This function is intended to be overridden by derived classes to implement specific hold operation behavior.
    * 
    * @param player The player performing the hold operation.
    * @param timeInterval The time interval for the hold operation.
    */
    public virtual void OperationHold(Player player,float timeInterval) { }

    /**
     * @brief Virtual function for performing operations.
     * 
     * @details This function is intended to be overridden by derived classes to implement specific operation behavior.
     * 
     * @param player The player performing the operation.
     */
    public virtual void Operation(Player player) {  }

    /**
     * @brief Control interface function for item ownership.
     * 
     * @return The transform that the HeyTeaObject follows.
     */
    public Transform GetHeyTeaObjectFollowTransform() {
        return counterTopPoint;
    }

    /**
     * @brief Sets the HeyTeaObject on the counter.
     * 
     * @param heyTeaObject The HeyTeaObject to set on the counter.
     */
    public void SetHeyTeaObject(HeyTeaObject heyTeaObject) {
        this.heyTeaObject = heyTeaObject;

        if (heyTeaObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    /**
     * @brief Gets the HeyTeaObject currently on the counter.
     * 
     * @return The HeyTeaObject currently on the counter.
     */
    public HeyTeaObject GetHeyTeaObject() {
        return heyTeaObject;
    }

    /**
     * @brief Clears the HeyTeaObject from the counter.
     */
    public void ClearHeyTeaObject() {
        heyTeaObject = null;
    }

    /**
     * @brief Checks if there is a HeyTeaObject on the counter.
     * 
     * @return true if there is a HeyTeaObject on the counter, false otherwise.
     */
    public bool HasHeyTeaObject() {
        return heyTeaObject != null;
    }
}
