using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config Mode", menuName = "Config/Config Mode", order = 3)]
public class Tai_ConfigMode : ScriptableObject
{
    public Tai_ConfigModeData[] data;
    private static Tai_ConfigMode Instance;

    public static Tai_ConfigModeData ConfigModeData(int indexMode)
    {
        Instance = Resources.Load<Tai_ConfigMode>("Configs/Config Mode");

        Tai_ConfigModeData result = null;

        if(Instance.data.Length > indexMode) 
        {
            result = Instance.data[indexMode];
        }

        if (result == null)
        {
            result = Instance.data[0];
        }

        return result;
    }

    public static Tai_ConfigWeekData ConfigWeekData(int indexMode, int indexWeek)
    {
        Instance = Resources.Load<Tai_ConfigMode>("Configs/Config Mode");

        Tai_ConfigWeekData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0];
        }

        return result;
    }

    public static Tai_ConfigSongData ConfigSongData(int indexMode,int indexWeek, int indexSong)
    {
        Instance = Resources.Load<Tai_ConfigMode>("Configs/Config Mode");

        Tai_ConfigSongData result = null;

        if (Instance.data.Length > indexMode && Instance.data[indexMode].configWeekDatas.Count > indexWeek
            && Instance.data[indexMode].configWeekDatas[indexWeek].configSongDatas.Count > indexSong)
        {
            result = Instance.data[indexMode].configWeekDatas[indexWeek].configSongDatas[indexSong];
        }
        else
        {
            result = Instance.data[0].configWeekDatas[0].configSongDatas[0];
        }

        return result;
    }
}

[Serializable]
public class Tai_ConfigSongData
{
    public string nameSong;
    public string nameJson;
}

[Serializable]
public class Tai_ConfigWeekData
{
    public string name;
    public List<Tai_ConfigSongData> configSongDatas = new List<Tai_ConfigSongData>();
}

[Serializable]
public class Tai_ConfigModeData
{
    public string nameMode;
    public string nameAssetBundle;
    public List<Tai_ConfigWeekData> configWeekDatas = new List<Tai_ConfigWeekData>();
}

