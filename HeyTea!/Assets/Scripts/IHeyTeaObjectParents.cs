using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Interface for item acquisition and conversion.
 * @details This interface defines methods for managing the parent relationship of HeyTeaObjects, including setting, getting, and clearing objects.
 * 
 * @date 28.08.2023
 */

/**
 * @interface IHeyTeaObjectParents
 * @brief Interface for managing HeyTeaObjects.
 * 
 * This interface provides methods for managing the parent relationship of HeyTeaObjects, including setting, getting, and clearing objects.
 */
public interface IHeyTeaObjectParents
{
    /**
     * @brief Gets the transform that the HeyTeaObject should follow.
     * @return The transform that the HeyTeaObject should follow.
     */
    public Transform GetHeyTeaObjectFollowTransform();

    /**
     * @brief Sets the HeyTeaObject for this parent.
     * @param heyTeaObject The HeyTeaObject to be set.
     */
    public void SetHeyTeaObject(HeyTeaObject heyTeaObject);

    /**
     * @brief Gets the HeyTeaObject associated with this parent.
     * @return The associated HeyTeaObject.
     */
    public HeyTeaObject GetHeyTeaObject();

    /**
     * @brief Clears the HeyTeaObject from this parent.
     */
    public void ClearHeyTeaObject();

    /**
     * @brief Checks if there is a HeyTeaObject associated with this parent.
     * @return True if there is a HeyTeaObject, otherwise false.
     */
    public bool HasHeyTeaObject();
}
