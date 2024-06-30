using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Handles interactions with the delivery counter in the HeyTea game.
 * 
 * @details The DeliveryCounter class extends the BaseCounter class and provides functionality for
 * handling player interactions with the delivery counter. When the player interacts with the delivery
 * counter while holding a CupObject, the CupObject is destroyed.
 * 
 * @date 29.06.2024
 * @author Xinyue Cheng
 */
public class DeliveryCounter : BaseCounter
{
    /**
     * @brief Interacts with the player.
     * 
     * @details This method handles the interaction between the player and the delivery counter. If the
     * player is holding a CupObject, the CupObject is destroyed upon interaction.
     * 
     * @param player The player interacting with the delivery counter.
     */
    public override void Interact(Player player)
    {
        if (player.GetHeyTeaObject() is CupObject)
        {
            player.GetHeyTeaObject().DestroySelf();
        }
        return;
    }
}

