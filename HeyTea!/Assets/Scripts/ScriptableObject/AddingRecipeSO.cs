using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class AddingRecipeSO
 * @brief ScriptableObject class for defining adding recipes in the HeyTea game.
 * 
 * This class is used to create and manage adding recipes, which consist of an input HeyTeaObjectSO 
 * and an output HeyTeaObjectSO. These recipes can be used to define the transformation of objects 
 * in the game.
 * 
 * @author Xinyue Cheng
 * @date 05.07.2024
 */

[CreateAssetMenu()]
public class AddingRecipeSO : ScriptableObject
{
    /** 
     * @brief The input HeyTeaObjectSO for the adding recipe.
     */
    public HeyTeaObjectSO input;

    /** 
     * @brief The output HeyTeaObjectSO for the adding recipe.
     */
    public HeyTeaObjectSO output;
}
