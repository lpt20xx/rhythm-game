using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using TMPro;
using UnityEngine.UI;
using Tai_Core;
using System;
using Random = UnityEngine.Random;

namespace Tai
{
	 public class Tai_UIMainMenu : BaseUI
	 {
        [SerializeField] private List<SongItem> lsSongItems = new List<SongItem>();
        [SerializeField] private TextMeshProUGUI txtCoin;

        [SerializeField] private Image imgBoy;
        [SerializeField] private Image imgGirl;

        [SerializeField] private Image imgVibrate;
        [SerializeField] private Sprite spriteVibrateOn;
        [SerializeField] private Sprite spriteVibrateOff;

        [SerializeField] private Animator boyAnimator;
        [SerializeField] private Animator girlAnimator;

        [SerializeField] private List<GameObject> lsGoSelectedModes = new List<GameObject>();

        private GameObject goCurSelectMode;

        private const int NumberSongNewHot = 20;

        private GameSave gameSave;

        public override void OnInit()
        {
            base.OnInit();

            for(int i = 0; i < lsGoSelectedModes.Count; i++)
            {
                lsGoSelectedModes[i].SetActive(false);
            }
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);

            gameSave = Tai_GameManager.Instance.GameSave;

            bool VibrateOn = gameSave.VibrateOn;
            if (VibrateOn)
            {
                imgVibrate.sprite = spriteVibrateOn;
            }
            else
            {
                imgVibrate.sprite = spriteVibrateOff;
            }
            //imgVibrate.SetNativeSize();

            if (!Tai_SoundManager.Instance.CheckSoundFXAvailable(SoundFXIndex.SoundMenu))
            {
                Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.SoundMenu);
            }

            // Select first mode

            Timer.DelayedCall(0.5f, () =>
            {
                //imgBoy.SetNativeSize();
                //imgGirl.SetNativeSize();
                //Get coin from Save
                UpdateTextCoin();
            }, this);

            OnSelectMode(0);

            if (gameSave.CurrentDay != DateTime.Now.DayOfYear)
            {
                gameSave.CurrentDay = DateTime.Now.DayOfYear;
                gameSave.HotNewSongs.Clear();
                int countSong = 0;
                while (countSong < NumberSongNewHot)
                {
                    int randMode = Random.Range(1, Tai_ConfigGameplay.GetModeLength());
                    int randWeek = Random.Range(0, Tai_ConfigGameplay.GetWeekLength(randMode));
                    int randSong = Random.Range(0, Tai_ConfigGameplay.GetSongLength(randMode, randWeek));
                    //HotNewSong hotNewSong = new HotNewSong
                    //{ }
                }
            }
        }

        public void OnOption_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UIOption);
        }

        public void OnRate_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UIRate);
        }

        public void OnSkin_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UISkin);
        }

        public void OnRewardLogin_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UIRewardLogin);
        }

        public void OnLuckyDraw_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            UIManager.Instance.ShowUI(UIIndex.UILuckyDraw);
        }

        public void OnVibrate_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            // Save vibrate setting
            gameSave.VibrateOn = !gameSave.VibrateOn;
            bool vibrateOn = gameSave.VibrateOn;
            if (vibrateOn)
            {
                imgVibrate.sprite = spriteVibrateOn;
            }
            else
            {
                imgVibrate.sprite = spriteVibrateOff;
            }
            //imgVibrate.SetNativeSize();
        }

        public void UpdateTextCoin()
        {
            // Set coin from save to TxtCoin
            int coin = gameSave.Coin;
            txtCoin.text = coin.ToString();
        }

        public void OnSaveSongData(int indexMode, int indexWeek, int indexSong)
        {
            gameSave.ModeSaves[indexMode].WeekSaves[indexWeek].SongSaves[indexSong]
                .IsBought = true;
            UpdateTextCoin();
        }

        private bool CheckAvailableSong(HotNewSong hotNewSong)
        {
            if (gameSave.HotNewSongs.Contains(hotNewSong))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnNewHot_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            for (int i = 0; i < lsSongItems.Count; i++)
            {
                lsSongItems[i].gameObject.SetActive(i < NumberSongNewHot);
            }

            for (int i = 0; i < gameSave.HotNewSongs.Count; i++)
            {
                HotNewSong hotNewSong = gameSave.HotNewSongs[i];

                SongSave songSave = gameSave.ModeSaves[hotNewSong.IndexMode].WeekSaves[hotNewSong.IndexWeek]
                    .SongSaves[hotNewSong.IndexSong];
                Tai_GameplaySongData gameplaySongData = Tai_ConfigGameplay.ConfigSongData(hotNewSong.IndexMode, hotNewSong.IndexWeek, hotNewSong.IndexSong);
                lsSongItems[i].OnSetUpSongItem(this, hotNewSong.IndexMode, hotNewSong.IndexWeek, 
                    hotNewSong.IndexSong, gameplaySongData, songSave.Score, songSave.IsBought);
            }
        }

        public void OnSelectMode(int index)
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            int countSong = Tai_ConfigGameplay.GetAllSongInMode(index);
            for(int i = 0; i < lsSongItems.Count; i++)
            {
                lsSongItems[i].gameObject.SetActive(i < countSong);
            }

            int count = 0;
            for (int i = 0; i < Tai_ConfigGameplay.GetWeekLength(index); i++)
            {
                for (int j = 0; j < Tai_ConfigGameplay.GetSongLength(index, i); j++)
                {
                    SongSave songSave = gameSave.ModeSaves[index].WeekSaves[i].SongSaves[j];
                    Tai_GameplaySongData gameplaySongData = Tai_ConfigGameplay.ConfigSongData(index, i, j);

                    lsSongItems[count].OnSetUpSongItem(this, index, i, j,
                        gameplaySongData, songSave.Score, songSave.IsBought);
                    count++;
                }
            }
        }

        public void ChangeUISelectModeBtn(int indexMode)
        {
            if (goCurSelectMode != null)
            {
                goCurSelectMode.SetActive(false);

            }

            goCurSelectMode = lsGoSelectedModes[indexMode];
            goCurSelectMode.SetActive(true);
        }
    }
}