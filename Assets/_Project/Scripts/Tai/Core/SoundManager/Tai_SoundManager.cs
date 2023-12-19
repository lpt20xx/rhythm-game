using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using Hellmade.Sound;

namespace Tai_Core
{   
    public class Tai_SoundManager : SingletonMono<Tai_SoundManager>
    {
        public bool isMute = false;
        [SerializeField]
        private List<Tai_SoundItem> soundFxItems = new List<Tai_SoundItem>(); 

        private Dictionary<SoundFXIndex, Tai_SoundItem> dicSoundFxs = new Dictionary<SoundFXIndex, Tai_SoundItem>();

        private List<AudioClip> lsBGMs = new List<AudioClip> ();

        private AudioSource bgmSource;
        private void Awake()
        {
            dicSoundFxs.Clear();
            for (int i = 0; i < soundFxItems.Count; i++)
            {
                Debug.Log("dict: " + soundFxItems[i].soundFXindex);
                dicSoundFxs.Add(soundFxItems[i].soundFXindex, soundFxItems[i]);
            }

            bgmSource = GetComponent<AudioSource>();
        }

        #region BGM

        public void AddSoundBGM(AudioClip bgmClip)
        {
            if (isMute)
                return;


            lsBGMs.Clear();
            lsBGMs.Add(bgmClip);


        }

        //Get length clip
        public float GetLengthBGM()
        {
            Debug.Log("count: " + lsBGMs.Count);
            if (lsBGMs.Count > 0)
            {
                Debug.Log("length: " + lsBGMs[0].length);
                return lsBGMs[0].length;
            }

            return 0;
        }

        public void PlaySoundBGM(float volume = 1, bool isLoop = false)
        {
            bgmSource.clip = lsBGMs[0];
            bgmSource.Play();
            bgmSource.volume = volume;
            bgmSource.DOFade(volume, 0.25f);
        }

        public void PauseSoundBGM()
        {
            bgmSource.Pause();
        }

        public void ResumeSoundBGM()
        {
            bgmSource.Play();
        }

        public void StopSoundBGM()
        {
            bgmSource.Stop();
        }

        public float GetCurrentTimeSoundBGM()
        {
            if (bgmSource != null)
            {
                return bgmSource.time;
            }

            return 0;
        }

        #endregion

        #region SFX

        public void PlaySoundFX(SoundFXIndex soundIndex, bool isLoop = false)
        {

            if (isMute)
                return;

            EazySoundManager.PlaySound(dicSoundFxs[soundIndex].soundFxClip, isLoop);
        }

        public void StopSoundFX(SoundFXIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dicSoundFxs[soundIndex].soundFxClip);

            if (audio != null)
            {
                audio.Stop();
            }
        }

        public void StopAllSoundFX()
        {
            EazySoundManager.StopAllSounds();
        }

        public bool CheckSoundFXAvailable(SoundFXIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetAudio(dicSoundFxs[soundIndex].soundFxClip);
            if(audio != null && audio.IsPlaying)
            {
                return true;
            }

            return false;
        }

        #endregion

        public void Mute()
        {
            isMute = true;
            StopSoundBGM();
            //Stop all FX
            StopAllSoundFX();
        }

        public void Unmute()
        {
            for(SoundFXIndex i = SoundFXIndex.Click; i < SoundFXIndex.COUNT; i++)
            {
                StopSoundFX(i);
            }

            isMute = false;
        }

    }
    public enum SoundFXIndex
    {
        Click = 0,
        One,
        Two,
        Three,
        Go,
        SoundMenu,
        GameOver,
        MissNote,
        ConfirmMenu,
        Victory,
        COUNT
    }

    [Serializable]
    public class Tai_SoundItem
    {
        public SoundFXIndex soundFXindex;
        public AudioClip soundFxClip;
    }
}

