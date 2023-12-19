using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using Tai_Core;
namespace Tai
{
	 public class Tai_UIPause : BaseUI
	 {
		public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            Time.timeScale = 0;
        }

        public void OnResume_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            //Resume game
            Tai_GameManager.Instance.ResumeGame();
            //Show inter ads
            UIManager.Instance.HideUI(this);
        }

        public void OnHome_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            //End game
            Tai_GameManager.Instance.GoToHome();
            UIManager.Instance.HideUI(this);
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
        }

        public void OnRestart_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            
            UIManager.Instance.HideUI(this);
            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            //Show inter ads
            //Restart level
            Tai_GameManager.Instance.RestartGame();
        }
    }
}