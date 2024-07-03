using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;



public class OrderListManager : MonoBehaviour
{
    public static OrderListManager Instance { get; private set; }

    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderCompleted;


    [SerializeField] private OrderListSO orderListSO;
    [SerializeField] private float spawnOrderInterval = 3f; // Interval for generating new orders

    private List<OrderSO> waitingOrderSOList;
    private float spawnOrderTimer;
    private int waitingOrdersMax = 3;
    private int ordersGenerated = 0;



    private void Awake() {
        Instance = this;
        waitingOrderSOList = new List<OrderSO>();
    }

    private void Update() {
        // Only generate a new order when the number of the waiting order list is less than 3.
        spawnOrderTimer -= Time.deltaTime;

        if (spawnOrderTimer <= 0f) {
            print("spawnOrderTimer is: " + spawnOrderTimer);

            spawnOrderTimer += spawnOrderInterval;

            if (waitingOrderSOList.Count < waitingOrdersMax) {
                OrderSO newOrder = orderListSO.orderSOList[UnityEngine.Random.Range(0, orderListSO.orderSOList.Count)];
                Debug.Log("生成新订单：" + newOrder.orderName);

                waitingOrderSOList.Add(newOrder);
                ordersGenerated++;
                OnOrderSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverOrder(CupObject cupObject)  {
        int i = 0;
        foreach (OrderSO waitingOrderSO in waitingOrderSOList) {
            print("i: " + i);
            // The order is matched successfully.
            if (IsMatchingOrder(waitingOrderSO, cupObject)) {
                print("Player delivered the correct order!");

                // Remove the order from the waiting order list.
                waitingOrderSOList.Remove(waitingOrderSO);

                print("删除订单：" + waitingOrderSOList.Count);
                OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        print("Player did not deliver a correct order!");
    }

    // Determine whether the delivered order matches the currently acquired order.
    private bool IsMatchingOrder(OrderSO order, CupObject cupObject) {
        List<HeyTeaObjectSO> cupObjects = cupObject.GetOutputHeyTeaObejctSOList();

        // If two orders contain different quantities of items.
        if (order.heyTeaObjectSOLists.Count != cupObjects.Count) {
            return false;
        }

        // Determine if the items included in the order can be matched up
        foreach (HeyTeaObjectSO orderObject in order.heyTeaObjectSOLists)  {
            print(orderObject + ", " + cupObjects.Contains(orderObject));
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
    //public int GetWaitingOrderNum() { return waitingOrderSOList.Count; }
}