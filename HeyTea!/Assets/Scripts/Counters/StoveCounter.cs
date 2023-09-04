using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter
{

    [SerializeField] private CookingRecipeSO[] cookingRecipeSOArray;

    private float cookingProgress;

    public override void Interact(Player player) {
        if (!HasHeyTeaObject()) {
            //no object here
            if (player.HasHeyTeaObject()) {
                //player carry something that can be cut
                if (HasRecipeWithInput(player.GetHeyTeaObject().GetHeyTeaObjectSO())) {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                    cookingProgress = 0;

                    HeyTeaObjectSO outputHeyTeaObjectSO = GetOutputForInput(GetHeyTeaObject().GetHeyTeaObjectSO());
                    GetHeyTeaObject().DestroySelf();
                    HeyTeaObject.SpawnHeyTeaObejct(outputHeyTeaObjectSO, this);
                }
            }
        } else {
            //has object here
            if (!player.HasHeyTeaObject()) {
                //player not carry something;
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
        }
    }

    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        return cookingRecipeSO != null;
    }

    private HeyTeaObjectSO GetOutputForInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputHeyTeaObjectSO);
        if (cookingRecipeSO != null) {
            return cookingRecipeSO.output;
        } else {
            return null;
        }
    }

    private CookingRecipeSO GetCookingRecipeSOWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOArray) {
            if (cookingRecipeSO.input == inputHeyTeaObjectSO) {
                return cookingRecipeSO;
            }
        }
        return null;
    }
}
