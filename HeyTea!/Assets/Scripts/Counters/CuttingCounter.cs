using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * @brief Handles the cutting of items on the cutting counter.
 * @details This class extends BaseCounter and implements the IHasProgress interface. It allows interaction with players to cut items, tracks progress, and triggers events.
 * 
 * @date 01.09.2023
 * @author Yong Wu
 */

/**
 * @class CuttingCounter
 * @brief Cutting cabinets for processing objects.
 * 
 * The CuttingCounter class extends BaseCounter and implements IHasProgress interface.
 * It allows interaction with players to cut items, tracks progress, and triggers events.
 */
public class CuttingCounter : BaseCounter, IHasProgress
{
    public static EventHandler OnAnyCut;
    /**
     * @brief Event triggered when the progress of cutting changes.
     */
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    /**
     * @brief Event triggered for the cut animation.
     */
    public event EventHandler OnCut;

    /**
     * @brief Array of objects that can be cut.
     */
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    /**
     * @brief The current progress of cutting.
     */
    private float cuttingProgress;

    /**
     * @brief Time for animation.
     */
    private int animationTime = 0;

    /**
     * @brief Interval for animation.
     */
    private float animationInterval = 0.2f;

    /**
     * @brief Handles interaction with the player.
     * @param player The player interacting with the counter.
     * 
     * @details If the counter has no object, it will check if the player is carrying an object that can be cut. If so, it transfers the object to the counter and starts the cutting process. If the counter already has an object, it will check if the player has a cup and try to add the object to the cup.
     */
    public override void Interact(Player player)
    {
        if (!HasHeyTeaObject())
        {
            // No object here
            Debug.Log("counter has no object");
            if (player.HasHeyTeaObject())
            {
                // Player carries something that can be cut
                if (HasRecipeWithInput(player.GetHeyTeaObject().GetHeyTeaObjectSO()))
                {
                    player.GetHeyTeaObject().SetHeyTeaObjectParents(this);
                    cuttingProgress = 0;
                    animationTime = 0;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = false, progressNormalized = (float)cuttingProgress / GetCuttingRecipeSOWithInput(GetHeyTeaObject().GetHeyTeaObjectSO()).cuttingProgressMax });
                    // 柜子上有物品，开始切菜
                    OnAnyCut?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                Debug.Log("player has no object.");
            }
        }
        else
        {
            // Has object here
            if (!player.HasHeyTeaObject())
            {
                // Player not carrying something
                this.GetHeyTeaObject().SetHeyTeaObjectParents(player);
            }
            else
            {
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct cupObject))
                {
                    if (cupObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType))
                    {
                        GetHeyTeaObject().DestroySelf();
                        return;
                    }
                }
            }
        }
    }

    /**
     * @brief Holds the operation for cutting.
     * @param player The player interacting with the counter.
     * @param timeInterval The time interval for holding the operation.
     * 
     * @details This method handles the continuous cutting operation. It updates the cutting progress and triggers the cut animation and progress change events. Once the cutting is complete, it spawns the output object.
     */
    public override void OperationHold(Player player, float timeInterval)
    {
        // Only can cut some object that can be cut
        if (HasHeyTeaObject() && HasRecipeWithInput(GetHeyTeaObject().GetHeyTeaObjectSO()))
        {
            cuttingProgress += timeInterval;

            if ((int)(cuttingProgress / animationInterval) > animationTime)
            {
                OnCut?.Invoke(this, EventArgs.Empty);
                animationTime = (int)(cuttingProgress / animationInterval);
            }

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetHeyTeaObject().GetHeyTeaObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { isProcessing = true, progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // Has object on the chopping board
                HeyTeaObjectSO outputHeyTeaObjectSO = GetOutputForInput(GetHeyTeaObject().GetHeyTeaObjectSO());

                GetHeyTeaObject().DestroySelf();

                HeyTeaObject.SpawnHeyTeaObejct(outputHeyTeaObjectSO, this);
            }
        }
        else
        {
            if (player.HasHeyTeaObject())
            {
                if (player.GetHeyTeaObject().TryGetKichenware(out IKichenwareObejct kichenwareObject))
                {
                    if (kichenwareObject.TryAddIngredient(GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)GetHeyTeaObject().GetHeyTeaObjectSO().materialType))
                    {
                        GetHeyTeaObject().DestroySelf();
                    }
                }
                if (this.HasHeyTeaObject())
                {
                    if (this.GetHeyTeaObject().TryGetKichenware(out kichenwareObject))
                    {
                        if (kichenwareObject.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (IKichenwareObejct.MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType))
                        {
                            player.GetHeyTeaObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }

    /**
     * @brief Checks if there is a recipe for the given input object.
     * @param inputHeyTeaObjectSO The input HeyTeaObjectSO.
     * @return True if a recipe exists, otherwise false.
     */
    private bool HasRecipeWithInput(HeyTeaObjectSO inputHeyTeaObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputHeyTeaObjectSO);
        return cuttingRecipeSO != null;
    }

    /**
     * @brief Gets the output HeyTeaObjectSO for a given input HeyTeaObjectSO.
     * @param inputHeyTeaObjectSO The input HeyTeaObjectSO.
     * @return The output HeyTeaObjectSO if a recipe exists, otherwise null.
     */
    private HeyTeaObjectSO GetOutputForInput(HeyTeaObjectSO inputHeyTeaObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputHeyTeaObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    /**
     * @brief Gets the CuttingRecipeSO for a given input HeyTeaObjectSO.
     * @param inputHeyTeaObjectSO The input HeyTeaObjectSO.
     * @return The CuttingRecipeSO if a recipe exists, otherwise null.
     */
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(HeyTeaObjectSO inputHeyTeaObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputHeyTeaObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
