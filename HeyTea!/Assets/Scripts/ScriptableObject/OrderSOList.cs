using System.Collections.Generic;
using UnityEngine;


/**
 * @author Bingyu Guo
 * 
 * @date 2024-06-30
 * 
 * @brief ScriptableObject for managing a list of orders.
 * 
 * @details This class contains a list of OrderSO objects that represent all available orders in the game.
 */
[CreateAssetMenu()]
public class OrderListSO : ScriptableObject{
    public List<OrderSO> orderSOList;   //!< List of OrderSO objects representing the available orders.
}