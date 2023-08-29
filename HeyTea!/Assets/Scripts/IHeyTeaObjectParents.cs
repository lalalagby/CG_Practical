using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeyTeaObjectParents 
{
    public Transform GetHeyTeaObjectFollowTransform();

    public void SetHeyTeaObject(HeyTeaObject heyTeaObject);

    public HeyTeaObject GetHeyTeaObject();
    public void ClearHeyTeaObject();

    public bool HasHeyTeaObject();
}
