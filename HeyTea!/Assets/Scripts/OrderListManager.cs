using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OrderListManager : MonoBehaviour
{
    public static OrderListManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float orderGenerationInterval = 3f; // Interval for generating new orders

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    //private float spawnRecipeTimerMax = 3f;
    private int waitingRecipesMax = 3;
    private int totalNumOrders = 5;
    private int ordersGenerated = 0;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    public void Start()
    {
        // Start generating orders
        for (int i = 0; i < 3 && ordersGenerated < totalNumOrders; i++)
        {
            AddNewRecipe();
        }
    }

    private void AddNewRecipe()
    {
        //if (ordersGenerated >= totalNumOrders) return;

        RecipeSO newRecipe = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
        Debug.Log("生成新订单：" + newRecipe.recipeName);

        waitingRecipeSOList.Add(newRecipe);
        ordersGenerated++;
    }

    private void Update()
    {
        if (waitingRecipeSOList.Count < 3 && ordersGenerated < totalNumOrders)
        {
            spawnRecipeTimer -= Time.deltaTime;

            if (spawnRecipeTimer <= 0f)
            {
                spawnRecipeTimer += orderGenerationInterval;
                AddNewRecipe();
            }
        }
    }

    public void DeliverOrder(CupObject cupObject)
    {
        int i = 0;
        foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList)
        {
            print("i: " + i);
            // 订单匹配成功
            if (IsMatchingRecipe(waitingRecipeSO, cupObject))
            {

                Debug.Log("Player delivered the correct recipe!");
                // 删除订单
                waitingRecipeSOList.Remove(waitingRecipeSO);
                Debug.Log("删除订单：" + waitingRecipeSOList.Count);                

                // 添加新订单
                if (ordersGenerated < totalNumOrders)
                {
                    AddNewRecipe();
                }
                Debug.Log("订单：" + ordersGenerated + "," + totalNumOrders + "," + waitingRecipeSOList.Count);
                return;
            }
        }
        Debug.Log("Player did not deliver a correct recipe!");
    }

    // 判断提交的订单是否和当前取得的订单匹配
    private bool IsMatchingRecipe(RecipeSO recipe, CupObject cupObject)
    {
        List<HeyTeaObjectSO> cupObjects = cupObject.GetOutputHeyTeaObejctSOList();

        // 如果两个订单包含的物品的数量不一致
        if (recipe.heyTeaObjectSOLists.Count != cupObjects.Count)
        {
            Debug.Log("数量不一致");
            return false;
        }

        // 判断订单中包含的物品是否能匹配上
        foreach (HeyTeaObjectSO recipeObject in recipe.heyTeaObjectSOLists)  {
            print(recipeObject + ", " + cupObjects.Contains(recipeObject));
            if (!cupObjects.Contains(recipeObject)) {
                return false;
            }
        }
        return true;
    }
    public void SetRecipeListSO(RecipeListSO recipeListSO) { this.recipeListSO = recipeListSO; }

    public RecipeListSO GetRecipeListSO() { return this.recipeListSO; }

    public void SetWaitingRecipeSOList(List<RecipeSO> waitingRecipeSOList) { this.waitingRecipeSOList = waitingRecipeSOList; }
    public List<RecipeSO> GetWaitingRecipeSOList() { return waitingRecipeSOList; }

    public void SetOrdersGenerated() { ordersGenerated++; }
    public int GetOrdersGenerated() { return ordersGenerated; }
    public int GetWaitingRecipeNum() { return waitingRecipeSOList.Count; }
}