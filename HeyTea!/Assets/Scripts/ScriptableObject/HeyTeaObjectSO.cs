using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief ScriptableObject that defines the properties of HeyTeaObjects in the game.
 * 
 * @details This class stores the data for HeyTeaObjects, including the prefab, sprite, 
 *          object name, and material type. It also provides a method to clone the object.
 */
[CreateAssetMenu()]
public class HeyTeaObjectSO : ScriptableObject
{
    //can obtain the prefab and sprite in this SO through objectname
    public Transform prefab;    //!< The prefab associated with the HeyTeaObject.
    public Sprite sprite;       //!< The sprite associated with the HeyTeaObject.
    public string objectName;   //!< The name of the HeyTeaObject.

    /**
     * @brief Enumeration of the different types of materials that a HeyTeaObject can have.
     */
    public enum MilkTeaMaterialType {
        none,           //!< pot, cup
        teaBase,        //!< tea, milk, milktea
        basicAdd,       //!< sugar, ice
        fruit,          //!< orange slice, grape slice, strawberry slice
        ingredients,    //!< red bean cooked, pearl cooked
        unTreat,        //!< bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry
    }
    public MilkTeaMaterialType materialType;    //!< The material type of the HeyTeaObject.

    /**
     * @brief Creates a deep copy (clone) of the HeyTeaObject.
     * 
     * @details This method creates a new instance of HeyTeaObjectSO and copies 
     *          the properties of the current object to the new instance.
     * 
     * @return A new instance of HeyTeaObjectSO with the same properties as the current object.
     */
    public object Clone() {
        // Implement deep copy.
        HeyTeaObjectSO heyTeaObjectSOClone = new HeyTeaObjectSO();
        heyTeaObjectSOClone.prefab = prefab;
        heyTeaObjectSOClone.sprite = sprite;
        heyTeaObjectSOClone.objectName = objectName;
        heyTeaObjectSOClone.materialType = materialType;
        return heyTeaObjectSOClone;
    }

}
