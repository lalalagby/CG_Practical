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
            if((!heyTeaObjectParent.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct))) {
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

    public static bool HeyTeaObejctInteract(HeyTeaObject heyTeaObject1,HeyTeaObject heyTeaObject2) {
        if(heyTeaObject1.TryGetKichenware(out IKichenwareObejct kichenwareObejct1)) {
            if(heyTeaObject2.TryGetKichenware(out IKichenwareObejct kichenwareObejct2)) {
                if (kichenwareObejct1.InteractWithOtherKichenware(kichenwareObejct2)) {
                    kichenwareObejct2.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
                    kichenwareObejct2.DestroyChild(heyTeaObjectSO);
                    return true;
                } else {
                    if (kichenwareObejct2.InteractWithOtherKichenware(kichenwareObejct1)) {
                        kichenwareObejct1.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
                        kichenwareObejct1.DestroyChild(heyTeaObjectSO);
                        return true;
                    } else {
                        return false;
                    }
                }
            } else {
                if (kichenwareObejct1.TryAddIngredient(heyTeaObject2.GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)heyTeaObject2.GetHeyTeaObjectSO().materialType)) {
                    heyTeaObject2.DestroySelf();
                    return true;
                } else {
                    return false;
                }
            }
        } else {
            if(heyTeaObject2.TryGetKichenware(out IKichenwareObejct kichenwareObejct2)) {
                if (kichenwareObejct2.TryAddIngredient(heyTeaObject1.GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)heyTeaObject1.GetHeyTeaObjectSO().materialType)) {
                    heyTeaObject1.DestroySelf();
                    return true;
                } else {
                    return false;
                }
            } else {
                //both normal objects,cant interact
                return false;
            }
        }
    }
}
