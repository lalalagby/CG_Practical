using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : ContainerCounter.cs
Function  : Material category
Author    : Yong Wu
Data      : 28.08.2023
Last Modified by: Bingyu Guo
Last Modification Date  :   14.05.2024
*/
public class HeyTeaObject : MonoBehaviour
{
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    private IHeyTeaObjectParents heyTeaObjectParent;

    public void SetHeyTeaObjectSO(HeyTeaObjectSO heyTeaObjectSO)
    {
        this.heyTeaObjectSO = heyTeaObjectSO;
    }

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

        // If heyTeaObjectParent has HeyTeaObject
        if (heyTeaObjectParent.HasHeyTeaObject()) {
            // The HeyTeaObject is not Kitchenware
            if ((!heyTeaObjectParent.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct))) {
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
        DestroyImmediate(gameObject);
        //Destroy(gameObject);
    }

    public bool TryGetKichenware(out IKichenwareObejct kichenwareObejct) {
        if(this is IKichenwareObejct) {
            kichenwareObejct = this as IKichenwareObejct;
            return true;
        } else {
            kichenwareObejct = null;
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
