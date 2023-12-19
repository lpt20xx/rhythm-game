using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameAnalytics
{
    void Init();
    void LogEvent(string name);
    void SetUserProperty(string name, string value);
}
