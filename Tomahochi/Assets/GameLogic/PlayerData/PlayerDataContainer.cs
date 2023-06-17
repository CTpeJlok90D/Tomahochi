using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Pets;
using System.Linq;

namespace Saving
{
	public class PlayerDataContainer : MonoBehaviour
	{
		[SerializeField] private PlayerData _playerData = new();
		[SerializeField] private int _secondsPassed = 0;
		[SerializeField] private Pet _startPet;
		[SerializeField] private UnityDictionarity<string, int> _foodInStorage = new();
		[SerializeField] private UnityDictionarity<string, int> _foodCookCount = new();
		[SerializeField] private UnityDictionarity<string, int> _ingridiendsInStorage = new();
		[SerializeField] private UnityDictionarity<string, int> _waterInStorage = new();

		private ReactiveVariable<int> _gemsCount = new(0);
		private ReactiveVariable<int> _moraCount = new(0);
		private ReactiveVariable<int> _fateCount = new(0);
		private ReactiveVariable<int> _rollCount = new(0);
		private UnityEvent<List<PetSaveInfo>> _unlockedPetsChanged = new();
		private UnityEvent<Food, int> _foodCountChanged = new();
		private UnityEvent<Ingredient, int> _ingridiendCountChanged = new();
		private UnityEvent<Water, int> _waterCountChanged = new();

		public static UnityEvent<List<PetSaveInfo>> unlockedPetsChanged => instance._unlockedPetsChanged;
		public static UnityEvent<Food, int> FoodCountChanged => instance._foodCountChanged;
		public static UnityEvent<Water, int> WaterCountChanged => instance._waterCountChanged;
		public static UnityEvent<Ingredient, int> IngridiendCountChanged => instance._ingridiendCountChanged;
		public static PetSaveInfo[] UnlockedPets => _instance._playerData.UnlockedPets.ToArray();
		public static UnityDictionarity<string, int> FoodInStorage => new(_instance._foodInStorage);
		public static UnityDictionarity<string, int> IngridiendsInStorage => new(_instance._ingridiendsInStorage);
		public static UnityDictionarity<string, int> FoodCookCount => new(_instance._foodCookCount);
		public static UnityDictionarity<string, int> WaterInStorage => new(_instance._waterInStorage);
		public static bool HaveInstance => _instance != null;

