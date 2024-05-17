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
    // Define events for cabinet interactions
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    public override void Interact(Player player)
    {
        // Player has an object in hand
        if (player.HasHeyTeaObject())
        {
            Debug.Log("Can't put an object on a ContainerCounter and can't grab the object from the ContainerCounter.");
        }
        else
        {
            // Player has no object in hand, spawn a new object and let the player take it
            HeyTeaObject spawnedObject = HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
