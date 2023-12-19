using System.Collections;
using System.Collections.Generic;
using Tai;
using Tai_Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private GameObject goFade;

    private int valueCoin;
    private int indexLogin;

    private Button btnClick;

    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;

    private void Awake()
    {
        btnClick = GetComponent<Button>();
        btnClick.onClick.AddListener(() =>
        {
            OnLogin_Clicked();
        });
    }

    public void OnSetup(int index, int coin, bool getReward, bool isToday)
    {
        valueCoin = coin;
        indexLogin = index;
        txtCoin.text = valueCoin.ToString();

        GetComponent<Image>().sprite = spriteOff;

        if(!getReward)
        {
            DisableSlot();
        }
        else
        {
            btnClick.enabled = isToday;
            if(isToday)
            {
                GetComponent<Image>().sprite = spriteOn;
            }
        }
    }

    private void OnLogin_Clicked()
    {
        Tai_SoundManager.Instance.PlaySoundFX(SoundFXIndex.Click);
        DisableSlot();
        // Show UI Reward
        UIManager.Instance.ShowUI(UIIndex.UIReward, new RewardParam() {valueCoin = valueCoin});

        indexLogin++;

        if(indexLogin >= 7)
        {
            indexLogin = 0;
        }

        Tai_GameManager.Instance.GameSave.CurrentDayLogin = indexLogin;
    }

    private void DisableSlot()
    {
        GetComponent<Image>().sprite = spriteOff;
        goFade.SetActive(true);
        btnClick.interactable = false;
    }
}
