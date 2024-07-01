using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.GetHeyTeaObject() is CupObject)
        {
            player.GetHeyTeaObject().DestroySelf();
            Debug.Log('1');
        }
        else
        {
            this.ClearHeyTeaObject();
            Debug.Log('2');
        }
    }
}
