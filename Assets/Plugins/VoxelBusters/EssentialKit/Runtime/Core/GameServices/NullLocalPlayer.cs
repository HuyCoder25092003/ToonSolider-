﻿using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal sealed class NullLocalPlayer : NullPlayer, ILocalPlayer
    {
        #region Static fields

        private     static  readonly    NullLocalPlayer     s_localPlayer    = new NullLocalPlayer();

        private     static  AuthChangeInternalCallback      s_onAuthChange;

        #endregion

        #region Constructors

        public NullLocalPlayer()
        { }

        #endregion

        #region Static methods

        public static NullLocalPlayer GetLocalPlayer()
        {
            LogNotSupported();

            return s_localPlayer;
        }

        public static void Authenticate()
        {
            LogNotSupported();

            if (s_onAuthChange != null)
            {
                s_onAuthChange(LocalPlayerAuthStatus.NotAvailable, Diagnostics.kFeatureNotSupported);
            }
        }

        public static void Signout()
        {
            LogNotSupported();
        }

        public static void SetAuthChangeCallback(AuthChangeInternalCallback callback)
        {
            // set value
            s_onAuthChange = callback;
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("LocalPlayer");
        }

        #endregion

        #region ILocalPlayer implementation

        public bool IsAuthenticated
        {
            get
            {
                LogNotSupported();

                return false;
            }
        }

        public bool IsUnderAge
        {
            get
            {
                LogNotSupported();

                return false;
            }
        }

        public void LoadFriends(EventCallback<GameServicesLoadPlayerFriendsResult> callback)
        {
            LogNotSupported();
            callback?.Invoke(null, Diagnostics.kFeatureNotSupported);
        }

        public void AddFriend(string playerId, EventCallback<bool> callback)
        {
            LogNotSupported();

            callback?.Invoke(false, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}