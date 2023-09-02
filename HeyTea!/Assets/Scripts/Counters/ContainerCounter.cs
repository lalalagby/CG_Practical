using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
File Name : ContainerCounter.cs
Function  : Container interaction class
Author    : Yong Wu
Data      : 28.08.2023

*/

public class ContainerCounter : BaseCounter 
{
    //Define events for cabinet crawling
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;
    public override void Interact(Player player) {
        //Set the coordinates for placing objects based on the coordinate values of top and update the visual effect.
        if (player.HasHeyTeaObject()) {
            if (!HasHeyTeaObject()) {
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            }
        } else {
            if (!HasHeyTeaObject()) {
                //When the player has nothing in their hands and there is nothing on the cabinet, 
                //create a new item and broadcast the animation event captured by the cabinet
                HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, player);

                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            } else {
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
        }
    }
}
