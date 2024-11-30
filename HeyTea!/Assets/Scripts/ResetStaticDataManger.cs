
using UnityEngine;
/**
 * @class ResetStaticDataManger
 * @brief Manages the resetting of static data in various counters at the start of the game.
 *
 * @details This class is responsible for calling reset functions on various static data held in 
 * different counters (e.g., CuttingCounter, BaseCounter, TrashCounter). It ensures that any static 
 * data is properly reset when the game starts or when the scene is reloaded.
 * 
 * @author Xinyue Cheng
 * @date 29.09.2024
 */
public class ResetStaticDataManger : MonoBehaviour
{
    /**
     * @brief Called when the script instance is being loaded.
     * 
     * @details This method is executed on the Awake phase, which occurs before the game starts.
     * It ensures that static data in the CuttingCounter, BaseCounter, and TrashCounter is reset 
     * when the game or scene is initialized. This is useful for clearing any lingering data from 
     * previous gameplay sessions.
     */
    private void Awake()
    {
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}

