using UnityEngine;

namespace Memory
{
	public class Save
	{
		internal static UnityDictionarity<string, string> Saves = new();
		internal static void LoadSaves()
		{
			try
			{
				Saves = JsonUtility.FromJson<UnityDictionarity<string, string>>(Init.Instance.PlayerData.Saves);
				Saves ??= new();
			}
			catch
			{
				Debug.Log("Loading defualt saves");
				Saves = new();
			}
		}

		public static bool ContainsSave(string saveName)
		{
			return Saves.Keys.Contains(saveName);
		}

		public Save(string name, object data)
		{
			Saves = JsonUtility.FromJson<UnityDictionarity<string, string>>(Init.Instance.PlayerData.Saves);
			Saves ??= new();

			Saves.Remove(name);
			Saves.Add(name, JsonUtility.ToJson(data));
			Init.Instance.PlayerData.Saves = JsonUtility.ToJson(Saves);
			Init.Instance.Save();
		}
	}

	public class Load<T>
	{
		private T _data;
		public T Data => _data;
		public Load(string saveName)
		{
			Save.LoadSaves();
			if (Save.Saves.Keys.Contains(saveName) == false)
			{
				_data = default;
				Debug.Log($"Failed to {saveName} save. Returning defualt value");
				return;
			}
			_data = JsonUtility.FromJson<T>(Save.Saves[saveName]);
		}

		public static implicit operator T(Load<T> load)
		{
			return load.Data;
		}
	}
}