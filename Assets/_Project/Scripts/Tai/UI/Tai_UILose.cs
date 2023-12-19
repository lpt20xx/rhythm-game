using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using Tai_Core;
namespace Tai
{
	 public class Tai_UILose : BaseUI
	 {
        [SerializeField] private GameObject goBtns;
		public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);

            goBtns.SetActive(false);

            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.GameOver);
            Timer.DelayedCall( 2, () =>
            {
                //Show UI buttons
                goBtns.SetActive(true);

            }, this);

            AdsManager.Instance.ShowInterstitialAds(() =>
            {
                Debug.Log("Show inter UI Lose");
            });
        }

        public void OnHome_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            Tai_SoundManager.Instance.StopSoundFX(SoundFXIndex.GameOver);

            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
            // Show inter ads
        }

        public void OnRestart_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            Tai_SoundManager.Instance.StopSoundFX(SoundFXIndex.GameOver);

            UIManager.Instance.HideUI(this);
            //Game Manager restart function
            Tai_GameManager.Instance.RestartGame();
        }
    }
}