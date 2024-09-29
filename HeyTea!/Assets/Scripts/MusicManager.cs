using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Bingyu Guo
 * 
 * @date 2024-09-10
 * 
 * @brief Manages background music in the game, including volume control and persistence.
 * 
 * @details This class controls the music volume, allows users to adjust it, and saves the volume setting
 *          using Unity's PlayerPrefs for future sessions. It follows a singleton pattern, ensuring only 
 *          one instance of the MusicManager exists at any time.
 */
public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume"; //!< Key used to save the music volume in PlayerPrefs.

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;    //!< The AudioSource component responsible for playing the music.
    private float volume = 0.3f;        //!< The current music volume, with a default value of 0.3.

    /**
     * @brief Initializes the singleton instance and loads the saved music volume.
     * 
     * @details Called when the script instance is being loaded. This method retrieves the saved 
     *          music volume from PlayerPrefs and applies it to the AudioSource. If no saved value 
     *          exists, it uses the default volume of 0.3.
     */
    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.3f);
        audioSource.volume = volume;
    }

    /**
     * @brief Increases the music volume in steps and wraps it around to zero when exceeding the maximum.
     * 
     * @details This method increases the volume by 0.1 each time it is called. If the volume exceeds 1.0, 
     *          it wraps back to 0.0. The new volume is saved in PlayerPrefs for future sessions.
     */
    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    /**
     * @brief Retrieves the current music volume.
     * 
     * @return The current music volume as a float value between 0.0 and 1.0.
     */
    public float GetVolume()
    {
        return volume;
    }
}
