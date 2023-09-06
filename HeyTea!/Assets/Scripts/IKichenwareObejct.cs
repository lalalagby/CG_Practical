using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKichenwareObejct
{
    /// <summary>
    /// teaBase:       tea or milkTea
    /// fruit:         fruit add,at most 2
    /// basicAdd:      sugar and ice
    /// ingredients :  red bean and pearl
    /// </summary>
    public enum MilkTeaMaterialType {
        none,
        teaBase,
        basicAdd,
        fruit,
        ingredients,
        unTreat,
    }
    public bool TryAddIngredient(HeyTeaObjectSO heyTeaObjectSO, MilkTeaMaterialType milkTeaMaterialType);

    public void Interact(IKichenwareObejct kichenwareObejct) {

    }

    public bool GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO) { heyTeaObjectSO = null;return false; }
}
