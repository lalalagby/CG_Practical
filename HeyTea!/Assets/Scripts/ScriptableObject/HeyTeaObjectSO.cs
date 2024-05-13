using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : HeyTeaObjectsSO.cs
Function  : Determine SO type
Author    : Yong Wu
Data      : 28.08.2023

*/

//Create this SO type
[CreateAssetMenu()]
public class HeyTeaObjectSO : ScriptableObject
{
    //can obtain the prefab and sprite in this SO through objectname
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
    public enum MilkTeaMaterialType {
        none,
        teaBase,
        basicAdd,
        fruit,
        ingredients,
        unTreat,
    }
    public MilkTeaMaterialType materialType;

    public object Clone() {
        // 实现深拷贝
        HeyTeaObjectSO heyTeaObjectSOClone = new HeyTeaObjectSO();
        heyTeaObjectSOClone.prefab = prefab;
        heyTeaObjectSOClone.sprite = sprite;
        heyTeaObjectSOClone.objectName = objectName;
        heyTeaObjectSOClone.materialType = materialType;
        return heyTeaObjectSOClone;
    }

}
