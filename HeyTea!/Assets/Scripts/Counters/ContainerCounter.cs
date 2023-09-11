using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            if (HasHeyTeaObject()) {
                HeyTeaObject.HeyTeaObejctInteract(player.GetHeyTeaObject(), GetHeyTeaObject());
            } else {
                if(player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct)) {
                    HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, this);
                    bool tag = kichenwareObejct.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType);
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
            }
        } else {
            if (HasHeyTeaObject()) {
                GetHeyTeaObject().SetHeyTeaObjectParents(player);
            } else {
                HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, player);
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
