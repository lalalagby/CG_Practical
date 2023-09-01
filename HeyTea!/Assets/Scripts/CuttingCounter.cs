using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player) {
        if (!HasHeyTeaObject()) {
            //no object here
            if (player.HasHeyTeaObject()) {
                //player carry something;
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            }
        } else {
            //has object here
            if (!player.HasHeyTeaObject()) {
                //player not carry something;
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
        }
    }

    public override void InteractC(Player player) {
        if (HasHeyTeaObject()) {
            //has object in the chopping board
            HeyTeaObjectSO outputHeyTeaObjectSO=GetOutputForInput(GetHeyTeaObject().GetHeyTeaObjectSO());

            GetHeyTeaObject().DestroySelf();

            HeyTeaObject.SpawnHeyTeaObejct(outputHeyTeaObjectSO, this);
        }
    }

    private HeyTeaObjectSO GetOutputForInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputHeyTeaObjectSO) {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

}
