using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/*
File Name : SoupContainerCounter.cs
Function  : Container interaction class
Author    : Yong Wu
Data      : 28.08.2023
Last Modified by: Xinyue Cheng
Last Modification Date  :   18.05.2024
*/

public class SoupContainerCounter : BaseCounter
{
    // Define events for cabinet interactions
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    public override void Interact(Player player)
    {
        // Player has an object in hand
        if (player.HasHeyTeaObject())
        {
            // If the player is holding a cup
            if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObject))
            {
                // Attempt to add ingredient to the kitchenware object
                bool tag = kichenwareObject.TryAddIngredient(heyTeaObjectSO, (IKichenwareObejct.MilkTeaMaterialType)heyTeaObjectSO.materialType);
                // If the interaction is successful, trigger the event and destroy the object in player's hand
                if (tag)
                {
                    OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
                    Debug.Log("Add ingredient to the kitchenware object.");
                }
                else
                {
                    Debug.Log("Cannot add ingredient to the kitchenware object.");
                }
            }
            else
            {
                Debug.Log("Cannot put ingredient on SoupContainerCounter.");
            }
        }
        else
        {
            // Players can't take the milk or tea directly, they must have a kitchenware.
            Debug.Log("Cannot grab this object.");
        }
    }
}
