using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief ScriptableObject that defines a cutting recipe in the game.
 * 
 * @details This class stores the data for a cutting recipe, including the input item, output item, and the maximum cutting progress.
 */
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{

    public HeyTeaObjectSO input;    //!< The input HeyTeaObjectSO required for the cutting recipe.
    public HeyTeaObjectSO output;   //!< The output HeyTeaObjectSO produced by the cutting recipe.
    public int cuttingProgressMax;  //!< The maximum cutting progress required to complete the recipe.
}
