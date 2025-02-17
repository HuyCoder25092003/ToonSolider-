#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    public class NativeBillingTransactionStateListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnFailedDelegate(NativeBillingTransaction transaction, NativeErrorInfo errorInfo);
        public delegate void OnStartedDelegate(NativeBillingTransaction transaction);
        public delegate void OnUpdatedDelegate(NativeBillingTransaction transaction);

        #endregion

        #region Public callbacks

        public OnFailedDelegate  onFailedCallback;
        public OnStartedDelegate  onStartedCallback;
        public OnUpdatedDelegate  onUpdatedCallback;

        #endregion


        #region Constructors

        public NativeBillingTransactionStateListener() : base("com.voxelbusters.essentialkit.billingservices.common.interfaces.IBillingTransactionStateListener")
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

        public void onFailed(AndroidJavaObject transaction, AndroidJavaObject errorInfo)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onFailed"  + " " + "[" + "transaction" + " : " + transaction +"]" + " " + "[" + "errorInfo" + " : " + errorInfo +"]");
#endif
            if(onFailedCallback != null)
            {
                onFailedCallback(new NativeBillingTransaction(transaction), new NativeErrorInfo(errorInfo));
            }
        }
        public void onStarted(AndroidJavaObject transaction)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onStarted"  + " " + "[" + "transaction" + " : " + transaction +"]");
#endif
            if(onStartedCallback != null)
            {
                onStartedCallback(new NativeBillingTransaction(transaction));
            }
        }
        public void onUpdated(AndroidJavaObject transaction)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onUpdated"  + " " + "[" + "transaction" + " : " + transaction +"]");
#endif
            if(onUpdatedCallback != null)
            {
                onUpdatedCallback(new NativeBillingTransaction(transaction));
            }
        }

        #endregion
    }
}
#endif