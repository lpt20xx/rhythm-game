using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using TMPro;
using Tai_Core;

namespace Tai
{
    public class RewardParam : UIParam
    {
        public int valueCoin;
    }
	public class Tai_UIReward : BaseUI
	{
        public TextMeshProUGUI txtCoin;

	    public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            RewardParam rewardParam = (RewardParam) param;
            txtCoin.text = "+" + rewardParam.valueCoin;

            Tai_GameManager.Instance.GameSave.Coin += rewardParam.valueCoin;
            SaveManager.Instance.SaveGame();

            Tai_UIMainMenu uiMainMenu = (Tai_UIMainMenu)UIManager.Instance.FindUIVisible(UIIndex.UIMainMenu);
            uiMainMenu.UpdateTextCoin();
        }


	}
}