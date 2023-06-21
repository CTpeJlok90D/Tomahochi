using Saving;
using UnityEditor;
using UnityEngine;

public static class PlayerPrefsTweaks
{
	[MenuItem("PlayerPrefs/Clear")]
	private static void ClearPrefs()
	{
		PlayerPrefs.DeleteAll();
		PlayerDataContainer.LoadStartSavedPrefs();
		Debug.Log("Prefs cleared");
	}

	[MenuItem("PlayerPrefs/Show")]
	private static void ShowPlayerPrefs()
	{
		Debug.Log(PlayerPrefs.GetString("JSON_SAVE"));
	}

	[MenuItem("PlayerPrefs/Save")]
	private static void SaveCurrentPrefs()
	{
		PlayerDataContainer.SaveData();
		Debug.Log($"Saved new player prefs:");
		ShowPlayerPrefs();
	}

	[MenuItem("PlayerPrefs/Load current")]
	private static void LoadCurrentSavedPrefs()
	{
		PlayerDataContainer.LoadPlayerData();
	}
}
