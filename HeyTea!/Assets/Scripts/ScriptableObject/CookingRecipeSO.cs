using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief ScriptableObject that defines a cooking recipe in the game.
 * 
 * This class stores the data for a cooking recipe, including the input, mid-state, output, and maximum cooking time.
 */
[CreateAssetMenu()]
public class CookingRecipeSO :ScriptableObject
{
    public HeyTeaObjectSO input;    //!< The input HeyTeaObjectSO required for the recipe.
    public HeyTeaObjectSO midState; //!< The mid-state HeyTeaObjectSO during the cooking process.
    public HeyTeaObjectSO output;   //!< The output HeyTeaObjectSO produced by the recipe.
    public float cookingTimerMax;   //!< The maximum cooking time for the recipe.
}
