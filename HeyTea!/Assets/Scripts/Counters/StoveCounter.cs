using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public bool isCooking;
    }

    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    private bool isCooking;
    private float setInterval = 0.2f;
    private float allTime;

    private void Start() {
        HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, this);
        isCooking = false;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
    }

    private void Update() {
        if (isCooking&&HasHeyTeaObject()) {
            PotObject potObject = GetHeyTeaObject() as PotObject;
            allTime += Time.deltaTime;
            if (potObject.AddCurrentCookingProgress(Time.deltaTime)) {
                isCooking = false;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
            }
            if (allTime >= setInterval||isCooking==false) {
                Debug.Log(potObject.GetCookingProgressPercentage());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = potObject.GetCookingProgressPercentage(), isProcessing = isCooking });
                allTime = 0;
            }
        }
    }

    public override void Interact(Player player) {
        if (HasHeyTeaObject()&&GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObejct)) {
            //this counter only can have pot
            if(player.HasHeyTeaObject()) {
                //if player hold cup or pot
                if(player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwarePlayerHold)) {
                    if (!kichenwareObejct.InteractWithOtherKichenware(kichenwarePlayerHold)) {
                        if (kichenwarePlayerHold.InteractWithOtherKichenware(kichenwareObejct)) {
                            kichenwareObejct.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
                            kichenwareObejct.DestroyChild(heyTeaObjectSO);
                        }
                    } else {
                        kichenwarePlayerHold.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
                        kichenwarePlayerHold.DestroyChild(heyTeaObjectSO);
                    }
                } else {
                    if (kichenwareObejct.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        player.GetHeyTeaObject().DestroySelf();
                        isCooking = false;
                    } 
                }
            } else {
                GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
        } else {
            if(player.HasHeyTeaObject()&& player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwarePlayerHold)) {
                if (player.GetHeyTeaObject().GetHeyTeaObjectSO() == heyTeaObjectSO) {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                }
            }
        }
    }
    public override void Operation(Player player) {
        if (HasHeyTeaObject()) {
            //has pot
            PotObject potObject = GetHeyTeaObject() as PotObject;
            if (potObject != null && potObject.CanCook()) {
                //if the food can cook;
                if (isCooking == false) {
                    isCooking = true;
                    allTime = 0;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = true });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
                } else {
                    isCooking = false;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
                }
            }
        }
    }
}
