using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;

/**
 * @author Yong Wu, Bingyu Guo
 * 
 * @brief The StoveCounter class is responsible for handling interactions with the stove, including cooking progress and managing objects on the stove.
 * 
 * @details This class extends the BaseCounter and implements the IHasProgress interface. It manages the cooking process, handles player interactions, and updates the progress state.
 * 
 * @note
 *      milkTeaMaterialType contains six types: teaBase, basicAdd, ingredients, fruit, none, un treat.
 *      HeyTeaObject includes the objects contained in teaBase, basicAdd, ingredients, fruit, none, un treat.
 *          - teaBase: tea, milk, milktea
 *          - basicAdd: sugar, ice
 *          - ingredients: red bean cooked, pearl cooked
 *          - fruit: orange slice, grape slice, strawberry slice
 *          - none: pot, cup
 *          - un treat: bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry
 */

public class StoveCounter : BaseCounter,IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;  //!< Event triggered when the cooking state changes.
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;   //!< Event triggered when the cooking progress changes.

    /**
     * @brief Event arguments for the OnStateChanged event.
     */
    public class OnStateChangedEventArgs : EventArgs {
        public bool isCooking;
    }

    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;     //!< PotObjectSO

    private bool isCooking;
    private float setInterval = 0.2f;
    private float allTime;

    /**
     * @brief Initializes the StoveCounter at the start of the game.
     * 
     * @details This method spawns a Pot on the StoveCounter and sets the initial cooking state to false.
     */
    private void Start() {
        HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, this);
        isCooking = false;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { isCooking = false });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = isCooking });
    }

    /**
    * @brief Updates the cooking progress and handles cooking completion.
    * 
    * @details This method checks if the StoveCounter has a Pot and an ingredient is cooking in the Pot. 
    *           It updates the cooking progress and triggers events when the state changes.
    */
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

    /**
     * @brief Handles interactions between the Player and the StoveCounter.
     * 
     * @details This method manages different interaction scenarios, 
     *          such as when both the Player and StoveCounter have HetTeaObjects, the Player has a Pot, or  StoveCounter has a Pot.
     * 
     * @param player The player interacting with the stove counter.
     */
    public override void Interact(Player player) {

        // StoveCounter has Pot and Player has HeyTeaObject.
        if (HasHeyTeaObject() && player.HasHeyTeaObject()) {
            GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct pot);
            HandleBothHaveObjects(player, pot);
            return;
        }

        // Player has Pot, StoveCounter has nothing
        if (player.HasHeyTeaObject() ) {
            if (player.GetHeyTeaObject() is PotObject) {
                // Put the object from player to counter
                player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                player.ClearHeyTeaObject();
                return;
            }
               
        }

        // StoveCounter has Pot, Player has nothing
        if (HasHeyTeaObject()) {
            // Put the object from counter to player
            GetHeyTeaObject().SetHeyTeaObjectParents(player);
            ClearHeyTeaObject();
        }
    }

    /**
     * @brief Handles the scenario where Player has HeyTeaObject and StoveCounter has a Pot
     * 
     * @details This method manages interactions between the Player's HeyTeaObject and the Pot on the StoveCounter, 
     *          such as transferring ingredients.
     * 
     * @param player The player interacting with the stove counter.
     * @param pot The pot on the stove counter.
     */
    private void HandleBothHaveObjects(Player player, IKichenwareObejct pot) {
        var flag =  player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct cup);

        // If Player has a Cup: Try to put HeyTeaObject from pot to playerKitchenware (Only cooked Ingredients could be added into Cup)
        // The interaction will be happened between pot and cup, rather than stovecounter and player
        if (flag && cup.InteractWithOtherKichenware(pot)) {
            // Delete this HeyTeaObject from pot
            pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
            pot.DestroyChild(heyTeaObjectSO);
            return;
        }

        // If Player has Food, try to put HeyTeaObject from Player to pot
        if (pot.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType)) {
            player.GetHeyTeaObject().DestroySelf();
            player.ClearHeyTeaObject();
            isCooking = false;
        }
}
    /**
     * @brief Initiates or stops the cooking process on the StoveCounter.
     * 
     * @details This method checks if the StoveCounter has a Pot and if the food can be cooked. 
     *          It toggles the cooking state and triggers the corresponding events.
     * 
     * @param player The player initiating the cooking process.
     */
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
