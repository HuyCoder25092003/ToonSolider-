﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information related to <see cref="CloudServices.OnSynchronizeComplete"/> event.
    /// </summary>
    /// @ingroup CloudServices
    public class CloudServicesSynchronizeResult
    {
        #region Properties

        /// <summary>
        /// The value indicates whether synchronize request was successful.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; private set; }

        #endregion

        #region Constructors

        internal CloudServicesSynchronizeResult(bool success)
        {
            // Set properties
            Success = success;
        }

        #endregion
    }
}