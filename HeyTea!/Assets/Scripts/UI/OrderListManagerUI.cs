using System;
using UnityEngine;

/**
 * @brief Manages the UI for displaying the list of orders.
 * 
 * @details This class is responsible for updating the visual representation of the current orders.
 *          It listens for order-related events and updates the UI accordingly.
 * 
 * @author Bingyu Guo
 * @date 2024-07-02
 */

public class OrderListManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;   //!< The container Transform where order UI elements will be instantiated.
    [SerializeField] private Transform orderTemplate;   //!< The template Transform for creating new order UI elements.


    /**
     * @brief Initializes the order template and subscribes to order-related events.
     */
    private void Awake()
    {
        orderTemplate.gameObject.SetActive(false);
    }

    /**
     * @brief Subscribes to the OrderListManager events and updates the UI.
     */
    private void Start()
    {
        OrderListManager.Instance.OnOrderSpawned += OrderListManager_OnOrderSpawned;
        OrderListManager.Instance.OnOrderCompleted += OrderListManager_OnOrderCompleted;
        UpdateVisual();
    }


    /**
     * @brief Event handler for when a new order is spawned.
     * 
     * @details This method updates the UI to reflect the new order.
     * 
     * @param sender The source of the event.
     * @param e The event arguments.
     */
    private void OrderListManager_OnOrderSpawned(object sender, EventArgs e) {
        UpdateVisual();
    }

    /**
     * @brief Event handler for when an order is completed.
     * 
     * @details This method updates the UI to reflect the completed order.
     * 
     * @param sender The source of the event.
     * @param e The event arguments.
     */
    private void OrderListManager_OnOrderCompleted(object sender, EventArgs e) {
        UpdateVisual();
    }

    /**
     * @brief Updates the visual representation of the current orders.
     * 
     * @details This method clears the current UI elements and instantiates new ones based on the 
     *          current list of waiting orders.
     */
    private void UpdateVisual() {
        // Clear existing order UI elements
        foreach (Transform child in container) {
            if (child == orderTemplate) continue;
            Destroy(child.gameObject);
        }

        // Create new order UI elements
        foreach (OrderSO orderSO in OrderListManager.Instance.GetWaitingOrderSOList())  {
            Transform orderTransform = Instantiate(orderTemplate, container);
            orderTransform.gameObject.SetActive(true);
            orderTransform.GetComponent<OrderListManagerSingleUI>().SetOrderSO(orderSO);
        }
    }

}