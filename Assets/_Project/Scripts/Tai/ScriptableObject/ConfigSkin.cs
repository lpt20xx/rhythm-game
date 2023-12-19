using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using System;
namespace Tai
{
	[CreateAssetMenu(fileName = "Config Skin", menuName = "Config/Config Skin", order = 5)]
	public class ConfigSkin : ScriptableObject
	{
		public ConfigSkinData[] dataBoys;
		public ConfigSkinData[] dataGirls;

		private  static ConfigSkin Instance;

		public static ConfigSkinData GetConfigSkinDataBoy(int index) 
		{
			Instance = Resources.Load<ConfigSkin>("Configs/Config Skin");

			ConfigSkinData result = null;

            foreach (var go in Instance.dataBoys)
            {
                if (go.id == index)
				{
					result = go;
					break;
				}
            }

			if (result == null)
			{
				result = Instance.dataBoys[0];
			}

			return result;

        }

        public static ConfigSkinData GetConfigSkinDataGirl(int index)
        {
            Instance = Resources.Load<ConfigSkin>("Configs/Config Skin");

            ConfigSkinData result = null;

            foreach (var go in Instance.dataGirls)
            {
                if (go.id == index)
                {
                    result = go;
                    break;
                }
            }

            if (result == null)
            {
                result = Instance.dataGirls[0];
            }

            return result;

        }

		public static int GetBoySkinDataLength()
		{
			Instance = Resources.Load<ConfigSkin>("Configs/Config Skin");
			return Instance.dataBoys.Length;
		}

        public static int GetGirlSkinDataLength()
        {
            Instance = Resources.Load<ConfigSkin>("Configs/Config Skin");
            return Instance.dataGirls.Length;
        }
    }

	[Serializable]
	public class ConfigSkinData 
	{
		public int id;
		public RuntimeAnimatorController skinAnimator;
		public int coin;
		public Sprite spriteSkin;
	}

}