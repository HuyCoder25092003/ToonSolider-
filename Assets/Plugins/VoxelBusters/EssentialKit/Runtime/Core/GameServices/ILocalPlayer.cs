﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides interface to access information about the authenticated player running your game on the device. 
    /// </summary>
    /// <description>
    /// At any given time, only one user may be authenticated on the device, this user must log out before another user can log in.
    /// </description>
    /// @remark Your game must authenticate the local user before using any features.
    /// @ingroup GameServices
    public interface ILocalPlayer : IPlayer
    {
        #region Properties

        /// <summary>
        /// A bool value that indicates whether a local player is currently signed in to game service. (read-only)
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// A bool value that indicates whether a local player is underage. (read-only)
        /// </summary>
        /// <value><c>true</c> if is under age; otherwise, <c>false</c>.</value>
        bool IsUnderAge { get; }

        #endregion

        #region Friends Methods

        /// <summary>
        /// Loads the list of friends for the authenticated local player.
        /// </summary>
        /// <param name="callback">The callback that will be called after operation is completed.
        /// If successful, it will contain the list of friends.</param>
        void LoadFriends(EventCallback<GameServicesLoadPlayerFriendsResult> callback);

        /// <summary>
        /// Initiates a friend request to the specified player.
        /// </summary>
        /// <param name="playerId">The id of the player to send the request to.</param>
        /// <param name="callback">The callback that will be called after operation is completed.
        /// </param>
        void AddFriend(string playerId, EventCallback<bool> callback);

        #endregion
    }
}