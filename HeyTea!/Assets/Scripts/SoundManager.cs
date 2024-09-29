using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * @author Bingyu Guo
 * 
 * @date 2024-09-10
 * 
 * @brief Manages sound effects in the game.
 * 
 * @details This class handles the playback of various sound effects triggered by in-game events,
 *          such as cutting ingredients, delivering recipes, and picking up objects. It also allows
 *          users to adjust the sound effects volume and stores the preferences in player settings.
 */
public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";  //!< Player preference key for sound effects volume.
    public static SoundManager Instance { get; private set; }       //!< Singleton instance of the SoundManager.

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;       //!< Reference to the AudioClipRefsSO scriptable object that holds audio clips.

    private float volume = 1f;      //!< Default sound effects volume.

    /**
     * @brief Initializes the singleton instance and loads sound effects volume from player preferences.
     */
    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    /**
     * @brief Subscribes to various game events to play corresponding sound effects.
     */
    private void Start()
    {
        OrderListManager.Instance.OnRecipeSuccess += OrderListManager_OnRecipeSuccess;
        OrderListManager.Instance.OnRecipeFailed += OrderListManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickUpSomething += Player_OnPickUpSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    /**
     * @brief Plays success sound effect when a recipe is successfully delivered.
     * 
     * @param sender The event sender.
     * @param e Event arguments.
     */
    private void OrderListManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    /**
     * @brief Plays failure sound effect when a recipe delivery fails.
     * 
     * @param sender The event sender.
     * @param e Event arguments.
     */
    private void OrderListManager_OnRecipeFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    /**
     * @brief Plays chopping sound effect when any object is cut.
     * 
     * @param sender The CuttingCounter that triggered the event.
     * @param e Event arguments.
     */
    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    /**
     * @brief Plays sound effect when the player picks up an object.
     * 
     * @param sender The event sender.
     * @param e Event arguments.
     */
    private void Player_OnPickUpSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    /**
     * @brief Plays sound effect when an object is placed on a base counter.
     * 
     * @param sender The BaseCounter that triggered the event.
     * @param e Event arguments.
     */
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    /**
     * @brief Plays sound effect when an object is placed on a base counter.
     * 
     * @param sender The BaseCounter that triggered the event.
     * @param e Event arguments.
     */
    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    /**
     * @brief Plays footstep sound at the given position.
     * 
     * @param position The world position where the footstep sound should play.
     * @param volume The volume of the footstep sound.
     */
    public void PlayFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    /**
     * @brief Plays a random sound from a list of AudioClips at a specified position and volume.
     * 
     * @param audioClipArray The array of AudioClips to choose from.
     * @param position The world position where the sound should play.
     * @param volume The volume of the sound.
     */
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    /**
     * @brief Plays an AudioClip at a specified position and volume.
     * 
     * @param audioClip The AudioClip to play.
     * @param position The world position where the sound should play.
     * @param volume The volume of the sound.
     */
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    /**
     * @brief Changes the volume of sound effects and saves the new volume to player preferences.
     */
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    /**
     * @brief Returns the current volume of sound effects.
     * 
     * @return The current sound effects volume.
     */
    public float GetVolume()
    {
        return volume;
    }
}
