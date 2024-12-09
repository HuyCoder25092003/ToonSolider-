using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.AdsKit;
using VoxelBusters.CoreLibrary;
public class AdsController : BYSingletonMono<AdsController>, IAdLifecycleEventListener
{
    public int CallbackOrder => 1;
    Action<bool, int> callback;
    void Start()
    {
        var consentFormProvider = GetConsentFormProvider();
        InitAdsManager(consentFormProvider);
    }
    IConsentFormProvider GetConsentFormProvider()
    {
        var consentFormProvider = AdServices.GetConsentFormProvider();

        if (consentFormProvider == null)
        {
            Debug.Log("There are no IConsentFormProvider implementations available. For a default plugin's implementation, enable Ad Mob network or implement a custom IConsentFormProvider on your own");
        }

        return consentFormProvider;
    }
    public void InitAdsManager(IConsentFormProvider consentFormProvider)
    {
        if (AdsManager.IsInitialisedOrWillChange)
        {
            Debug.Log("Initialisation is in progress or already initialised.");
            return;
        }

        var operation = AdsManager.Initialise(consentFormProvider);

        Debug.Log("*** Operation: " + operation);
        operation.OnComplete += (op) =>
        {
            if (op.Error == null)
            {
                Debug.Log("Initialise complete. You can start loading or showing the ads from now.");
            }
            else
            {
                Debug.Log("Failed initialising Ads Kit with: " + op.Error);
            }
        };
    }
    private void ShowAd(string placement, Action<bool, int> callback)
    {
        this.callback = callback;
        AdsManager.ShowAd(placement);
    }
    private void OnEnable()
    {
        AdsManager.RegisterListener(this);
    }
    private void OnDisable()
    {
        AdsManager.UnregisterListener(this);
    }
    #region IAD events
    public void OnInitialisationComplete(InitialiseResult result)
    {
        Debug.Log("AdsKit is initialised successfully.");
    }

    public void OnInitialisationFail(Error error)
    {
        Debug.Log($"AdsKit failed to initialise with error {error}.");
    }

    public void OnLoadAdComplete(string placement, LoadAdResult result)
    {
        Debug.Log($"AdsKit has successfully loaded ad for placementId: {placement}.");
    }

    public void OnLoadAdFail(string placement, Error error)
    {
        Debug.Log($"AdsKit has failed to load ad for placementId: {placement} with error: {error}.");
    }

    public void OnShowAdStart(string placement)
    {

    }

    public void OnShowAdClick(string placement)
    {
        Debug.Log($"AdsKit has recognised click on ad for placementId: {placement}.");
        callback?.Invoke(true, 2);
        callback = null;
    }

    public void OnShowAdComplete(string placement, ShowAdResult result)
    {
        Debug.Log($"AdsKit has completed showing ad for placementId: {placement} with result: {result}.");
        callback?.Invoke(true, 1);
        callback = null;
    }

    public void OnShowAdFail(string placement, Error error)
    {
        Debug.Log($"AdsKit has failed to show ad for placementId: {placement} with Error: {error}.");
        callback?.Invoke(false, 0);
        callback = null;
    }

    public void OnAdImpressionRecorded(string placement)
    {

    }

    public void OnAdPaid(string placement, AdTransaction transaction)
    {

    }

    public void OnAdReward(string placement, AdReward reward)
    {
        Debug.Log($"OnAdReward: {placement}");
    }
    #endregion
}
