using System.Collections.Generic;
using UnityEngine;

/**
 * @author Bingyu Guo
 * @date 2024-06-30
 * 
 * @brief ScriptableObject representing an order in the game.
 * 
 * @details This class contains a list of HeyTeaObjectSO objects that define the contents of the order,
 *          as well as the name of the order.
 */
[CreateAssetMenu()]
public class OrderSO : ScriptableObject {

    /**
     * @brief List of HeyTeaObjectSO objects that make up the order.
     */
    public List<HeyTeaObjectSO> heyTeaObjectSOLists;

    public string orderName;    //!< The name of the order.
}