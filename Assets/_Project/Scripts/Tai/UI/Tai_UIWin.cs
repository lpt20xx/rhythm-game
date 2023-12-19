using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using TMPro;
using UnityEngine.UI;
using Tai_Core;

namespace Tai
{
    public class WinParam : UIParam
    {
        public int coinReward;
    }

    public class Tai_UIWin : BaseUI
	 {
        [SerializeField] private TextMeshProUGUI txtCoinReward;
        [SerializeField] private TextMeshProUGUI txtCoin;

        [SerializeField] private Button btnX5;

        private bool isWatchAds;
        private int valueCoinReward;

        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            // Get coin from save


            WinParam winParam = (WinParam) param;
            txtCoinReward.text = "+" + winParam.coinReward;

            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Victory);

            btnX5.gameObject.SetActive(true);
            isWatchAds = false;
        }

        public void OnHome_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            Tai_SoundManager.Instance.StopSoundFX(SoundFXIndex.Victory);
            // Show inter ads


            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);

            // Disable Game Content
            Tai_GameManager.Instance.goGameContent.SetActive(false);
            if (!isWatchAds)
            {
                //Add coin reward value to Save
                Tai_GameManager.Instance.GameSave.Coin += valueCoinReward;
                SaveManager.Instance.SaveGame();
            }
        }

        public void OnX5_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            AdsManager.Instance.ShowRewardedAds( () => 
            {
                UIManager.Instance.HideUI(UIIndex.UIGameplay);
                //UIManager.Instance.HideUI(this);

                //Show reward ads
                isWatchAds = true;
                valueCoinReward *= 5;
                //Add coin reward to Save
                btnX5.gameObject.SetActive(false);
            });
            
        }
	 }
}