using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using Tai_Core;
using System.Reflection;

namespace Tai
{
    public class Tai_UILuckyDraw : BaseUI
    {
        [SerializeField] private Image imgDraw;
        [SerializeField] private TextMeshProUGUI txtCountDown;
        [SerializeField] private List<TextMeshProUGUI> lsTextCoins = new List<TextMeshProUGUI>();

        [SerializeField] private Button btnFree;

        private double timerCountdown;
        private const double ValueTimerCountdown = 7199;
        private bool isShowCountdown = false;
        private bool isAds = false;

        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            timerCountdown = (Tai_GameManager.Instance.GameSave.CountdownLuckyDraw -
                              TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds);

            if (timerCountdown > 0)
            {
                isShowCountdown = true;
            }
            else
            {
                isShowCountdown = false;
                txtCountDown.text = "FREE";
            }

            btnFree.interactable = !isShowCountdown;

            //Set text coin from config LuckyDraw
            for (int i = 0; i < lsTextCoins.Count; i++)
            {
                ConfigLuckyDrawData luckyDrawData = ConfigLuckyDraw.GetConfigLuckyDrawData(i);
                lsTextCoins[i].text = luckyDrawData.coin.ToString();
            }

            imgDraw.transform.eulerAngles = new Vector3(0, 0, -30);
        }

        private void Spin()
        {
            float randTimer = Random.Range(3.5f, 5f);
            Transform transWheelCircle = imgDraw.transform;
            transWheelCircle.eulerAngles = new Vector3(0, 0, -30);

            float pieceAngle = 360 / lsTextCoins.Count;
            float halfPieceAngle = pieceAngle / 2;
            float halfPieceAngleWithPadding = halfPieceAngle - (halfPieceAngle / 4);

            int randIndex = Random.Range(0, lsTextCoins.Count);
            Debug.Log("random: " + randIndex + " " + lsTextCoins[randIndex].text);

            float angle = -(pieceAngle * randIndex);

            float rightOffset = (angle - halfPieceAngleWithPadding) % 360;
            float leftOffset = (angle + halfPieceAngleWithPadding) % 360;

            float randomAngle = Random.Range(leftOffset, rightOffset);
            randomAngle = randIndex * 60 + 30;

            Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * 5);
            Debug.Log("Rotation: " + targetRotation);


            float prevAngle, curAngle;
            prevAngle = curAngle = transWheelCircle.eulerAngles.z;

            transWheelCircle
                .DORotate(targetRotation, randTimer, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuart)
                .OnUpdate(() =>
                {
                    //Debug.Log("Rotate: ");
                    float diff = Mathf.Abs(prevAngle - curAngle);
                    if(diff >= halfPieceAngle)
                    {
                        prevAngle = curAngle;
                    }

                    curAngle = transWheelCircle.eulerAngles.z;
                })
                .OnComplete(() =>
                {
                    Timer.DelayedCall(1, () =>
                    {
                        //int indexLuckyDraw = Mathf.Abs(Mathf.RoundToInt(transWheelCircle.eulerAngles.z / 60));
                        //Debug.Log("index: " + indexLuckyDraw + " " + lsTextCoins[indexLuckyDraw].text);

                        // Get config depend Index to coin
                        ConfigLuckyDrawData luckyDrawData = ConfigLuckyDraw.GetConfigLuckyDrawData(randIndex);
                        // Show Reward
                        UIManager.Instance.ShowUI(UIIndex.UIReward, new RewardParam()
                        {
                            valueCoin = luckyDrawData.coin
                        });
                        if (!isAds)
                        {
                            timerCountdown = ValueTimerCountdown;
                            isShowCountdown = true;
                            // Show timer
                            ShowTimer();
                        }

                    }, this);
                });
        }

        private void ShowTimer()
        {
            int second = (int)(timerCountdown % 60);
            int minutes = (int) (timerCountdown / 60) % 60;
            int hour = (int)(timerCountdown / 60) / 60 % 60;
            txtCountDown.text = string.Format("{0:0}:{1:00}:{2:00}", hour, minutes, second);
        }

        private void Update()
        {
            if (isShowCountdown)
            {
                timerCountdown -= Time.deltaTime;
                ShowTimer();
                if (timerCountdown <= 0)
                {
                    timerCountdown = 0;
                    isShowCountdown= false;
                    txtCountDown.text = "FREE";
                    btnFree.interactable = true;
                }
            }
        }

        public void OnFree_Clicked()
        {
            isAds = false;
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            Tai_GameManager.Instance.GameSave.CountdownLuckyDraw =
                TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds + ValueTimerCountdown;
            btnFree.interactable = false;
            Spin();
        }

        public void OnAds_Clicked()
        {
            
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            // Show Reward Ads
            AdsManager.Instance.ShowRewardedAds(() =>
            {
                isAds = true;
                Spin();
            });
            
        }

        public override void OnCloseClick()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            base.OnCloseClick();
            // Show Inter ads

            if(UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu) != null)
            {
                Tai_UIMainMenu uiMainMenu = (Tai_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
                // Update Currency
            }
        }
    }
}