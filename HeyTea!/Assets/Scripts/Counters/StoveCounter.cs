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
        // If StoveCounter has a Pot, and Ingredient is cooking in Pot
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

        // StoveCounter has Pot and Player has HeyTeaObject.
        if (HasHeyTeaObject() && player.HasHeyTeaObject()) {
            GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct pot);
            HandleBothHaveObjects(player, pot);
            return;
        }

        // Player has Pot, StoveCounter has nothing
        if (player.HasHeyTeaObject() && player.GetHeyTeaObject() is PotObject) {
            // Put the object from player to counter
            player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
            player.ClearHeyTeaObject();
            return;
        }

        // StoveCounter has Pot, Player has nothing
        if (HasHeyTeaObject()) {
            // Put the object from counter to player
            GetHeyTeaObject().SetHeyTeaObjectParents(player);
            ClearHeyTeaObject();
            return;
        }

    }

    // If Player and Counter both has HeyTeaObject
    private void HandleBothHaveObjects(Player player, IKichenwareObejct pot) {
        // If Player has a kitchenware(i.e. Cup)
        if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct playerKitchenware)) {
            // Try to put HeyTeaObject from pot to playerKitchenware (Only cooked Ingredients could be added into Cup)
            if (playerKitchenware.InteractWithOtherKichenware(pot)) {
                // Delete this HeyTeaObject from pot
                pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
                pot.DestroyChild(heyTeaObjectSO);
                return;
            }
            return;
        }

        // If Player has Food, try to put HeyTeaObject from Player to pot
        if (pot.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
            player.GetHeyTeaObject().DestroySelf();
            player.ClearHeyTeaObject();
            isCooking = false;
        }

    }

    public override void Operation(Player player) {
        // If StoveCounter has Pot
        if (HasHeyTeaObject()) {
            PotObject potObject = GetHeyTeaObject() as PotObject;
            // If Food can be cooked;
            if (potObject.CanCook()) {
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
