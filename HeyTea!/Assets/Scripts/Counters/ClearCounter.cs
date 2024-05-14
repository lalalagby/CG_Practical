using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

/*
File Name : ClearCounter.cs
Function  : counter interacte action
Author    : Yong Wu
Created   : 28.08.2023
Last Modified by: Bingyu Guo
Last Modification Date  :   10.05.2024
*/

public class ClearCounter : BaseCounter
{
    //Interaction functions for counter items
    public override void Interact(Player player) {
        // If there is no HeyTeaObject on the counter
        if (!HasHeyTeaObject()) {
            // If the player carry HeyTeaObject
            if (player.HasHeyTeaObject()) {
                // Put the object from player to counter
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                player.ClearHeyTeaObject();
            } 
        } else {    // There is HeyTeaObject on the counter
            // If the player does not carry HeyTeaObject
            if (!player.HasHeyTeaObject()) {
                // Put the object from counter to player
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
                this.ClearHeyTeaObject();
            } else {    // The player has HeyTeaObject
                // If player holds a cup or a pot
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct)) {
                    // Try to add HeyTeaObject from the counter to the player's container 
                    if (kichenwareObejct.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        // Destroy HeyTeaObject on the counter
                        GetHeyTeaObject().DestroySelf();
                    }
                } else {    // The HeyTeaObject in the player's hand is not a cup
                    // If the counter holds a cup or a pot
                    if (this.GetHeyTeaObject().TryGetKichenware(out kichenwareObejct)) {
                        // If the object on the counter is a container, try to add the player's objects to the counter's container
                        if (kichenwareObejct.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                            // Destroy bject in Player's hands
                            player.GetHeyTeaObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}
