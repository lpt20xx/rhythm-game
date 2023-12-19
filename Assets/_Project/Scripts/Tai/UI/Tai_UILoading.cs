using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Tai_Core;

namespace Tai
{
	 public class Tai_UILoading : BaseUI
	 {
        [SerializeField] private TextMeshProUGUI txtLoading;
        [SerializeField] private Image imgProgress;
        [SerializeField] private float timerLoading = 3f;

        private float timer;
        private float valueSlider;
		public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.SoundMenu, true);
            valueSlider = 0;
            imgProgress.fillAmount = 0;
            DOTween.To(() => imgProgress.fillAmount, x => imgProgress.fillAmount = x, 1,
                timerLoading).OnComplete(() =>
            {
                UIManager.Instance.HideUI(this);
                UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
            });

            StartCoroutine(ShowLoading());
            base.OnSetup(param);
        }

        IEnumerator ShowLoading()
        {
            while (true)
            {
                txtLoading.text = "Loading . ";
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(1f);
                    txtLoading.text = txtLoading.text + ".";
                }
            }
        }
	 }
}