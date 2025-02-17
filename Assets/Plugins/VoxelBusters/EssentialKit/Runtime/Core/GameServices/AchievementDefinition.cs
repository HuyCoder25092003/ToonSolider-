﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Represents an object containing additional information related to game achievement.
    /// </summary>
    /// @ingroup GameServices
    [Serializable]
    public partial class AchievementDefinition
    {
        #region Fields

        [SerializeField]
        private     string                      m_id;

        [SerializeField]
        private     string                      m_platformId;

        [SerializeField]
        private     RuntimePlatformConstantSet  m_platformIdOverrides;

        [SerializeField]
        private     string                      m_title;

        [SerializeField]
        private     int                         m_numOfStepsToUnlock = 1;

        [SerializeField, HideInInspector]
        private     IosPlatformProperties       m_iosProperties;

        [SerializeField, HideInInspector]
        private     AndroidPlatformProperties   m_androidProperties;

        #endregion

        #region Properties

        /// <summary>
        /// The string that identifies the achievement within Unity environment. (read-only)
        /// </summary>
        public string Id => PropertyHelper.GetValueOrDefault(m_id);

        /// <summary>
        /// The name of the achievement. (read-only)
        /// </summary>
        public string Title => PropertyHelper.GetValueOrDefault(m_title);

        /// <summary>
        /// The number of steps required to unlock the achievement.
        /// </summary>
        public int NumOfStepsToUnlock => Math.Max(m_numOfStepsToUnlock, 1);

        public IosPlatformProperties IosProperties => m_iosProperties;

        public AndroidPlatformProperties AndroidProperties => m_androidProperties;

        #endregion

        #region Constructors

        public AchievementDefinition(string id = null, string platformId = null,
            RuntimePlatformConstantSet platformIdOverrides = null, string title = null,
            int numOfStepsToUnlock = 1, IosPlatformProperties iosProperties = null,
            AndroidPlatformProperties androidProperties = null)
        {
            // set default values
            m_id                    = id;
            m_platformId            = platformId;
            m_platformIdOverrides   = platformIdOverrides ?? new RuntimePlatformConstantSet();
            m_title                 = title;
            m_numOfStepsToUnlock    = numOfStepsToUnlock;
            m_iosProperties         = iosProperties ?? new IosPlatformProperties(); 
            m_androidProperties     = androidProperties ?? new AndroidPlatformProperties(); 
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the achievement identifier for active platform.
        /// </summary>
        public string GetPlatformIdForActivePlatform()
        {
            return m_platformIdOverrides.GetConstantForActivePlatform(m_platformId);
        }

        #endregion
    }
}