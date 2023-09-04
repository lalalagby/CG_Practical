using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu()]
public class CookingRecipeSO :ScriptableObject
{
    public HeyTeaObjectSO input;
    public HeyTeaObjectSO output;
    public float cookingTimerMax;
    public float outputScale;
}
