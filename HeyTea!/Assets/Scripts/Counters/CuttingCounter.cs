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

public class CuttingCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    //cut animation event
    public event EventHandler OnCut;

    //define the objects array that can be cut
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    //processing time
    private float cuttingProgress;
    private int animationTime = 0;
    private float animationInterval = 0.2f;


    public override void Interact(Player player) {
        if (!HasHeyTeaObject()) {
            //no object here
            if (player.HasHeyTeaObject()) {
                //player carry something that can be cut
                if (HasRecipeWithInput(player.GetHeyTeaObject().GetHeyTeaObjectSO())) {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                    cuttingProgress = 0;
                    animationTime = 0;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized=(float)cuttingProgress/ GetCuttingRecipeSOWithInput(GetHeyTeaObject().GetHeyTeaObjectSO()).cuttingProgressMax });
                }
            }
        } else {
            //has object here
            if (!player.HasHeyTeaObject()) {
                //player not carry something;
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            } else {
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct cupObject)) {
                    if (cupObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        GetHeyTeaObject().DestroySelf();
                    }
                }
            }
        }
    }

   
    public override void OperationHold(Player player, float timeInterval) {
        //only can cut some object that can be cut;
        if (HasHeyTeaObject() && HasRecipeWithInput(GetHeyTeaObject().GetHeyTeaObjectSO())) {
            cuttingProgress+=timeInterval;

            if((int)(cuttingProgress/ animationInterval) > animationTime) {
                OnCut?.Invoke(this, EventArgs.Empty);
                animationTime = (int)(cuttingProgress / animationInterval);
            }
            

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetHeyTeaObject().GetHeyTeaObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                //has object in the chopping board
                HeyTeaObjectSO outputHeyTeaObjectSO = GetOutputForInput(GetHeyTeaObject().GetHeyTeaObjectSO());

                GetHeyTeaObject().DestroySelf();

                HeyTeaObject.SpawnHeyTeaObejct(outputHeyTeaObjectSO, this);

            }
        } else {
            if(player.HasHeyTeaObject()){
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObject)) {
                    if (kichenwareObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        GetHeyTeaObject().DestroySelf();
                    }
                }
                if (this.HasHeyTeaObject()) {
                    if (this.GetHeyTeaObject().TryGetKichenware(out kichenwareObject)) {
                        if (kichenwareObject.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                            player.GetHeyTeaObject().DestroySelf();
                        }
                    }
                }
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
