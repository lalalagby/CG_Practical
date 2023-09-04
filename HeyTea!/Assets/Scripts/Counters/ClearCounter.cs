using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*
File Name : ClearCounter.cs
Function  : counter interacte action
Author    : Yong Wu
Data      : 28.08.2023

*/

public class ClearCounter : BaseCounter
{
    //Interaction functions for counter items
    public override void Interact(Player player) {
        if (!HasHeyTeaObject()) {
            //no object here
            if (player.HasHeyTeaObject()) {
                //player carry something,put object from player to counter
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            } 
        } else {
            //has object here
            if (!player.HasHeyTeaObject()) {
                //player not carry something,put object from counter to player
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            } else {
                //if player hold the cup,so check whether the obeject can be placed in the cup.
                if (player.GetHeyTeaObject().TryGetCup(out CupObject cupObject)) {
                    if (cupObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (CupObject.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        GetHeyTeaObject().DestroySelf();
                    }
                } else {
                    //if cup is in the counter
                    if (this.GetHeyTeaObject().TryGetCup(out CupObject cupObject1)) {
                        if (cupObject1.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (CupObject.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                            player.GetHeyTeaObject().DestroySelf();
                        }
                    }
                }
               
            }
        }
    }

}
