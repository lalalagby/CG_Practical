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
        // To see if player has heytea object
        if (player.HasHeyTeaObject() == false)
        {
            Debug.Log("Player is not holding any object.");
            return;
        }

        // Try to get the kichenware object from player's hand
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
