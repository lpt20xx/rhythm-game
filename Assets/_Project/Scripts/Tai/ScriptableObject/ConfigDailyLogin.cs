using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tai;
using System;

namespace Tai
{
	[CreateAssetMenu(fileName = "Config Daily Login Reward", menuName = "Config/Config Daily Login", order = 1)]
	public class ConfigDailyLogin : ScriptableObject
	{
		public ConfigDailyLoginData[] data;
		private static ConfigDailyLogin Instance;

		public static ConfigDailyLoginData GetDailyLoginData(int index)
		{
			Instance = Resources.Load<ConfigDailyLogin>("Configs/Config Daily Login Reward");
			ConfigDailyLoginData result = null;
			foreach (var go in Instance.data)
			{
				if (go.id == index)
				{
					result = go;
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
	public class ConfigDailyLoginData
	{
		public string day;
		public int id;
		public int coin;
	}
}