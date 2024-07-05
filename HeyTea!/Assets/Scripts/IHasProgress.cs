using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Interface for handling progress-based operations.
 * @details This interface defines the structure for objects that can track and report progress, providing an event for progress updates.
 * 
 * @date 28.08.2023
 * @author Yong Wu
 */

/**
 * @interface IHasProgress
 * @brief Interface for objects that have progress tracking capabilities.
 * 
 * This interface defines the structure for objects that can track and report progress, providing an event for progress updates.
 */
public interface IHasProgress
{
    /**
     * @brief Event triggered when progress changes.
     * @details This event is triggered whenever the progress of an operation changes. It provides the normalized progress value and processing state.
     */
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    /**
     * @brief Arguments for the OnProgressChanged event.
     * @details This class contains the arguments for the OnProgressChanged event, including the normalized progress value and processing state.
     */
    public class OnProgressChangedEventArgs : EventArgs
    {
        /**
         * @brief Normalized progress value.
         * @details This value represents the progress of an operation, normalized between 0 and 1.
         */
        public float progressNormalized;

        /**
         * @brief Indicates if the operation is currently processing.
         * @details This boolean value indicates whether the operation is actively processing (true) or not (false).
         */
        public bool isProcessing;
    }
}
