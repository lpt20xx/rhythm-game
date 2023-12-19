using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Tai;
using System;

namespace Tai_Core
{
    public class SaveManager : SingletonMono<SaveManager>
    {
        private float gameSaveInterval = 10f;
        private string gameSaveFileName = "/game-save.json";

        private float timeSinceSave;
        private GameSave gameSave;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if(Tai_GameManager.Instance && gameSave != null) 
            {
                timeSinceSave += Time.deltaTime;
                if(timeSinceSave > gameSaveInterval)
                {
                    timeSinceSave = 0;
                    // Save game
                    SaveGame();
                }
            }
        }

        public GameSave Init()
        {
            try
            {
                if (gameSave == null)
                {
                    string gameSavePath = GetGameSavePath();
                    if (File.Exists(gameSavePath))
                    {
                        Debug.Log("Loading game save: " + gameSavePath);

                        var s = FileHelper.LoadFileWithPassword(gameSavePath, "", true);

                        try
                        {
                            gameSave = JsonConvert.DeserializeObject<GameSave>(s);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Parse Game Save Error: " + e.Message);
                        }
                    }
                    else
                    {
                        Debug.Log("Game save not found, starting a new game");
                        if (gameSave == null)
                        {
                            gameSave = new GameSave();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load saved game due to: " + e.Message);
            }

            return gameSave;
        }

        private string GetGameSavePath()
        {
            return Application.persistentDataPath + gameSaveFileName;
        }

        public GameSave LoadGame()
        {
            if (gameSave == null)
            {
                Init();
            }

            gameSave.Init(Application.version);

            return gameSave;
        }

        public void SaveGame()
        {
            string gameSavePath = GetGameSavePath();
            string content = JsonConvert.SerializeObject(gameSave, Formatting.Indented);
            File.WriteAllText(gameSavePath, content);
        }

        private void OnApplicationQuit()
        {
            gameSave.isFirstOpen = true;
        }
    }
}