		private const string JSON_SAVE_KEY = "JSON_SAVE";
		private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";
		private static PlayerDataContainer instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<PlayerDataContainer>();
				}
				return _instance;
			}
		}

		private static PlayerDataContainer _instance;

		public static int SecondsPassed => instance._secondsPassed;
		public static Pet PetBySystemName(string name) => PetInfoBySystemName(name).Pet();
		public static PetSaveInfo PetInfoBySystemName(string name) => instance._playerData.UnlockedPets.Where((pet) => pet.SystemName == name).First();

		public static int RollCount
		{
			get => instance._rollCount.Value;
			set
			{
				instance._rollCount.Value = value;
				instance._playerData.LegendaryRollCount = value;
			}
		}

		public static int GemsCount
		{
			get => instance._gemsCount.Value;
			set
			{
				instance._playerData.GemsCount = value;
				instance._gemsCount.Value = value;
			}
		}
		public static UnityEvent<int> GemsCountChanged => instance._gemsCount.Changed;

		public static int MoraCount
		{
			get => instance._moraCount.Value;
			set
			{
				instance._moraCount.Value = value;
				instance._playerData.MoraCount = value;
			}
		}
		public static UnityEvent<int> MoraCountChanged => instance._moraCount.Changed;

		public static int FateCount
		{
			get => instance._fateCount.Value;
			set
			{
				instance._fateCount.Value = value;
				instance._playerData.FateCount = value;
			}
		}
		public static UnityEvent<int> FadeCountChanged => instance._gemsCount.Changed;

		public static void AddPet(PetSaveInfo info)
		{
			instance._playerData.UnlockedPets.Add(info);
			instance._unlockedPetsChanged.Invoke(instance._playerData.UnlockedPets);
		}

		public static int GetFoodCount(Food food)
		{
			return instance._foodInStorage[food.name];
		}

		public static void AddCookedFood(Food food, int count = 1)
		{
			if (instance._foodCookCount.Keys.Contains(food.name) == false)
			{
				instance._foodCookCount.Add(food.name, 0);
			}
			instance._foodCookCount[food.name] += count;
			AddFood(food, count);
		}

		public static void AddFood(Food food, int count = 1)
		{
			if (instance._foodInStorage.Keys.Contains(food.name) == false)
			{
				instance._foodInStorage.Add(food.name, 0);
			}
			instance._foodInStorage[food.name] += count;
			instance._foodCountChanged.Invoke(food, instance._foodInStorage[food.name]);
			SaveData();
		}
		public static void RemoveFood(Food food, int count = 1)
		{
			instance._foodInStorage[food.name] -= count;
			instance._foodCountChanged.Invoke(food, instance._foodInStorage[food.name]);
			SaveData();
		}
		public static int GetIngridientCount(Ingredient ingredient)
		{
			if (instance._ingridiendsInStorage.Keys.Contains(ingredient.name) == false)
			{
				return 0;
			}
			return instance._ingridiendsInStorage[ingredient.name];
		}

		public static void AddIngridient(Ingredient ingredient, int count = 1)
		{
			if (instance._ingridiendsInStorage.Keys.Contains(ingredient.name) == false)
			{
				instance._ingridiendsInStorage.Add(ingredient.name, 0);
			}
			instance._ingridiendsInStorage[ingredient.name] += count;
			instance._ingridiendCountChanged.Invoke(ingredient, instance._ingridiendsInStorage[ingredient.name]);
			SaveData();
		}

		public static void RemoveIngridient(Ingredient ingridient, int count = 1)
		{
			instance._ingridiendsInStorage[ingridient.name] -= count;
			instance._ingridiendCountChanged.Invoke(ingridient, instance._ingridiendsInStorage[ingridient.name]);
			SaveData();
		}

		public static void AddWater(Water water, int count)
		{
			if (instance._waterInStorage.Keys.Contains(water.name) == false)
			{
				instance._waterInStorage.Add(water.name, 0);
			}
			instance._waterInStorage[water.name] += count;
			instance._waterCountChanged.Invoke(water, instance._waterInStorage[water.name]);
			SaveData();
		}

		public static void RemoveWater(Water water, int count = 1)
		{
			instance._waterInStorage[water.name] -= count;
			instance._waterCountChanged.Invoke(water, instance._waterInStorage[water.name]);
			SaveData();
		}

		private void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
			_instance = this;

			LoadPlayerData();
			_secondsPassed = (int)(DateTime.UtcNow - DateTime.ParseExact(instance._playerData.LastLaunchTime, DATETIME_FORMAT, CultureInfo.InvariantCulture)).TotalSeconds;
			Pet.FallRatePetsByTime(UnlockedPets, _secondsPassed);
		}

		private void OnEnable()
		{
			GemsCountChanged.AddListener(UpdatePlayerData);
			MoraCountChanged.AddListener(UpdatePlayerData);
			FadeCountChanged.AddListener(UpdatePlayerData);
			FoodCountChanged.AddListener(UpdatePlayerData);
			WaterCountChanged.AddListener(UpdatePlayerData);
		}

		private void OnDisable()
		{
			GemsCountChanged.RemoveListener(UpdatePlayerData);
			MoraCountChanged.RemoveListener(UpdatePlayerData);
			FadeCountChanged.RemoveListener(UpdatePlayerData);
			FoodCountChanged.RemoveListener(UpdatePlayerData);
			WaterCountChanged.RemoveListener(UpdatePlayerData);
		}

		private void UpdatePlayerData(int value) => UpdatePlayerData();
		private void UpdatePlayerData(Food food, int value) => UpdatePlayerData();
		private void UpdatePlayerData(Water food, int value) => UpdatePlayerData();
		public void UpdatePlayerData()
		{
			_playerData.MoraCount = MoraCount;
			_playerData.FateCount = FateCount;
			_playerData.GemsCount = GemsCount;
			SaveData();
		}

		public static void SaveData()
		{
			instance._playerData.FoodInStorage = JsonUtility.ToJson(instance._foodInStorage);
			instance._playerData.IngridientsInStorage = JsonUtility.ToJson(instance._ingridiendsInStorage);
			instance._playerData.FoodCookCount = JsonUtility.ToJson(instance._foodCookCount);

			instance._playerData.LastLaunchTime = DateTime.UtcNow.ToString(DATETIME_FORMAT);

			// Менять ниже
			Debug.Log("Save player data");
			PlayerPrefs.SetString(JSON_SAVE_KEY, JsonUtility.ToJson(instance._playerData, true));
			// Менять выше
		}

		public static PlayerData LoadPlayerData()
		{
			// Менять ниже
			Debug.Log("Load player data");
			string playerDataJson = PlayerPrefs.GetString(JSON_SAVE_KEY);
			instance._playerData = JsonUtility.FromJson<PlayerData>(playerDataJson);
			// Менять выше

			if (instance._playerData == null)
			{
				LoadStartSavedPrefs();
			}

			instance._foodInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.FoodInStorage);
			instance._ingridiendsInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.IngridientsInStorage);
			instance._foodCookCount = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.FoodCookCount);

			return instance._playerData;
		}

		[MenuItem("PlayerPrefs/Load start values")]
		private static void LoadStartSavedPrefs()
		{
			instance._playerData = new()
			{
				UnlockedPets = new()
				{
					new()
					{
						SystemName = instance._startPet.name,
						Food = 50,
						Sleep = 50,
						Joy = 50,
						Water = 50
					}
				}
			};

			SaveData();
			Debug.Log($"Prefs loaded");
		}

#if UNITY_EDITOR
		[Header("Editor")]
		[SerializeField] private int selectedPetIndex;
		[SerializeField] private Food feedingFood;
		[SerializeField] private Water drinkingWater;
		[SerializeField] private Food addingFood;
		[SerializeField] private Ingredient addingingridient;

		[CustomEditor(typeof(PlayerDataContainer))]
		public class PlayerDataContainerEditor : Editor
		{
			new PlayerDataContainer target => base.target as PlayerDataContainer;
			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();
				if (Application.isPlaying == false)
				{
					return;
				}
				GUILayout.Label("Options", EditorStyles.boldLabel);
				if (GUILayout.Button("Feed"))
				{
					UnlockedPets[target.selectedPetIndex].Feed(target.feedingFood);
				}
				if (GUILayout.Button("Drink"))
				{
					UnlockedPets[target.selectedPetIndex].Drink(target.drinkingWater);
				}
				if (GUILayout.Button("Stroke"))
				{
					UnlockedPets[target.selectedPetIndex].Stroke();
				}
				if (GUILayout.Button("AddIngridient"))
				{
					AddIngridient(target.addingingridient, 1);
				}
				if (GUILayout.Button("AddFood"))
				{
					AddFood(target.addingFood, 1);
				}
			}
		}
#endif
	}
}