using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/**
 * @brief Manages the UI display of cup icons.
 * @details This class handles the visual representation of cup icons in the UI, updating the display based on the ingredients added to a cup.
 * 
 * @date 01.09.2023
 */

/**
 * @class CupIconUI
 * @brief UI class for displaying cup icons based on ingredients.
 * 
 * This class subscribes to events from the CupObject to update the UI display of cup icons when ingredients are added.
 */
public class CupIconUI : MonoBehaviour
{
    /**
     * @brief The CupObject that this UI represents.
     */
    [SerializeField] private CupObject cupObject;

    /**
     * @brief The template transform for the icon.
     */
    [SerializeField] private Transform iconTemplate;

    /**
     * @brief Disables the icon template game object on Awake.
     */
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    /**
     * @brief Subscribes to the OnIngredientAdded event of the CupObject.
     */
    private void Start()
    {
        cupObject.OnIngredinetAdded += CupObject_OnIngredientAdded;
    }

    /**
     * @brief Handles the OnIngredientAdded event from the CupObject.
     * @details This method is called when the OnIngredientAdded event is triggered on the CupObject.
     * It updates the visual representation of the cup icons.
     * 
     * @param sender The source of the event.
     * @param e The event data.
     */
    private void CupObject_OnIngredientAdded(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    /**
     * @brief Updates the visual representation of the cup icons.
     * @details This method destroys the existing icons (except for the template) and creates new icons based on the current ingredients in the cup.
     */
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        var sortList = cupObject.GetMilkTeaMaterialQuota().OrderBy(t => t.milkTeaMaterialType);

        foreach (IKichenwareObejct.MilkTeaMaterialQuota milkTeaMaterialQuota in sortList)
        {
            foreach (IKichenwareObejct.HeyTeaObejctStruct heyTeaObejctStruct in milkTeaMaterialQuota.heyTeaObejctStructArray)
            {
                for (int i = 0; i < heyTeaObejctStruct.currentNum; i++)
                {
                    Transform iconTransform = Instantiate(iconTemplate, transform);
                    iconTransform.gameObject.SetActive(true);
                    iconTransform.GetComponent<CupIconSingleUI>().SetHeyTeaObjectSO(heyTeaObejctStruct.heyTeaObjectSO);
                }
            }
        }
    }
}
