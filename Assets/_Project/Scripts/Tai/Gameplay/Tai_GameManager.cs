using System;
using System.Collections;
using System.Collections.Generic;
using Tai_Core;
using UnityEngine;
using Newtonsoft.Json;
using Tai;

public class Tai_GameManager : SingletonMono<Tai_GameManager>
{
    public GameSave GameSave { get; set; }

    public Tai_UIGameplay uiGameplay;

    [Header("----------Transform and Game Object----------")]
    [SerializeField]
    private List<GameObject> lsPrefabArrows = new List<GameObject>();
    [SerializeField]
    private List<Transform> lsContainSpawnArrow = new List<Transform>();
    [SerializeField]
    private List<Transform> lsContainSpawnEnemyArrow = new List<Transform>();

    [SerializeField]
    private List<Transform> lsTransTargetArrows = new List<Transform>();
    [SerializeField]
    private List<Transform> lsPositionSpawnArrows = new List<Transform>();

    public GameObject goGameContent;

    [Header("----------Animator----------")]
    [SerializeField] private Tai_CharacterDataBinding boyDataBinding;
    [SerializeField] private Tai_CharacterDataBinding girlDataBinding;
    [SerializeField] private Tai_CharacterDataBinding bossDataBinding;
    private Tai_CharacterDataBinding enemyDataBinding;

    [Header("----------Data----------")]
    private List<Tai_ArrowDataItem> lsArrowDataItems = new List<Tai_ArrowDataItem>();

    [Header("----------Variable----------")]
    private float prevTimeArrow = 0;
    private float distanceMoveArrow = 0;
    private int curIndexArrow = 0;

    private float timeMoveArrow;

    [Range(1, 3)]
    [SerializeField]
    private float defaultTimeMoveArrow = 1.8f;

    public GameState gameState;
    private float timerSong;
    private float deltaTime;

    public string nameSong;

    private int miss;

    public float TimerSong
    {
        get
        {
            return timerSong;
        }
        set
        {
            timerSong = value;
            if(uiGameplay != null)
            {
                uiGameplay.UpdateTimerText(timerSong);
            }
        }
    }

    public int Miss
    {
        get
        {
            return miss;
        }
        set
        {
            miss = value;
            if (uiGameplay != null)
            {
                uiGameplay.UpdateMissText(miss);
            }
        }
    }

