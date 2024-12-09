using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : BYSingletonMono<AnalyticsManager>
{
    [SerializeField] FirebaseAnalyticsManager firebaseAnalytics;
    public void Log(string eventName)
    {
        firebaseAnalytics.Log(eventName);
    }
    public void Log(string eventName, float value)
    {
        firebaseAnalytics.Log(eventName, value);
    }
    public void Log(string eventName, int value)
    {
        firebaseAnalytics.Log(eventName, value);
    }
}
