using UnityEngine;

/**
 * @author Bingyu Guo
 * 
 * @date 2024-09-10
 * 
 * @brief A ScriptableObject that holds references to various audio clips used in the game.
 * 
 * @details This class provides arrays of audio clips for different in-game events, such as chopping, 
 *          delivering success or failure, footsteps, object pickups, and more. It is used by the 
 *          SoundManager to play these sound effects during gameplay.
 */
[CreateAssetMenu]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] chop;             //!< Array of audio clips for chopping sounds.
    public AudioClip[] deliveryFail;     //!< Array of audio clips for delivery failure sounds.
    public AudioClip[] deliverySuccess;  //!< Array of audio clips for delivery success sounds.
    public AudioClip[] footstep;         //!< Array of audio clips for footstep sounds.
    public AudioClip[] objectDrop;       //!< Array of audio clips for object dropping sounds.
    public AudioClip[] objectPickup;     //!< Array of audio clips for object pickup sounds.
    public AudioClip stoveSizzle;        //!< Single audio clip for stove sizzling sound.
    public AudioClip[] trash;            //!< Array of audio clips for trashing sounds.
}