    private int score;
    private int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            if (uiGameplay != null)
            {
                uiGameplay.UpdateScoreText(score);
            }
        }
    }

    
    private int indexMode;
    private int indexWeek;
    private int indexSongOfWeek;

    private Difficult difficult;

    private Tai_ConfigWeekData configWeekData;
    private Tai_ConfigSongData configSongData;

    private Tai_GameplaySongData gameplaySongData;

    public List<Tai_TargetArrow> lsTargetArrows = new List<Tai_TargetArrow>();

    private void Awake()
    {
        Input.multiTouchEnabled = true;
        goGameContent.SetActive(false);
        GameSave = SaveManager.Instance.LoadGame();

        UIManager.Instance.InitUI(() =>
        {
            UIManager.Instance.ShowUI(UIIndex.UILoading);
        });
    }

    private IEnumerator Start()
    {
        //Get List Position Spawn Arrow
        lsPositionSpawnArrows = uiGameplay.GetListTransformSpawnArrow();
        //Get List Trans Target Arrow
        lsTransTargetArrows = uiGameplay.GetListTargetArrow();

        yield return new WaitForSeconds(0.1f);
        //SetUpGameplay(0, 1, 0, Difficult.Easy);
    }

    public void SetUpGameplay(int indexMode, int indexWeek, int indexSong, Difficult difficult)
    {
        this.indexMode = indexMode;
        this.indexWeek = indexWeek;
        this.indexSongOfWeek = indexSong;
        this.difficult = difficult;

        curIndexArrow = 0;

        configWeekData = Tai_ConfigMode.ConfigWeekData(indexMode, indexWeek);
        configSongData = Tai_ConfigMode.ConfigSongData(indexMode, indexWeek, indexSongOfWeek);

        goGameContent.SetActive(true);

        gameState = GameState.None;
        GetSongGamePlay(indexMode, configSongData.nameJson);
        Tai_SoundManager.Instance.PlaySoundBGM();
        float lengthSong = Tai_SoundManager.Instance.GetLengthBGM();
        Debug.Log("lengthSong: " + lengthSong);

        SetUpGameplayUI(lengthSong);

        // Game Analytics
        GameAnalyticsManager.Instance.PlayStart(configSongData.nameSong);
    }

    public void SetUpGameplayUI(float lengthSong)
    {

        prevTimeArrow = 0;
        distanceMoveArrow = uiGameplay.GetDistanceMoveArrow();
        timeMoveArrow = defaultTimeMoveArrow;

        //Get Data JSON
        Tai_RootItem rootItem =
            JsonConvert.DeserializeObject<Tai_RootItem>(Resources.Load<TextAsset>("Jsons/" + configSongData.nameJson + "-easy").text);
        Tai_SongItem songItem = rootItem.song;

        Debug.Log("json: " + rootItem.ToString());

        lsArrowDataItems.Clear();
        for (int i = 0; i < songItem.notes.Count; i++)
        {
            for (int j = 0; j < songItem.notes[i].sectionNotes.Count; j++)
            {
                Tai_ArrowDataItem arrowDataItem = new Tai_ArrowDataItem(songItem.notes[i].sectionNotes[j][0],
                    (int)songItem.notes[i].sectionNotes[j][1] % 4, songItem.notes[i].sectionNotes[j][2],
                    songItem.notes[i].mustHitSection);
                lsArrowDataItems.Add(arrowDataItem);
                //Debug.Log("data: " + arrowDataItem.indexArrow + " " +)
            }
        }

        lsArrowDataItems.Sort(SortByTimeAppear);

        timeMoveArrow = defaultTimeMoveArrow * 1;
        TimerSong = lengthSong;
        deltaTime = timeMoveArrow - 0.1f;

        Tai_SoundManager.Instance.StopSoundFX(SoundFXIndex.SoundMenu);
        Debug.Log("Show Gameplay");
        UIManager.Instance.ShowUI(UIIndex.UIGameplay, new GameplayParam()
        {
            difficult = difficult,
            maxValueSlider = 50,
            nameSong = configSongData.nameSong
        }, () =>
        {
            gameplaySongData = Tai_ConfigGameplay.ConfigSongData(indexMode, indexWeek, indexSongOfWeek);
            uiGameplay.SetSpriteIconBoss(gameplaySongData.spriteIconLose,
                gameplaySongData.spriteIcon);
        });

        Debug.Log("Show UI Done");

        Miss = 0;
        Score = 0;

        SetUpCharacter();

        //gameState = GameState.Playing;
    }

    public void SetUpCharacter()
    {
        if (configSongData.nameJson == "tutorial")
        {
            bossDataBinding.gameObject.SetActive(false);
            enemyDataBinding = girlDataBinding;
        }
        else
        {
            bossDataBinding.gameObject.SetActive(true);
            girlDataBinding.SetAnimatonCharacter(0);
            bossDataBinding.GetComponent<Animator>().runtimeAnimatorController = gameplaySongData.enemyAnimator;
            enemyDataBinding = bossDataBinding;
        }
    }

    private void Update()
    {
        if (gameState == GameState.Playing && TimerSong >= 0)
        {
            ShowTimerSong();
            if (indexMode == 0 && indexWeek == 0 && indexSongOfWeek == 0)
            {
                LoadNoteNew(Tai_SoundManager.Instance.GetCurrentTimeSoundBGM() + deltaTime);
            }
            else
            {
                CalculateCreateArrow(Tai_SoundManager.Instance.GetCurrentTimeSoundBGM() + deltaTime);
            }

        }
    }

    private void ShowTimerSong()
    {

        TimerSong -= Time.deltaTime;
        if (TimerSong <= 0)
        {
            //Show timer text
            uiGameplay.UpdateTimerText(0);
            //Check win/lose
            if (uiGameplay.CheckGameWin())
            {
                ShowGameWin();
            }
            else
            {
                goGameContent.SetActive(false);
                ShowGameLose();
            }
        }
    }

    private int SortByTimeAppear(Tai_ArrowDataItem obj1, Tai_ArrowDataItem obj2)
    {
        return obj1.timeAppear.CompareTo(obj2.timeAppear);
    }

    public void LoadNoteNew(float time)
    {
        if (curIndexArrow == lsArrowDataItems.Count - 1)
        {

            //if (lsArrowDataItems[curIndexArrow - 1].timeAppear > time * 1000) 
            //{
            //return;
            //}
            //else
            {
                if (((lsArrowDataItems[curIndexArrow].timeAppear / 1000) - time) < -0.001f &&
                    ((lsArrowDataItems[curIndexArrow].timeAppear / 1000) - time) >= -0.15f)
                {
                    Debug.Log("arrow: " + curIndexArrow + " " + " " + lsArrowDataItems.Count +
                        " " + lsArrowDataItems[curIndexArrow].timeAppear);
                    //Create arrow
                    CreateArrow();
                    return;
                }
            }
        }
        else
        {

            if (curIndexArrow < lsArrowDataItems.Count - 1)
            {
                if (lsArrowDataItems[curIndexArrow].timeAppear == 0 || lsArrowDataItems[curIndexArrow].timeAppear < 1000)
                {
                    //Create arrow
                    CreateArrow();
                    return;
                }
                else
                {
                    if (lsArrowDataItems[curIndexArrow].timeAppear > time * 1000)
                    {
                        return;
                    }
                    else
                    {
                        if (lsArrowDataItems[curIndexArrow + 1].timeAppear > time * 1000)
                        {
                            //Create Arrow
                            CreateArrow();
                        }
                    }
                }
            }

        }
    }

    private void CalculateCreateArrow(float time)
    {
        if (curIndexArrow >= lsArrowDataItems.Count)
        {
            return;
        }

        if (lsArrowDataItems[curIndexArrow].timeAppear / 1000 < time && (time - prevTimeArrow > 0.1f))
        {
            Debug.Log("arrow: " + curIndexArrow + " " + lsArrowDataItems[curIndexArrow].timeAppear + " " + lsArrowDataItems.Count);
            CreateArrow();
            prevTimeArrow = time;
        }
    }

    private void CreateArrow()
    {
        if (lsArrowDataItems[curIndexArrow] != null)
        {
            int indexArrowClone = lsArrowDataItems[curIndexArrow].indexArrow;
            int sumArrow = lsArrowDataItems.Count;


            //Debug.Log("Arrow: " + indexArrowClone + " " + lsArrowDataItems[curIndexArrow].timeAppear);

            if (lsArrowDataItems[curIndexArrow].mustHit)
            {
                //Create arrow from list prefab
                GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone], lsContainSpawnArrow[indexArrowClone]);
                Debug.Log("Arrow 1: " + indexArrowClone + " " + lsArrowDataItems[curIndexArrow].timeAppear + " " + goArrow.name);
                goArrow.transform.localPosition = lsPositionSpawnArrows[indexArrowClone].position;
                Tai_Arrow arrowMove = goArrow.GetComponent<Tai_Arrow>();
                //Set up arrow
                arrowMove.SetUpArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000,
                    lsArrowDataItems[curIndexArrow].indexArrow, lsArrowDataItems[curIndexArrow].mustHit,
                    distanceMoveArrow, curIndexArrow, sumArrow);
            }
            else
            {

                //Create arrow from list prefab
                GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone], lsContainSpawnEnemyArrow[indexArrowClone]);
                Debug.Log("Arrow 2: " + indexArrowClone + " " + lsArrowDataItems[curIndexArrow].timeAppear);
                goArrow.transform.localPosition = lsPositionSpawnArrows[indexArrowClone].position;
                Tai_Arrow arrowMove = goArrow.GetComponent<Tai_Arrow>();
                //Set up arrow
                arrowMove.SetUpArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000,
                    lsArrowDataItems[curIndexArrow].indexArrow, lsArrowDataItems[curIndexArrow].mustHit,
                    distanceMoveArrow, curIndexArrow, sumArrow);
            }

            curIndexArrow++;

        }
    }

    private void GetSongGamePlay(int indexMod, string nameSong)
    {
        if (indexMod == 0)
        {
            AudioClip songAudioClip = Resources.Load("Sounds/Inst-" + nameSong) as AudioClip;
            Tai_SoundManager.Instance.AddSoundBGM(songAudioClip);
        }
        else
        {
            //Get from Asset Bundle
        }
    }

    public void OnButtonClickDown(int index)
    {
        lsTargetArrows[index].IsPress = true;
    }

    public void OnButtonClickUp(int index)
    {
        lsTargetArrows[index].IsPress = false;
        for (int i = 0; i < lsContainSpawnArrow[index].childCount; i++)
        {
            lsContainSpawnArrow[index].GetChild(i).GetComponent<Tai_Arrow>().IsPress = false;
        }
    }

    public void SetAnimationBoy(float index, float timeLoop = 0)
    {
        boyDataBinding.SetAnimatonCharacter(index);
        if (timeLoop == 0)
        {
            timeLoop = 0.5f;
        }

        float speedMove = distanceMoveArrow / timeMoveArrow;
        float newTimeMove = (timeLoop * 10) / speedMove;
        CancelInvoke("DelayFinishAnimBoy");
        Invoke("DelayFinishAnimBoy", newTimeMove);
    }

    public void DelayFinishAnimBoy()
    {
        boyDataBinding.SetAnimatonCharacter(0);
    }

    public void AddScore()
    {
        //Show combo text correct
        Score += 100;
        uiGameplay.SetSliderHP(-1);
    }

    public void SubScore()
    {
        Miss++;
        uiGameplay.SetSliderHP(1);
    }

    public void ShowGameLose()
    {
        Debug.Log("ShowGameLose");
        if (gameState != GameState.EndGame)
        {
            Tai_SoundManager.Instance.StopSoundBGM();

            //Clear all arrow
            ClearAllArrow();

            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            gameState = GameState.EndGame;

            GameAnalyticsManager.Instance.PlayEnd(configSongData.nameSong, "lose", score, miss, timerSong);
            UIManager.Instance.ShowUI(UIIndex.UILose);
        }
    }

    public void ShowGameWin()
    {
        GameSave.ModeSaves[indexMode].WeekSaves[indexWeek].SongSaves[indexSongOfWeek].Score = score;
        gameState = GameState.EndGame;

        GameAnalyticsManager.Instance.PlayEnd(configSongData.nameSong, "win", score, miss, timerSong);
        UIManager.Instance.ShowUI(UIIndex.UIWin, new WinParam() { coinReward = 100 });
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameState = GameState.EndGame;
        SetUpGameplay(indexMode, indexWeek, indexSongOfWeek, difficult);
        Tai_SoundManager.Instance.StopSoundBGM();

        ClearAllArrow();
    }
    public void GoToHome()
    {
        Time.timeScale = 1;
        gameState = GameState.EndGame;
        UIManager.Instance.HideUI(UIIndex.UIGameplay);
        Tai_SoundManager.Instance.StopSoundBGM();

        //Hide Game Object Content
        goGameContent.SetActive(false);

        ClearAllArrow();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameState = GameState.Playing;
        Tai_SoundManager.Instance.ResumeSoundBGM();
    }

    private void ClearAllArrow()
    {
        for (int i = 0; i < lsContainSpawnArrow.Count; i++)
        {
            if (lsContainSpawnArrow[i].childCount > 0)
            {
                for (int j = 0; j < lsContainSpawnArrow[i].childCount; j++)
                {
                    lsContainSpawnArrow[i].GetChild(j).GetComponent<Tai_Arrow>().DestroySelf();
                }
            }
        }

        for (int i = 0; i < lsContainSpawnEnemyArrow.Count; i++)
        {
            if (lsContainSpawnEnemyArrow[i].childCount > 0)
            {
                for (int j = 0; j < lsContainSpawnEnemyArrow[i].childCount; j++)
                {
                    lsContainSpawnEnemyArrow[i].GetChild(j).GetComponent<Tai_Arrow>().DestroySelf();
                }
            }
        }
    }

    public void SetAnimationEnemy(float index, float timeLoop = 0)
    {
        if (enemyDataBinding == null || !enemyDataBinding.gameObject.activeSelf)
            return;
        enemyDataBinding.SetAnimatonCharacter(index);
        if(timeLoop == 0)
        {
            timeLoop = 0.5f;
        }

        float speedMove = distanceMoveArrow / timeMoveArrow;
        float newTimeMove = (timeLoop * 10) / speedMove;

        CancelInvoke("DelayFinishAnimEnemy");
        Invoke("DelayFinishAnimEnemy", newTimeMove);
    }

    public void DelayFinishAnimEnemy()
    {
        enemyDataBinding.SetAnimatonCharacter(0);
    }
}

