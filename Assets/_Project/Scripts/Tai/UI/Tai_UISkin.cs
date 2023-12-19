using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using UnityEngine.UI;
using TMPro;
using Tai_Core;

namespace Tai
{
	public class Tai_UISkin : BaseUI
	{
        [SerializeField] private Image imgGirl;
        [SerializeField] private Image imgBoy;

        [SerializeField] private List<SkinItem> lsSkinBoyItems = new List<SkinItem>();
        [SerializeField] private List<SkinItem> lsSkinGirlItems = new List<SkinItem>();

        private SkinItem curBoySkinItem;
        private SkinItem curGirlSkinItem;

        [SerializeField] private GameObject goSrollRectBoy;
        [SerializeField] private GameObject goSrollRectGirl;

        [SerializeField] private Button btnAds;
        [SerializeField] private Button btnBoy;
        [SerializeField] private Button btnGirl;

        [SerializeField] private Sprite spriteBtnBoySelect;
        [SerializeField] private Sprite spriteBtnGirlSelect;

        [SerializeField] private Sprite spriteBtnBoyDeselect;
        [SerializeField] private Sprite spriteBtnGirlDeselect;

        [SerializeField] private TextMeshProUGUI txtCoin; 

        [SerializeField] private Animator boyAnimator;
        [SerializeField] private Animator girlAnimator;
        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSetup(UIParam param = null)
        {
            base.OnSetup(param);
            for (int i = 0; i < ConfigSkin.GetBoySkinDataLength(); i++)
            {
                ConfigSkinData configSkinData = ConfigSkin.GetConfigSkinDataBoy(i);
                bool isBought = Tai_GameManager.Instance.GameSave.BoySkinBoughts.IndexOf(i) >= 0;
                lsSkinBoyItems[i].OnSetup(this, configSkinData, isBought, false);
            }

            for (int i = 0; i < ConfigSkin.GetGirlSkinDataLength(); i++)
            {
                ConfigSkinData configSkinData = ConfigSkin.GetConfigSkinDataGirl(i);
                bool isBought = Tai_GameManager.Instance.GameSave.GirlSkinBoughts.IndexOf(i) >= 0;
                lsSkinGirlItems[i].OnSetup(this, configSkinData, isBought, true);
                Debug.Log("Girl: " + i + " " + configSkinData.id);
            }

            txtCoin.text = Tai_GameManager.Instance.GameSave.Coin.ToString();
            lsSkinBoyItems[Tai_GameManager.Instance.GameSave.CurrentIndexBoy].OnSkin_Clicked(false);
            lsSkinGirlItems[Tai_GameManager.Instance.GameSave.CurrentIndexGirl].OnSkin_Clicked();
            // Show Boy Skin
            OnShowBoySkinClick();
        }

        public void OnChangeSkinGirl(Sprite spriteGirl, SkinItem skinItem)
        {
            if (curGirlSkinItem != null && curGirlSkinItem != skinItem)
            {
                curGirlSkinItem.OnDeselect();
            }

            curGirlSkinItem = skinItem;
            int indexSkin = lsSkinGirlItems.IndexOf(skinItem);
            girlAnimator.SetFloat("Index", indexSkin);

            imgGirl.sprite = spriteGirl;
            //imgGirl.SetNativeSize();

            bool isBought = Tai_GameManager.Instance.GameSave.GirlSkinBoughts.IndexOf(indexSkin) >= 0;
            if (isBought)
            {
                Tai_GameManager.Instance.GameSave.CurrentIndexGirl = indexSkin;
            }
        }

        public void OnChangeSkinBoy(Sprite spriteBoy, SkinItem skinItem)
        {
            if (curBoySkinItem != null && curBoySkinItem != skinItem)
            {
                curBoySkinItem.OnDeselect();
            }

            curBoySkinItem = skinItem;
            int indexSkin = lsSkinBoyItems.IndexOf(skinItem);
            boyAnimator.SetFloat("Index", indexSkin);

            imgBoy.sprite = spriteBoy;
            //imgBoy.SetNativeSize();

            bool isBought = Tai_GameManager.Instance.GameSave.BoySkinBoughts.IndexOf(indexSkin) >= 0;
            if (isBought)
            {
                Tai_GameManager.Instance.GameSave.CurrentIndexBoy = indexSkin;
            }
        }

        public void OnShowBoySkinClick()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);

            btnBoy.enabled = false;
            btnBoy.image.sprite = spriteBtnBoySelect;

            btnGirl.enabled = true;
            btnGirl.image.sprite = spriteBtnGirlDeselect;

            goSrollRectBoy.SetActive(true);
            goSrollRectGirl.SetActive(false);
        }

        public void OnShowGirlSkinClick()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);

            btnGirl.enabled = false;
            btnGirl.image.sprite = spriteBtnGirlSelect;

            btnBoy.enabled = true;
            btnBoy.image.sprite = spriteBtnBoyDeselect;

            goSrollRectBoy.SetActive(false);
            goSrollRectGirl.SetActive(true);
        }

        public void OnAds_Clicked()
        {
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);

            // Show Ads
            AdsManager.Instance.ShowRewardedAds( () =>
            {
                Debug.Log("Finished reward ads");
                Tai_GameManager.Instance.GameSave.Coin += 100;
                txtCoin.text = Tai_GameManager.Instance.GameSave.Coin.ToString();
            });
        }

        public void UpdateTextCoin()
        {
            txtCoin.text = Tai_GameManager.Instance.GameSave.Coin.ToString();
        }

        public override void OnCloseClick()
        {
            base.OnCloseClick();
            UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
            Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
            if (Tai_GameManager.Instance.GameSave.isFirstOpen)
            {
                // Show Inter ads
            }
        }
    }
}