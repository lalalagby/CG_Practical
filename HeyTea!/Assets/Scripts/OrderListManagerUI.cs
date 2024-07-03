using System;
//using TMPro;
using Unity;
using UnityEngine;

public class OrderListManagerUI : MonoBehaviour
{
    //[SerializeField] private Transform container;
    //[SerializeField] private Transform orderTemplate;
    //[SerializeField] private TextMeshProUGUI orderNameText;


    //private void Awake()
    //{
    //    orderTemplate.gameObject.SetActive(false);
    //}

    //private void Start()
    //{
    //    OrderListManager.Instance.OnOrderSpawned += OrderListManager_OnOrderSpawned;
    //    OrderListManager.Instance.OnOrderCompleted += OrderListManager_OnOrderCompleted;
    //    UpdateVisual();
    //}

    //private void OrderListManager_OnOrderSpawned(object sender, EventArgs e)
    //{
    //    UpdateVisual();
    //}

    //private void OrderListManager_OnOrderCompleted(object sender, EventArgs e)
    //{
    //    UpdateVisual();
    //}

    //public void setOrderSO(OrderSO orderSO)
    //{
    //    orderNameText.text = orderSO.orderName;
    //}

    //private void UpdateVisual()
    //{
    //    foreach (Transform child in container)
    //    {
    //        if (child == orderTemplate) continue;
    //        Destroy(child.gameObject);
    //    }

    //    foreach (OrderSO orderSO in OrderListManager.Instance.GetWaitingOrderSOList())
    //    {
    //        Transform orderTransform = Instantiate(orderTemplate, container);
    //        orderTransform.gameObject.SetActive(true);
    //        orderTransform.GetComponent<OrderListManagerSingleUI>().setOrderSO(orderSO);
    //    }
    //}

}