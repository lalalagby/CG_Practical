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
            if (allTime >= setInterval) {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = potObject.GetCookingProgressPercentage(), isProcessing = isCooking });
                allTime = 0;
            }
        }
    }

    public override void Interact(Player player) {
        if (HasHeyTeaObject() && GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenware)) {
            //has pot
            if (player.HasHeyTeaObject()) {
                //player has object
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenware1)) {
                    //if player has cup or pot,and the thing in pot can be put in cup
                    if (kichenware.GetOutputHeyTeaObejct(out HeyTeaObjectSO inPotHeyTeaObjectSO) && kichenware1.TryAddIngredient(inPotHeyTeaObjectSO, (IKichenwareObejct.MilkTeaMaterialType)inPotHeyTeaObjectSO.materialType)) {
                        HeyTeaObjectSO heyTeaObjectSOClone = (HeyTeaObjectSO)heyTeaObjectSO.Clone();
                        GetHeyTeaObject().DestroySelf();
                        HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSOClone, this);
                        isCooking = false;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {  isProcessing = isCooking });
                    }
                } else {
                    if (kichenware.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
                        player.GetHeyTeaObject().DestroySelf();
                        isCooking = false;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
                    }
                }
            } else {
                GetHeyTeaObject().SetHeyTeaObjectParents(player);
                isCooking = false;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
            }
        } else {
            //not have thing, if player hold pot, can put pot
            if (player.GetHeyTeaObject() as PotObject) {
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                isCooking = false;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
            }
        }
    }
    public override void Operation(Player player) {
        if (HasHeyTeaObject()) {
            //has pot
            PotObject potObject = GetHeyTeaObject() as PotObject;
            if (potObject != null && potObject.GetHeyTeaObject().GetHeyTeaObjectSO()!= potObject.GetOutputForInput(potObject.GetHeyTeaObject().GetHeyTeaObjectSO())) {
                //if the food can cook;
                if (isCooking == false) {
                    isCooking = true;
                    allTime = 0;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = true }) ;
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
