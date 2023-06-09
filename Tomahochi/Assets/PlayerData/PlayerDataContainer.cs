using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Pets;

namespace Saving
{
	public class PlayerDataContainer : MonoBehaviour
	{
		[SerializeField] private PlayerData _playerData = new();
		[SerializeField] private int _secondsPassed = 0;

		private ReactiveVariable<int> _gemsCount = new(0);
		private ReactiveVariable<int> _moraCount = new(0);
		private ReactiveVariable<int> _fateCount = new(0);
		private UnityEvent<List<PetSaveInfo>> _unlockedPetsChanged = new();
		public UnityEvent<List<PetSaveInfo>> unlockedPetsChanged => _unlockedPetsChanged;

		private const string JSON_SAVE_KEY = "JSON_SAVE";
		private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";

		private static PlayerDataContainer _instance;
		public static int SecondsPassed => _instance._secondsPassed;
		public static int GemsCount
		{
			get => _instance._gemsCount.Value;
			set => _instance._gemsCount.Value = value;
		}
		public static UnityEvent<int> GemsCountChanged => _instance._gemsCount.Changed;

		public static int MoraCount
		{
			get => _instance._moraCount.Value;
			set => _instance._moraCount.Value = value;
		}
		public static UnityEvent<int> MoraCountChanged => _instance._gemsCount.Changed;

		public static int FateCount
		{
			get => _instance._fateCount.Value; 
			set => _instance._fateCount.Value = value;
		}
		public static UnityEvent<int> FadeCountChanged => _instance._gemsCount.Changed;

		public static List<PetSaveInfo> PetList => _instance._playerData.UnlockedPets;

		public static void AddPet(PetSaveInfo info)
		{
			_instance._playerData.UnlockedPets.Add(info);
			_instance._unlockedPetsChanged.Invoke(_instance._playerData.UnlockedPets);
		}

		public static PetSaveInfo PetByName(string name)
		{
			return _instance._playerData.UnlockedPets.Where(pet => pet.SystemName == name).First();
		}

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
			_instance = this;

			LoadPlayerData();
			_secondsPassed = (int)(DateTime.UtcNow - DateTime.ParseExact(_instance._playerData.LastLaunchTime, DATETIME_FORMAT, CultureInfo.InvariantCulture)).TotalSeconds;
			Pet.FallRatePetsByTime(PetList, _secondsPassed);
		}

		private void OnEnable()
		{
			GemsCountChanged.AddListener(UpdatePlayerData);
			MoraCountChanged.AddListener(UpdatePlayerData);
			FadeCountChanged.AddListener(UpdatePlayerData);
		}

		private void OnDisable()
		{
			GemsCountChanged.RemoveListener(UpdatePlayerData);
			MoraCountChanged.RemoveListener(UpdatePlayerData);
			FadeCountChanged.RemoveListener(UpdatePlayerData);
		}

		private void UpdatePlayerData(int value) => UpdatePlayerData();
		public void UpdatePlayerData()
		{
			_playerData.MoraCount = MoraCount;
			_playerData.FateCount = FateCount;
			_playerData.GemsCount = GemsCount;
			SaveData();
		}

		public static void SaveData()
		{
			Debug.Log("Save player data");

			_instance._playerData.LastLaunchTime = DateTime.UtcNow.ToString(DATETIME_FORMAT);
			PlayerPrefs.SetString(JSON_SAVE_KEY, JsonUtility.ToJson(_instance._playerData));
		}

		public static PlayerData LoadPlayerData()
		{
			Debug.Log("Load player data");

			_instance._playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(JSON_SAVE_KEY));
			if (_instance._playerData == null)
			{
				LoadStartSavedPrefs();
			}
			return _instance._playerData;
		}

		[MenuItem("PlayerPrefs/Load start values")]
		private static void LoadStartSavedPrefs()
		{
			_instance._playerData = new()
			{
				UnlockedPets = new()
				{
					new()
					{
						SystemName = "TestPet",
						Hunger = 50,
						Polution = 50,
						Fatigue = 50,
						Play = 50
					}
				}
			};
			SaveData();
			Debug.Log($"Prefs loaded");
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			MoraCount = _playerData.MoraCount;
			FateCount = _playerData.FateCount;
			GemsCount = _playerData.GemsCount;
		}

		[InitializeOnLoadMethod]
		private static void Init()
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<PlayerDataContainer>();
				if (FindObjectsOfType<PlayerDataContainer>().Length == 0)
				{
					GameObject gameObject = new();
					gameObject.name = $"[{nameof(PlayerData)}]";
					gameObject.AddComponent<PlayerDataContainer>();
					gameObject.transform.SetAsLastSibling();
				}
			}
		}

		[MenuItem("PlayerPrefs/Clear")]
		private static void ClearPrefs()
		{
			PlayerPrefs.DeleteAll();
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
			SaveData();
			Debug.Log($"Saved new player prefs:");
			ShowPlayerPrefs();
		}

		[MenuItem("PlayerPrefs/Load current")]
		private static void LoadCurrentSavedPrefs()
		{
			LoadPlayerData();
			Debug.Log($"Prefs loaded");
		}
#endif
	}
}