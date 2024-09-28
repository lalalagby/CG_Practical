using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief Class for handling trash disposal in the game.
 * 
 * @details This class extends the BaseCounter class and provides functionality to interact with the player
 *          and destroy any HeyTeaObject that the player is holding, simulating a trash disposal. Additionally,
 *          it triggers an event when an item is disposed.
 */
public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    /**
     * @brief Resets static data for the class.
     */
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public event Action OnItemDisposed; //!< Event triggered when an item is disposed.
    /**
     * @brief Interacts with the player to dispose of HeyTeaObjects.
     * 
     * @details This method overrides the Interact method from the BaseCounter class. 
     *          It checks if the Player is holding a HeyTeaObject, and if so, destroys the object. 
     *          If the object is a PotObject, it calls the DestroySelf method on the PotObject,
     *          and triggers the OnItemDisposed event. 
     *          Otherwise, it calls the DestroySelf method on the HeyTeaObject.  
     * 
     * @param player The Player interacting with the TrashCounter.
     */
    public override void Interact(Player player) {
        // If Player has HeyTeaObject
        if (player.HasHeyTeaObject()) {
            PotObject potObject = player.GetHeyTeaObject() as PotObject;
            // If the HeyTeaObject is not Pot.
            if(potObject == null) {
                player.GetHeyTeaObject().DestroySelf();
            }else if (potObject.CheckTotalInpotNum() > 0) { 
                potObject.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
                potObject.DestroyChild(heyTeaObjectSO);
                OnItemDisposed?.Invoke();
                OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
                //potObject.DestroySelf();
            }
        }
    }
}
