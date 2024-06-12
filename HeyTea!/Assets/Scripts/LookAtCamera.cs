using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Yong Wu
 * 
 * @brief Defines how the cutting progress bar is displayed by orienting towards the camera.
 * 
 * @details The LookAtCamera class manages the orientation of the progress bar or other UI elements to
 *          ensure they face the camera correctly based on different modes.
 */
public class LookAtCamera : MonoBehaviour
{
    /**
     * @brief Enumeration for the different modes of orientation.
     */
    private enum Mode {
        LookAt,             //!< Face directly towards the camera.
        LokkAtInverted,     //!< Face directly away from the camera.
        CameraForward,      //!< Align with the camera's forward direction.
        CameraForwardInverted,  //!< Align with the opposite of the camera's forward direction.
    }

    [SerializeField] private Mode mode;     //!< The mode of orientation for the UI element.

    /**
     * @brief Updates the orientation of the UI element late in the frame.
     * 
     * @details This method is called once per frame, after all Update functions have been called. 
     *          It updates the orientation of the UI element based on the selected mode 
     *          to ensure it faces the camera correctly.
     */
    private void LateUpdate() {
        //It can be set to switch as the camera changes, or it can be set to keep facing straight ahead.
        switch (mode) {
            case Mode.LookAt: transform.LookAt(Camera.main.transform);break;
            case Mode.LokkAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
        
    }

}
