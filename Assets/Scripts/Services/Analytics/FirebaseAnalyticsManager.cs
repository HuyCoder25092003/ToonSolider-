using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseAnalyticsManager : MonoBehaviour
{
    public void Log(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }
    public void Log(string evenetName, float value)
    {
        FirebaseAnalytics.LogEvent(evenetName, evenetName + "_param", value);
    }
    public void Log(string evenetName, int value)
    {
        FirebaseAnalytics.LogEvent(evenetName, evenetName + "_param", value);
    }
}
