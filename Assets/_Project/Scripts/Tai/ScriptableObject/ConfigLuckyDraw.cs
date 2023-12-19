using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using System;

namespace Tai
{
	[CreateAssetMenu(fileName = "Config Lucky Draw", menuName = "Config/Config Lucky Draw", order = 0)]
	public class ConfigLuckyDraw : ScriptableObject
	{
		public ConfigLuckyDrawData[] data;
		private static ConfigLuckyDraw Instance;

		public static ConfigLuckyDrawData GetConfigLuckyDrawData(int index)
		{
			Instance = Resources.Load<ConfigLuckyDraw>("Configs/Config Lucky Draw");
			ConfigLuckyDrawData result = null;
            foreach (var go in Instance.data)
            {
                if(go.id == index)
				{
					return go;
					break;
				}
            }

			if (result == null)
			{
				result = Instance.data[0];
			}

			return result;
        }
    }

	[Serializable]
	public class ConfigLuckyDrawData
	{
		public int id;
		public int coin;
	}
}