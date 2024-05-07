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

*/

public class SoupContainerCounter : BaseCounter 
{
    //Define events for cabinet crawling
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    public override void Interact(Player player) {
        //Set the coordinates for placing objects based on the coordinate values of top and update the visual effect.
        if (player.HasHeyTeaObject()) {
            if (!HasHeyTeaObject()) {
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct)) {
                    HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, this);
                    bool tag = kichenwareObejct.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType);
                    if (tag) {            
                        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
                    }
                    GetHeyTeaObject().DestroySelf();
                    if (!tag) {
                        player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                    }
                } else {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                }
            } else {
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct)) {
                    //player hold cup
                    if (kichenwareObejct.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        GetHeyTeaObject().DestroySelf();
                    }
                } else {
                    if (this.GetHeyTeaObject().TryGetKichenware(out kichenwareObejct)) {
                       {
                            if (kichenwareObejct.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                                player.GetHeyTeaObject().DestroySelf();
                            }
                       }
                    }
                }
            }
        } else {
            if (HasHeyTeaObject()) {
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            } 
        }
    }
}
