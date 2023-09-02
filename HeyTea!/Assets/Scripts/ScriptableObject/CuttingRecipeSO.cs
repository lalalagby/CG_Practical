using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : CuttingRecipeSO.cs
Function  : Define a new association method so that the items after cutting, 
            the items before cutting, and the cutting time can be obtained
Author    : Yong Wu
Data      : 01.09.2023

*/

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{

    public HeyTeaObjectSO input;
    public HeyTeaObjectSO output;
    public int cuttingProgressMax;
}
