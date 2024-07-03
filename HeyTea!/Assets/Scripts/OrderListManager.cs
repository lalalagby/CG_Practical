using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

/**
 * @brief Manages the list of orders in the game.
 * 
 * @details This class is responsible for generating new orders at regular intervals, 
 *          tracking the list of waiting orders, and verifying if the delivered order matches
 *          any of the waiting orders. It triggers events when an order is spawned or completed.
 *          
 * @author Bingyu Guo
 * @date 2024-06-30
 */

public class OrderListManager : MonoBehaviour
{
    public event EventHandler OnOrderSpawned;       //!< Event triggered when a new order is spawned.
    public event EventHandler OnOrderCompleted;     //!< Event triggered when an order is completed.

    public static OrderListManager Instance { get; private set; }   //!< Singleton instance of the OrderListManager class.

    [SerializeField] private OrderListSO orderListSO;       //!< Contains the list of possible orders.

    private List<OrderSO> waitingOrderSOList;       //!< List of orders currently generated.
    private float spawnOrderTimer;      //!< Timer for spawning new orders.
    private float spawnOrderInterval = 2f; //!< Interval for generating new orders
    private int waitingOrdersMax = 3;   //!< Maximum number of orders that can be waiting to be delivered.
    private int ordersGenerated = 0;    //!< Total number of orders that have been generated.


    private void Awake() {
        Instance = this;
        waitingOrderSOList = new List<OrderSO>();
    }

    /**
     * @brief Updates the order generation timer and spawns new orders 
     *        if there is less than three orders in the waitingOrderSOList.
     */
    private void Update() {
        // Only generate a new order when the number of the waiting order list is less than 3.
        spawnOrderTimer -= Time.deltaTime;

        if (spawnOrderTimer <= 0f) {
            spawnOrderTimer += spawnOrderInterval;

            if (waitingOrderSOList.Count < waitingOrdersMax) {
                OrderSO newOrder = orderListSO.orderSOList[UnityEngine.Random.Range(0, orderListSO.orderSOList.Count)];
                print("new order£º" + newOrder.orderName);

                waitingOrderSOList.Add(newOrder);
                ordersGenerated++;
                OnOrderSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /**
     * @brief Delivers an order and checks if it matches any waiting orders.
     * 
     * @param cupObject The delivered CupObject to check against the waiting orders.
     */
    public void DeliverOrder(CupObject cupObject)  {
        foreach (OrderSO waitingOrderSO in waitingOrderSOList) {
            // The order is matched successfully.
            if (IsMatchingOrder(waitingOrderSO, cupObject)) {
                print("Player delivered the correct order!");

                // Remove the order from the waiting order list.
                waitingOrderSOList.Remove(waitingOrderSO);
                OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        print("Player did not deliver a correct order!");
    }


    /**
     * @brief Determines whether the delivered order matches the currently acquired order.
     * 
     * @param order The order to check.
     * @param cupObject The delivered CupObject to check against the order.
     * @return True if the delivered order matches the waiting order, false otherwise.
     */
    private bool IsMatchingOrder(OrderSO order, CupObject cupObject) {
        List<HeyTeaObjectSO> cupObjects = cupObject.GetOutputHeyTeaObejctSOList();

        // If two orders contain different quantities of items.
        if (order.heyTeaObjectSOLists.Count != cupObjects.Count) {
            return false;
        }

        // Determine if the items included in the order can be matched up
        foreach (HeyTeaObjectSO orderObject in order.heyTeaObjectSOLists)  {
            if (!cupObjects.Contains(orderObject)) {
                return false;
            }
        }
        return true;
    }


    public void SetOrderListSO(OrderListSO orderListSO) { this.orderListSO = orderListSO; }

    public void SetWaitingOrderSOList(List<OrderSO> waitingOrderSOList) { this.waitingOrderSOList = waitingOrderSOList; }
    
    public List<OrderSO> GetWaitingOrderSOList() { return waitingOrderSOList; }

    public void SetOrdersGenerated() { ordersGenerated++; }

    public int GetOrdersGenerated() { return ordersGenerated; }
}