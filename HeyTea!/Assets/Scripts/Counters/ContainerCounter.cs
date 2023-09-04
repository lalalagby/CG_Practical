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
            //player hold something
            if (!HasHeyTeaObject()) {
                //no thing in the counter
                if (player.GetHeyTeaObject().TryGetCup(out CupObject cupObject)) {
                    //if player hold cup,and the object in the counter can be placed in cup directily
                    HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, this);
                    bool tag = cupObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (CupObject.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType);
                    //if can be placed in cup directly
                    if (tag) {
                        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
                    }
                    GetHeyTeaObject().DestroySelf();
                    //if cant be placed in cup directly,put cup in the counter
                    if (!tag) {
                        player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                    }
                } else {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                }
            } else {
                //has something in the counter
                if (player.GetHeyTeaObject().TryGetCup(out CupObject cupObject)) {
                    if (cupObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (CupObject.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        GetHeyTeaObject().DestroySelf();
                    }
                } else {
                    if (this.GetHeyTeaObject().TryGetCup(out  cupObject)) {
                        if (cupObject.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (CupObject.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                            player.GetHeyTeaObject().DestroySelf();
                        }
                    }
                }
            }
        } else {
            //player dont hold something
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
