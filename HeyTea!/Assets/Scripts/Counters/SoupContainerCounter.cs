using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * @file SoupContainerCounter.cs
 * @brief Container interaction class
 * 
 * @details This class handles interactions between the player and the SoupContainerCounter.
 * It checks if the player is holding a kitchenware object (e.g., a cup) and attempts to add an ingredient to it.
 * If successful, it triggers an event.
 * 
 * @date 28.08.2023
 * @author Yong Wu, Xinyue Cheng
 */
public class SoupContainerCounter : BaseCounter
{
    /**
     * @brief Event triggered when the player grabs an object from the counter.
     */
    public event EventHandler OnPlayerGrabbedObject;

    /**
     * @brief The ingredient to be added to the kitchenware.
     */
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    /**
     * @brief Interacts with the player.
     * 
     * @details This method handles the interaction between the player and the counter. 
     * If the player is holding a kitchenware object (e.g., a cup), it attempts to add an ingredient to it. 
     * If successful, it triggers an event.
     * 
     * @param player The player interacting with the counter.
     */
    public override void Interact(Player player)
    {
        // To see if player has heytea object
        if (player.HasHeyTeaObject() == false)
        {
            Debug.Log("Player is not holding any object.");
            return;
        }

        // Try to get the kitchenware object from player's hand
        if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObject))
        {
            // Attempt to add ingredient to the kitchenware object
            bool tag = kichenwareObject.TryAddIngredient(heyTeaObjectSO, (IKichenwareObejct.MilkTeaMaterialType)heyTeaObjectSO.materialType);

            // If the kitchenware object is a cup, trigger the event
            if (tag)
            {
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
                Debug.Log("Add ingredient to the cup.");
                return;
            }
        }
        else
        {
            Debug.Log("Player is not holding a cup. Cannot interact with SoupContainerCounter.");
            return;
        }
    }
}
