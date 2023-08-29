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

public class ClearCounter : MonoBehaviour,IHeyTeaObjectParents
{
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] bool testing;

    private HeyTeaObject heyTeaObject;

    private void Update() {
        if (testing && Input.GetKeyDown(KeyCode.T)){
            if (heyTeaObject != null) {
                heyTeaObject.SetHeyTeaObjectParents(secondClearCounter);
            }
        }
    }

    //Interaction functions for counter items
    public void Interact(Player player) {
        if (heyTeaObject == null) {
            //Set the coordinates for placing objects based on the coordinate values of top and update the visual effect.
            Transform heyTeaTransform = Instantiate(heyTeaObjectSO.prefab, counterTopPoint);
            heyTeaTransform.GetComponent<HeyTeaObject>().SetHeyTeaObjectParents(this);
        } else {
            //give the object to player;
            //heyTeaObject.SetClearCounter(player);
        }
    }

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
