using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tai_Core
{
    public interface IGameAds
    {
        void Init();

        void Update();

        void ShowBanner();

        void HideBanner();

        bool IsRewardedReady();

        void LoadRewardedVideo();

        void ShowRewardedVideo(Action finished, Action watchFailed);

        bool IsInterstitialReady();

        void LoadInterstitial();

        void ShowInterstitial(Action finished);
    }
}

