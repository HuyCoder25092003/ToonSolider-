﻿#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal class Player : PlayerBase
    {
        #region Constructors

        static Player()
        {
            // register callbacks
            PlayerBinding.NPPlayerRegisterCallbacks(HandleLoadPlayersNativeCallback, NativeCallbackResponder.HandleLoadImageNativeCallback, NativeCallbackResponder.HandleViewClosedNativeCallback);
        }

        protected Player()
        { }
        
        public Player(IntPtr nativePtr, bool retain = true)
        {
            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain);
        }

        ~Player()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        public static void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback)
        {
            var     tagPtr  = MarshalUtility.GetIntPtr(callback);
            PlayerBinding.NPPlayerLoadPlayers(playerIds, playerIds.Length, tagPtr);
        }

        #endregion

        #region Base class methods

        protected override string GetIdInternal()
        {
            return PlayerBinding.NPPlayerGetId(AddrOfNativeObject());
        }
        protected override string GetDeveloperScopeIdInternal()
        {
            return PlayerBinding.NPPlayerGetDeveloperScopeId(AddrOfNativeObject());
        }

        protected override string GetLegacyIdInternal()
        {
            return PlayerBinding.NPPlayerGetLegacyId(AddrOfNativeObject());
        }

        protected override string GetAliasInternal()
        {
            return PlayerBinding.NPPlayerGetAlias(AddrOfNativeObject());
        }

        protected override string GetDisplayNameInternal()
        {
            return PlayerBinding.NPPlayerGetDisplayName(AddrOfNativeObject());
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            var     tagPtr  = MarshalUtility.GetIntPtr(callback);
            PlayerBinding.NPPlayerLoadImage(AddrOfNativeObject(), tagPtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(GameServicesLoadArrayNativeCallback))]
        private static void HandleLoadPlayersNativeCallback(ref NativeArray nativeArray, NativeError error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     managedArray    = MarshalUtility.CreateManagedArray(nativeArray.Pointer, nativeArray.Length);
                var     players         = (managedArray == null) 
                    ? null 
                    : Array.ConvertAll(managedArray, (nativePtr) => new Player(nativePtr));
                var     errorObj        = error.Convert(GameServicesError.kDomain);
                ((LoadPlayersInternalCallback)tagHandle.Target).Invoke(players, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion
    }
}
#endif