using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : ContainerCounter.cs
Function  : Interface for item acquisition and conversion
Author    : Yong Wu
Data      : 28.08.2023

*/

public interface IHeyTeaObjectParents 
{
    public Transform GetHeyTeaObjectFollowTransform();

    public void SetHeyTeaObject(HeyTeaObject heyTeaObject);

    public HeyTeaObject GetHeyTeaObject();
    public void ClearHeyTeaObject();

    public bool HasHeyTeaObject();
}
