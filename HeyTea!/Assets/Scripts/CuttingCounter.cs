using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
File Name : CuttingCounter.cs
Function  : Cutting cabinets
Author    : Yong Wu
Data      : 01.09.2023

*/

public class CuttingCounter : BaseCounter
{
    //Define the cutting progress bar and modify the progress event
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    //Event passing cutting progress
    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }
    //cut animation event
    public event EventHandler OnCut;

    //define the objects array that can be cut
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    //processing time
    private int cuttingProgress;
    public override void Interact(Player player) {
        if (!HasHeyTeaObject()) {
            //no object here
            if (player.HasHeyTeaObject()) {
                //player carry something that can be cut
                if (HasRecipeWithInput(player.GetHeyTeaObject().GetHeyTeaObjectSO())) {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                    cuttingProgress = 0;

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {progressNormalized=(float)cuttingProgress/ GetCuttingRecipeSOWithInput(GetHeyTeaObject().GetHeyTeaObjectSO()).cuttingProgressMax });
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

   
    public override void InteractC(Player player) {
        //only can cut some object that can be cut;
        if (HasHeyTeaObject()&& HasRecipeWithInput(GetHeyTeaObject().GetHeyTeaObjectSO())) {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetHeyTeaObject().GetHeyTeaObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                //has object in the chopping board
                HeyTeaObjectSO outputHeyTeaObjectSO = GetOutputForInput(GetHeyTeaObject().GetHeyTeaObjectSO());


                GetHeyTeaObject().DestroySelf();
                HeyTeaObject.SpawnHeyTeaObejct(outputHeyTeaObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputHeyTeaObjectSO);
        return cuttingRecipeSO != null;
    }

    private HeyTeaObjectSO GetOutputForInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputHeyTeaObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(HeyTeaObjectSO inputHeyTeaObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputHeyTeaObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
