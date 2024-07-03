using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OrderSO : ScriptableObject
{
    public List<HeyTeaObjectSO> heyTeaObjectSOLists;

    public string orderName;
}