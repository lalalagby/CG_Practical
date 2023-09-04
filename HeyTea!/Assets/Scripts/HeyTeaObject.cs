using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : ContainerCounter.cs
Function  : Material category
Author    : Yong Wu
Data      : 28.08.2023

*/
public class HeyTeaObject : MonoBehaviour
{
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    private IHeyTeaObjectParents heyTeaObjectParent;

    public HeyTeaObjectSO GetHeyTeaObjectSO() {
        return heyTeaObjectSO;
    }

    //Convert the owner and location of this item by passing in the owner.
    public void SetHeyTeaObjectParents(IHeyTeaObjectParents heyTeaObjectParent) {
        //Clear the record of the old counter 
        if (this.heyTeaObjectParent != null) {
            this.heyTeaObjectParent.ClearHeyTeaObject();
        }
        //set the record of the new counter
        this.heyTeaObjectParent = heyTeaObjectParent;

        if (heyTeaObjectParent.HasHeyTeaObject()) {
            if(!heyTeaObjectParent.GetHeyTeaObject().TryGetCup(out CupObject cupObject)) {
                Debug.LogError("Counter already has a object");
            }
        }

        heyTeaObjectParent.SetHeyTeaObject(this);

        //refresh the visual
        transform.parent = heyTeaObjectParent.GetHeyTeaObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IHeyTeaObjectParents GetHeyTeaObjectParents() {
        return heyTeaObjectParent;
    }

    public void DestroySelf() {
        heyTeaObjectParent.ClearHeyTeaObject();
        Destroy(gameObject);
    }

    public bool TryGetCup(out CupObject cupObject) {
        if(this is CupObject) {
            cupObject = this as CupObject;
            return true;
        } else {
            cupObject = null;
            return false;
        }
        
    }

    public static HeyTeaObject SpawnHeyTeaObejct(HeyTeaObjectSO heyTeaObjectSO,IHeyTeaObjectParents heyTeaObjectParents) {
        Transform heyTeaTransform = Instantiate(heyTeaObjectSO.prefab);

        HeyTeaObject heyTeaObject = heyTeaTransform.GetComponent<HeyTeaObject>();

        heyTeaObject.SetHeyTeaObjectParents(heyTeaObjectParents);

        return heyTeaObject;
    }
}
