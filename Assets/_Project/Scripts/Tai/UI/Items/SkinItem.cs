using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tai;
using Tai_Core;

public class SkinItem : MonoBehaviour
{
    [SerializeField] private GameObject goPrice;
    [SerializeField] private Image imgSkin;

    private Tai_UISkin parent;

    //Config skin data
    private ConfigSkinData configSkinData;

    private bool isBought;

    private bool isSkinGirl;

    [SerializeField] private Sprite spriteDeselect;
    [SerializeField] private Sprite spriteSelect;

    [SerializeField] private Sprite spriteGirlDeselect;

    private const int valueBoughtSkin = 1000;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnSkin_Clicked());

        goPrice.GetComponent<Button>().onClick.AddListener(() => OnBuy_Clicked());

    }

    public void OnSkin_Clicked()
    {
        Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
        if (!goPrice.activeSelf)
        {
            GetComponent<Image>().sprite = spriteSelect;
            if (isSkinGirl)
            {
                // Change skin for girl
                parent.OnChangeSkinGirl(configSkinData.spriteSkin, this);
            }
            else
            {
                // Change skin for boy
                parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);
            }
        }
        
    }

    public void OnSkin_Clicked(bool isGirl)
    {
        GetComponent<Image>().sprite = spriteSelect;

        if (isGirl)
        {
            // Change skin for girl
            parent.OnChangeSkinGirl(configSkinData.spriteSkin, this);
        }
        else
        {
            // Change skin for boy
            parent.OnChangeSkinBoy(configSkinData.spriteSkin, this);
        }
    }

    public void OnDeselect()
    {
        GetComponent<Image>().sprite = spriteDeselect;
    }

    public void OnSetup(Tai_UISkin parent, ConfigSkinData configSkinData, bool isBought, bool isSkinGirl = true)
    {
        this.parent = parent;
        this.isBought = isBought;
        this.isSkinGirl = isSkinGirl;

        this.configSkinData = configSkinData;

        GetComponent<Image>().sprite = spriteDeselect;

        if (configSkinData.coin == 0 || isBought)
        {
            goPrice.SetActive(false);
            // Set sprite skin
            imgSkin.sprite = configSkinData.spriteSkin;
        }
        else
        {
            imgSkin.sprite = spriteGirlDeselect;
            goPrice.SetActive(true);
        }
    }

    public void OnBuy_Clicked()
    {
        Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);

        if(Tai_GameManager.Instance.GameSave.Coin >= valueBoughtSkin && !isBought) 
        {
            isBought = true;
            Tai_GameManager.Instance.GameSave.Coin -= valueBoughtSkin;
            goPrice.SetActive(false);

            if (isSkinGirl)
            {

                //id get from config
                int idSkinGirl = configSkinData.id;
                Tai_GameManager.Instance.GameSave.GirlSkinBoughts.Add(idSkinGirl);
            }
            else
            {
                //id get from config
                int idSkinBoy = configSkinData.id;
                Tai_GameManager.Instance.GameSave.BoySkinBoughts.Add(idSkinBoy);

            }

            // Update text coin
            parent.UpdateTextCoin();
            // Set sprite for imgSkin
            imgSkin.sprite = configSkinData.spriteSkin;
        }
        OnSkin_Clicked();
    }
}
