using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
File Name : TrashCounter.cs
Function  : Throw trash and destory them
Author    : Yong Wu
Data      : 02.09.2023

*/
public class TrashCounter : BaseCounter
{
    public override void Interact(Player player) {
        if (player.HasHeyTeaObject()) {
            PotObject potObject = player.GetHeyTeaObject() as PotObject;
            if (potObject != null) {
                potObject.DestroySelf();
            } else {
                player.GetHeyTeaObject().DestroySelf();
            }
        }
    }
}
