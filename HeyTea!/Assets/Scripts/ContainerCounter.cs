using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter 
{
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
                Transform heyTeaTransform = Instantiate(heyTeaObjectSO.prefab);
                heyTeaTransform.GetComponent<HeyTeaObject>().SetHeyTeaObjectParents(player);
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            } else {
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
        }
    }
}
