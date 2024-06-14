using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * @file ContainerCounter.cs
 * @brief Container interaction class
 * 
 * @details This class handles the interaction logic between the ContainerCounter and the Player.
 * The ContainerCounter allows the Player to grab specific objects when the Player has no object in hand.
 * If the Player already has an object, they cannot place it on the ContainerCounter or grab another object.
 * 
 * @date 28.08.2023
 * @author Yong Wu, Xinyue Cheng
 * @last modification date 18.05.2024
 */

public class ContainerCounter : BaseCounter
{
    /** 
     * @brief Event triggered when the player grabs an object.
     */
    public event EventHandler OnPlayerGrabbedObject;

    /**
     * @brief The specific HeyTeaObjectSO that this ContainerCounter spawns.
     */
    [SerializeField] private HeyTeaObjectSO heyTeaObjectSO;

    /**
     * @brief Handles the interaction between the Player and the ContainerCounter.
     * 
     * @details This method defines the interaction logic:
     * - If the player has an object in hand, they cannot place it on the ContainerCounter or grab another object.
     * - If the player has no object in hand, the ContainerCounter spawns a new object and lets the player take it.
     * 
     * @param player The player that interacts with the ContainerCounter.
     */
    public override void Interact(Player player)
    {
        // Player has an object in hand
        if (player.HasHeyTeaObject())
        {
            Debug.Log("Can't put an object on a ContainerCounter and can't grab the object from the ContainerCounter.");
        }
        else
        {
            // Player has no object in hand, spawn a new object and let the player take it
            HeyTeaObject spawnedObject = HeyTeaObject.SpawnHeyTeaObejct(heyTeaObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        return;
    }
}
