using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

/**
 * @author Yong Wu, Bingyu Guo
 * 
 * @date 28.08.2023
 * 
 * @brief Handle interaction between ClearCounter and Player.
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

public class ClearCounter : BaseCounter {
    /**
     * @brief   The ClearCounter interacts with the Player.
     * 
     * @details Player interacts with ClearCounter and handles the following three main cases:
     * 1. Player has nothing, but ClearCounter has HeyTeaObject. then put HeyTeaObject from ClearCounter to Player.
     * 2. Player has HeyTeaObject, but ClearCounter has nothing. Then put HeyTeaObject from Player to ClearCounter.
     * 3. Player and ClearCounter both have HeyTeaObject. execute HandleBothHaveObjects function.
     * 
     * @param Player that interact with the ClearCounter.
     * 
     * Interact has no return value.
     */
    public override void Interact(Player player) {
        // Counter and Player both have HeyTeaObject.
        if (HasHeyTeaObject() && player.HasHeyTeaObject()) {
            HandleBothHaveObjects(player);
            return;
        }

        // Only Player has a HeyTeaObject
        if (player.HasHeyTeaObject()) {
            // Put the object from player to counter
            player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            player.ClearHeyTeaObject();
            return;
        }

        // Only Counter has a HeyTeaObject
        if (HasHeyTeaObject()) {
            // Put the object from counter to player
            GetHeyTeaObject().SetHeyTeaObjectParents(player);
            ClearHeyTeaObject();
        }
    }

    /**
     * @brief   Handle the interaction when Player and ClearCounter both have HeyTeaObject.
     * 
     * @details When both Player and ClearCounter hold HeyTeaObject at the same time:
     * 1. Player holds HeyTeaObject as Cup and tries to put the HeyTeaObject held by ClearCounter into Cup. 
     *      If successful, destroy the HeyTeaObject held by ClearCounter.
     * 2. The HeyTeaObject held by ClearCounter is Cup, try to put the HeyTeaObject held by Player into Cup. 
     *      If successful, destroy the HeyTeaObject held by Player.
     * 
     * @param Player that interact with the ClearCounter.
     * 
     * HandleBothHaveObjects has no return value.
     */
    private void HandleBothHaveObjects(Player player)  {
        // If Player has a Cup
        if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct playerKichenware))  {
            var ingredientSO = GetHeyTeaObject().GetHeyTeaObjectSO();
            // Put ingredients from Counter to PlayerKitchenware
            if (playerKichenware.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType)) {
                GetHeyTeaObject().DestroySelf();
            }
            return;
        }

        // If Counter has a Cup
        if (GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct counterKichenware))  {
            var ingredientSO = player.GetHeyTeaObject().GetHeyTeaObjectSO();
            // Put ingredients from Player to CounterKitchenware
            if (counterKichenware.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType))  {
                player.GetHeyTeaObject().DestroySelf();
            }
        }
    }

}
