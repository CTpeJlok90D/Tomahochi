using System;
using UnityEngine;

namespace Memory
{
	public class Save
	{
		internal static UnityDictionarity<string, string> Saves = new();

		public Save(string name, object data)
		{
			Saves.Remove(name);
			Saves.Add(name, JsonUtility.ToJson(data));
			Init.Instance.PlayerData.Saves = JsonUtility.ToJson(data);
			Init.Instance.Save();
		}
	}

	public static class Loader
	{
		public static T Load<T>(string saveName)
		{
			try
			{
				Save.Saves = JsonUtility.FromJson<UnityDictionarity<string, string>>(Init.Instance.PlayerData.Saves);
			}
			catch
			{
				Debug.Log("Loading defualt saves");
				Save.Saves = new();
			}
			try
			{
				return JsonUtility.FromJson<T>(Save.Saves[saveName]);
			}
			catch (Exception e)
			{
				Debug.Log($"Loading save {saveName} failed");
				Debug.LogError(e);
			}
			return default;
		}
	}
}