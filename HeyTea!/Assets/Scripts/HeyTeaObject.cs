using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeyTeaObject : MonoBehaviour
{
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    private IHeyTeaObjectParents heyTeaObjectParent;

    public HeyTeaObjectSO GetHeyTeaObjectSO() {
        return heyTeaObjectSO;
    }

    public void SetHeyTeaObjectParents(IHeyTeaObjectParents heyTeaObjectParent) {
        //Clear the record of the old counter 
        if (this.heyTeaObjectParent != null) {
            this.heyTeaObjectParent.ClearHeyTeaObject();
        }
        //set the record of the new counter
        this.heyTeaObjectParent = heyTeaObjectParent;

        if (heyTeaObjectParent.HasHeyTeaObject()) {
            Debug.LogError("Counter already has a object");
        }

        heyTeaObjectParent.SetHeyTeaObject(this);

        //refresh the visual
        transform.parent = heyTeaObjectParent.GetHeyTeaObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IHeyTeaObjectParents GetHeyTeaObjectParents() {
        return heyTeaObjectParent;
    }
}
