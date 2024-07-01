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
        Debug.Log("�����¶�����" + newRecipe.recipeName);

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
            // ����ƥ��ɹ�
            if (IsMatchingRecipe(waitingRecipeSO, cupObject))
            {

                Debug.Log("Player delivered the correct recipe!");
                // ɾ������
                waitingRecipeSOList.Remove(waitingRecipeSO);
                Debug.Log("ɾ��������" + waitingRecipeSOList.Count);                

                // ����¶���
                if (ordersGenerated < totalNumOrders)
                {
                    AddNewRecipe();
                }
                Debug.Log("������" + ordersGenerated + "," + totalNumOrders + "," + waitingRecipeSOList.Count);
                return;
            }
        }
        Debug.Log("Player did not deliver a correct recipe!");
    }

    // �ж��ύ�Ķ����Ƿ�͵�ǰȡ�õĶ���ƥ��
    private bool IsMatchingRecipe(RecipeSO recipe, CupObject cupObject)
    {
        List<HeyTeaObjectSO> cupObjects = cupObject.GetOutputHeyTeaObejctSOList();

        // �������������������Ʒ��������һ��
        if (recipe.heyTeaObjectSOLists.Count != cupObjects.Count)
        {
            Debug.Log("������һ��");
            return false;
        }

        // �ж϶����а�������Ʒ�Ƿ���ƥ����
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



//public class OrderListManager : MonoBehaviour
//{
//    public static OrderListManager Instance { get; private set; }

//    [SerializeField] private RecipeListSO recipeListSO;

//    private List<RecipeSO> waitingRecipeSOList;
//    private float spawnRecipeTimer;
//    private float spawnRecipeTimerMax = 4f;
//    private int waitingRecipesMax = 4;
//    private int ordersGenerated = 0;

//    private void Awake()
//    {
//        Instance = this;
//        waitingRecipeSOList = new List<RecipeSO>();
//    }

//    private void Update()
//    {
//        spawnRecipeTimer -= Time.deltaTime;
//        if (spawnRecipeTimer <= 0f)
//        {
//            spawnRecipeTimer += spawnRecipeTimerMax;

//            if (waitingRecipeSOList.Count < waitingRecipesMax)
//            {
//                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
//                Debug.Log(waitingRecipeSO.recipeName);
//                waitingRecipeSOList.Add(waitingRecipeSO);
//            }
//        }
//    }

//    public void DeliverOrder(CupObject cupObject)
//    {
//        for (int i = 0; i < waitingRecipeSOList.Count; i++)
//        {
//            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
//            if (waitingRecipeSO.heyTeaObjectSOLists.Count == cupObject.GetOutputHeyTeaObejctSOList().Count)
//            {
//                // �����еĲ�Ʒ������ȥ�Ĳ�Ʒ��ͬ����������Ʒ���
//                bool plateContentsMatchesRecipe = true;
//                foreach (HeyTeaObjectSO recipeKitchenObjectSO in waitingRecipeSO.heyTeaObjectSOLists)
//                {
//                    // ���������еĲ�Ʒ����ɵ���Ʒ
//                    bool ingredientFound = false;
//                    foreach (HeyTeaObjectSO plateKitchenObjectSO in cupObject.GetOutputHeyTeaObejctSOList())
//                    {
//                        // ��������ȥ�Ĳ�Ʒ����ɵ���Ʒ
//                        if (recipeKitchenObjectSO == plateKitchenObjectSO)
//                        {
//                            // �����еĲ�Ʒ������ȥ�Ĳ�Ʒ��ͬ������Ʒ���
//                            ingredientFound = true;
//                            break;
//                        }
//                    }
//                    if (!ingredientFound)
//                    {
//                        // �����еĲ�Ʒ������ȥ�Ĳ�Ʒ�ɲ�ͬ����Ʒ���
//                        plateContentsMatchesRecipe = false;
//                    }
//                }

//                if (plateContentsMatchesRecipe)
//                {
//                    Debug.Log("Player delivered the correct recipe!");
//                    waitingRecipeSOList.RemoveAt(i);
//                    return;
//                }
//            }
//        }
//        // ���������ж�����û���ҵ�ƥ��Ķ���
//        Debug.Log("Player did not deliver a correct recipe!");
//    }

//    public void SetRecipeListSO(RecipeListSO recipeListSO) { this.recipeListSO = recipeListSO; }

//    public RecipeListSO GetRecipeListSO() { return this.recipeListSO; }

//    public List<RecipeSO> GetWaitingRecipeSOList() { return waitingRecipeSOList; }

//    public void SetWaitingRecipeSOList(List<RecipeSO> waitingRecipeSOList) { this.waitingRecipeSOList = waitingRecipeSOList;  }

//    public int GetOrdersGenerated() { return ordersGenerated; }
//}