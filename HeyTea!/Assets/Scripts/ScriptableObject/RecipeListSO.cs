using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief ScriptableObject for managing a list of recipes in the HeyTea game.
 * 
 * @details This ScriptableObject holds a list of RecipeSO objects, allowing for easy management
 * and access to multiple recipes within the game. Each RecipeSO in the list represents a different
 * recipe that can be utilized in the HeyTea game.
 * 
 * @date 30.06.2024
 * @author Xinyue Cheng
 */
public class RecipeListSO : ScriptableObject
{
    /**
     * @brief List of RecipeSO objects.
     * 
     * @details This list contains multiple RecipeSO objects, each representing a different recipe
     * in the HeyTea game. It allows for centralized management of all recipes.
     */
    public List<RecipeSO> recipeSOList;
}

