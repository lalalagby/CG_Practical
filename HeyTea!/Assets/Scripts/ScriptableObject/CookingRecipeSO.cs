using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu()]
public class CookingRecipeSO :ScriptableObject
{
    public HeyTeaObjectSO input;
    public HeyTeaObjectSO midState;
    public HeyTeaObjectSO output;
    public float cookingTimerMax;
}
