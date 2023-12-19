using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using System;

namespace Tai
{
	 public class Tai_UIRewardLogin : BaseUI
	 {
        [SerializeField] private List<SlotItem> lsSlotItems = new List<SlotItem>();
		public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            for (int  i = 0; i < lsSlotItems.Count; i++)
            {
                // Get config daily reward
                ConfigDailyLoginData configDailyLoginData = ConfigDailyLogin.GetDailyLoginData(i);

                //Debug.Log("Config: " + configDailyLoginData.coin + " " + configDailyLoginData.id);
                Debug.Log("Login: " + Tai_GameManager.Instance.GameSave.CurrentDay + " " +
                        Tai_GameManager.Instance.GameSave.CurrentDayOfWeekLogin);

                int currentDayLogin = Tai_GameManager.Instance.GameSave.CurrentDayLogin;
                int currentWeekLogin = Tai_GameManager.Instance.GameSave.CurrentDayOfWeekLogin;

                
                int coin = configDailyLoginData.coin;

                if (DateTime.Now.DayOfYear - currentWeekLogin == currentDayLogin)
                {
                    // Get coin from config
                    lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, i == currentDayLogin);
                }
                else
                {
                    if (DateTime.Now.DayOfYear - currentWeekLogin < currentDayLogin)
                    {
                        lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, false);
                    }
                    else
                    {
                        lsSlotItems[i].OnSetup(i, coin, i >= currentDayLogin, i == currentDayLogin);
                    }
                }
            }
        }

        public override void OnCloseClick()
        {
            base.OnCloseClick();
            Tai_UIMainMenu uiMainMenu = (Tai_UIMainMenu) UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
            uiMainMenu.UpdateTextCoin();
            // Show inter ads
        }
    }
}