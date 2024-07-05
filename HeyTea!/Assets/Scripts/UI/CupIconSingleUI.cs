using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @brief Handles the UI display of a single cup icon.
 * @details This class manages the display of a cup icon in the UI by setting the appropriate sprite image.
 * 
 * @date 01.09.2023
 */

/**
 * @class CupIconSingleUI
 * @brief UI class for displaying a single cup icon.
 * 
 * This class is responsible for setting the sprite of an Image component to represent a HeyTeaObjectSO in the UI.
 */
public class CupIconSingleUI : MonoBehaviour
{
    /**
     * @brief The Image component used to display the cup icon.
     */
    [SerializeField] private Image image;

    /**
     * @brief Sets the sprite of the Image component based on the given HeyTeaObjectSO.
     * @param heyTeaObjectSO The HeyTeaObjectSO containing the sprite to be displayed.
     */
    public void SetHeyTeaObjectSO(HeyTeaObjectSO heyTeaObjectSO)
    {
        image.sprite = heyTeaObjectSO.sprite;
    }
}