[Serializable]
public class Tai_ArrowDataItem
{
    public float timeAppear;
    public int indexArrow;
    public float timeTail;
    public bool mustHit;

    public Tai_ArrowDataItem(float timeAppear, int indexArrow, float timeTail, bool mustHit)
    {
        this.timeAppear = timeAppear;
        this.indexArrow = indexArrow;
        this.timeTail = timeTail;
        this.mustHit = mustHit;
    }

    
}

[Serializable]
public class Tai_NoteSongItem
{
    public int lengthInStep;
    public bool mustHitSection;
    public List<float[]> sectionNotes = new List<float[]>();

    public Tai_NoteSongItem(List<float[]> sectionNotes, int lengthInStep, bool mustHitSection)
    {
        this.sectionNotes = sectionNotes;
        this.lengthInStep = lengthInStep;
        this.mustHitSection = mustHitSection;
    }
}

[Serializable]
public class Tai_SongItem
{
    public List<Tai_NoteSongItem> notes = new List<Tai_NoteSongItem>();
}

[Serializable]
public class Tai_RootItem
{
    public Tai_SongItem song;
}

public enum GameState
{
    None = 0, 
    PauseGame = 1,
    Playing = 2,
    Ready = 3,
    EndGame = 4
}

public enum Difficult
{
    Easy = 0,
    Normal = 1,
    Hard = 2,
}
