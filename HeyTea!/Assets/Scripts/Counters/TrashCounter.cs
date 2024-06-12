using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief Class for handling trash disposal in the game.
 * 
 * @details This class extends the BaseCounter class and provides functionality to interact with the player
 *          and destroy any HeyTeaObject that the player is holding, simulating a trash disposal.
 */
public class TrashCounter : BaseCounter
{
    /**
     * @brief Interacts with the player to dispose of HeyTeaObjects.
     * 
     * @details This method overrides the Interact method from the BaseCounter class. 
     *          It checks if the Player is holding a HeyTeaObject, and if so, destroys the object. 
     *          If the object is a PotObject, it calls the DestroySelf method on the PotObject.
     *          Otherwise, it calls the DestroySelf method on the HeyTeaObject.
     * 
     * @param player The Player interacting with the TrashCounter.
     */
    public override void Interact(Player player) {
        if (player.HasHeyTeaObject()) {
            PotObject potObject = player.GetHeyTeaObject() as PotObject;
            if (potObject != null) {
                potObject.DestroySelf();
            } else {
                player.GetHeyTeaObject().DestroySelf();
            }
        }
    }
}
