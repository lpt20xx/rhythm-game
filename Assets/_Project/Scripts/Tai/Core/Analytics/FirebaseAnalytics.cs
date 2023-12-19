using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseAnalytics : IGameAnalytics
{
    private float startTime;
    public void Init()
    {
        startTime = Time.time;
    }

    public void LogEvent(string name)
    {
        Debug.Log("Log Event");
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
    }

    public void LogEvent(string name, Firebase.Analytics.Parameter[] parameters)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameters);
    }

    public void LogEvent(string name, string parameterName, string parameterValue)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
    }

    public void SetUserProperty(string name, string value)
    {
        Firebase.Analytics.FirebaseAnalytics.SetUserProperty(name, value);
    }
}
