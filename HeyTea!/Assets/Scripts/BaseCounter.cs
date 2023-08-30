using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IHeyTeaObjectParents {

    [SerializeField] private Transform counterTopPoint;

    private HeyTeaObject heyTeaObject;
    public virtual void Interact(Player player) { }

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
