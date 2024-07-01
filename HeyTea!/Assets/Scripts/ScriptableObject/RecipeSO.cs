using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<HeyTeaObjectSO> heyTeaObjectSOLists;

    public string recipeName;
}