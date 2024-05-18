using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

/*
File Name : ClearCounter.cs
Function  : counter interacte action
Author    : Yong Wu
Created   : 28.08.2023
Last Modified by: Bingyu Guo
Last Modification Date  :   18.05.2024
*/

public class ClearCounter : BaseCounter {
    public override void Interact(Player player) {

        // Counter and Player both have HeyTeaObject.
        if (HasHeyTeaObject() && player.HasHeyTeaObject()) {
            print("Counter and Player both have HeyTeaObject.");
            HandleBothHaveObjects(player);
            return;
        }

        // Only Player has a HeyTeaObject
        if (player.HasHeyTeaObject()) {
            print("Only Player has a HeyTeaObject");
            // Put the object from player to counter
            player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            player.ClearHeyTeaObject();
            return;
        }

        // Only Counter has a HeyTeaObject
        if (HasHeyTeaObject()) {
            print("Only Counter has a HeyTeaObject");
            // Put the object from counter to player
            GetHeyTeaObject().SetHeyTeaObjectParents(player);
            ClearHeyTeaObject();
        }
    }

    // If Player and Counter both has HeyTeaObject
    private void HandleBothHaveObjects(Player player)  {
        // If Player has a kitchenware(cup)
        if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct playerKichenware))  {
            var ingredientSO = GetHeyTeaObject().GetHeyTeaObjectSO();
            // Put ingredients from Counter to PlayerKitchenware
            if (playerKichenware.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType)) {
                GetHeyTeaObject().DestroySelf();
            }
            return;
        }

        // If Counter has a kitchenware(cup)
        if (GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct counterKichenware))  {
            var ingredientSO = player.GetHeyTeaObject().GetHeyTeaObjectSO();
            // Put ingredients from Player to CounterKitchenware
            if (counterKichenware.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType))  {
                player.GetHeyTeaObject().DestroySelf();
            }
        }
    }

}
