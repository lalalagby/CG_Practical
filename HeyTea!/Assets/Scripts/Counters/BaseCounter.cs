using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : BaseCounter.cs
Function  : As the base class of the cabinet, it has virtual interaction functions 
            and interface functions for item ownership processing.
Author    : Yong Wu
Data      : 31.08.2023

*/

public class BaseCounter : MonoBehaviour, IHeyTeaObjectParents {

    //Coordinate points of items on the cabinet
    [SerializeField] private Transform counterTopPoint;

    //Unified category of items
    public HeyTeaObject heyTeaObject;

    //Virtual functions for interacting with users
    public virtual void Interact(Player player) { }

    public virtual void OperationHold(Player player,float timeInterval) { }

    public virtual void Operation(Player player) {  }

    //Control interface function for item ownership
    public Transform GetHeyTeaObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetHeyTeaObject(HeyTeaObject heyTeaObject) {
        this.heyTeaObject = heyTeaObject;
    }

    public HeyTeaObject GetHeyTeaObject() {
        return heyTeaObject;
    }

    public void ClearHeyTeaObject() {
        heyTeaObject = null;
    }

    public bool HasHeyTeaObject() {
        return heyTeaObject != null;
    }
}
