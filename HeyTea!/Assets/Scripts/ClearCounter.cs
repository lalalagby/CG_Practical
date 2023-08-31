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
                //player carry something;
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            } 
        } else {
            //has object here
            if (!player.HasHeyTeaObject()) {
                //player not carry something;
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
        }
    }

}
