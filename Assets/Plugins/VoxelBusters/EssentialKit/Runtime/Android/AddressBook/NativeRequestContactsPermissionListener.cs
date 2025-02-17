#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.AddressBookCore.Android
{
    public class NativeRequestContactsPermissionListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnFailureDelegate(NativeErrorInfo errorInfo);
        public delegate void OnSuccessDelegate();

        #endregion

        #region Public callbacks

        public OnFailureDelegate  onFailureCallback;
        public OnSuccessDelegate  onSuccessCallback;

        #endregion


        #region Constructors

        public NativeRequestContactsPermissionListener() : base("com.voxelbusters.essentialkit.addressbook.IAddressBookHandler$IRequestContactsPermissionListener")
        {
        }

        #endregion


        #region Public methods
#if NATIVE_PLUGINS_DEBUG_ENABLED
        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            DebugLogger.Log("**************************************************");
            DebugLogger.Log("[Generic Invoke : " +  methodName + "]" + " Args Length : " + (javaArgs != null ? javaArgs.Length : 0));
            if(javaArgs != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                foreach(AndroidJavaObject each in javaArgs)
                {
                    if(each != null)
                    {
                        builder.Append(string.Format("[Type : {0} Value : {1}]", each.Call<AndroidJavaObject>("getClass").Call<string>("getName"), each.Call<string>("toString")));
                        builder.Append("\n");
                    }
                    else
                    {
                        builder.Append("[Value : null]");
                        builder.Append("\n");
                    }
                }

                DebugLogger.Log(builder.ToString());
            }
            DebugLogger.Log("-----------------------------------------------------");
            return base.Invoke(methodName, javaArgs);
        }
#endif

        public void onFailure(AndroidJavaObject errorInfo)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onFailure"  + " " + "[" + "errorInfo" + " : " + errorInfo +"]");
#endif
            if(onFailureCallback != null)
            {
                onFailureCallback(new NativeErrorInfo(errorInfo));
            }
        }
        public void onSuccess()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onSuccess" );
#endif
            if(onSuccessCallback != null)
            {
                onSuccessCallback();
            }
        }

        #endregion
    }
}
#endif