using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief ScriptableObject for defining recipes in the HeyTea game.
 * 
 * @details This ScriptableObject holds a list of HeyTeaObjectSO objects that are part of the recipe,
 * as well as the name of the recipe. It is used to define the ingredients and the name for various
 * recipes in the HeyTea game.
 * 
 * @date 30.06.2024
 * @author Xinyue Cheng
 */
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    /**
     * @brief List of HeyTeaObjectSO objects that make up the recipe.
     * 
     * @details This list contains the ingredients required for the recipe. Each element in the list
     * is a HeyTeaObjectSO that represents an ingredient.
     */
    public List<HeyTeaObjectSO> heyTeaObjectSOLists;

    /**
     * @brief The name of the recipe.
     * 
     * @details This string holds the name of the recipe. It can be used to identify the recipe in the game.
     */
    public string recipeName;
}
