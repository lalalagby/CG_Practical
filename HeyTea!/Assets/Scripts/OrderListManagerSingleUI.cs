//using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/**
 * @brief Manages the UI for displaying a single order.
 * 
 * @details This class is responsible for updating the visual representation of a single order.
 *          It sets the order name and icons based on the provided OrderSO.
 *          
 * @author Bingyu Guo
 * @date 2024-07-02
 */
public class OrderListManagerSingleUI : MonoBehaviour
{
    [SerializeField] private Text orderNameText;    //!< The Text component for displaying the order name.
    [SerializeField] private Transform iconContainer;   //!< The container Transform for the icons.
    [SerializeField] private Transform iconTemplate;    //!< The template Transform for creating new icons.


    /**
     * @brief Initializes the icon template.
     * 
     * @details This method hides the icon template at the start.
     */
    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    /**
     * @brief Sets the OrderSO and updates the UI.
     * 
     * @details This method sets the order name and creates icons for each HeyTeaObjectSO in the order.
     *          If the HeyTeaObjectSO's name is "LiquidMilktea", it should add separate icons for milk and tea.
     * 
     * @param orderSO The OrderSO to display.
     */
    public void SetOrderSO(OrderSO orderSO)  {
        orderNameText.text = orderSO.orderName;

        // Clear existing icons
        foreach (Transform child in iconContainer) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // Create new icons for each HeyTeaObjectSO in the order
        foreach (HeyTeaObjectSO heyTeaObjectSO in orderSO.heyTeaObjectSOLists) {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);

            if (heyTeaObjectSO.objectName == "LiquidMilktea") {
                HeyTeaObjectSO heyTeaSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilk.asset");     
                iconTransform.GetComponent<Image>().sprite = heyTeaSO.sprite;

                Transform iconTransform2 = Instantiate(iconTemplate, iconContainer);
                iconTransform2.gameObject.SetActive(true);
                heyTeaSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
                iconTransform2.GetComponent<Image>().sprite = heyTeaSO.sprite;
            } else {
                iconTransform.GetComponent<Image>().sprite = heyTeaObjectSO.sprite;
            }

        }
    }
}