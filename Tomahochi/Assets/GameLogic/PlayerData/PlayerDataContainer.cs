//#define DEBUG_SAVE_LOAD

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
		[SerializeField] private UnityDictionarity<string, Furniture> _furnitureById = new();
		[SerializeField] private UnityDictionarity<string, int> _furnitureInStorage = new();


		private ReactiveVariable<int> _gemsCount = new(0);
		private ReactiveVariable<int> _moraCount = new(0);
		private ReactiveVariable<int> _fateCount = new(0);
		private UnityEvent<List<PetSaveInfo>> _unlockedPetsChanged = new();
		private UnityEvent<Food, int> _foodCountChanged = new();
		private UnityEvent<Ingredient, int> _ingridiendCountChanged = new();
		private UnityEvent<Water, int> _waterCountChanged = new();
		public delegate void FurnitureCountChangedHandler(string name, int count);
		private event FurnitureCountChangedHandler _furnitureOnStorageCountChanged;

		public static UnityEvent<List<PetSaveInfo>> unlockedPetsChanged => instance._unlockedPetsChanged;
		public static UnityEvent<Food, int> FoodCountChanged => instance._foodCountChanged;
		public static UnityEvent<Water, int> WaterCountChanged => instance._waterCountChanged;
		public static UnityEvent<Ingredient, int> IngridiendCountChanged => instance._ingridiendCountChanged;
		public static event FurnitureCountChangedHandler FurnitureOnStarageCountChanged
		{
			add
			{
				_instance._furnitureOnStorageCountChanged += value;
			}
			remove
			{
				_instance._furnitureOnStorageCountChanged -= value;
			}
		}
		public static PetSaveInfo[] UnlockedPets => _instance._playerData.UnlockedPets.ToArray();
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
			GemsCountChanged.AddListener(SaveData);
			MoraCountChanged.AddListener(SaveData);
			FadeCountChanged.AddListener(SaveData);
			FoodCountChanged.AddListener(SaveData);
			WaterCountChanged.AddListener(SaveData);
		}
		private void OnDisable()
		{
			GemsCountChanged.RemoveListener(SaveData);
			MoraCountChanged.RemoveListener(SaveData);
			FadeCountChanged.RemoveListener(SaveData);
			FoodCountChanged.RemoveListener(SaveData);
			WaterCountChanged.RemoveListener(SaveData);
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
		public static int MoraCount
		{
			get => instance._moraCount.Value;
			set
			{
				instance._moraCount.Value = value;
				instance._playerData.MoraCount = value;
			}
		}
		public static int FateCount
		{
			get => instance._fateCount.Value;
			set
			{
				instance._fateCount.Value = value;
				instance._playerData.FateCount = value;
			}
		}
		public static UnityEvent<int> GemsCountChanged => instance._gemsCount.Changed;
		public static UnityEvent<int> MoraCountChanged => instance._moraCount.Changed;
		public static UnityEvent<int> FadeCountChanged => instance._gemsCount.Changed;
		public static void AddPet(PetSaveInfo info)
		{
			instance._playerData.UnlockedPets.Add(info);
			instance._unlockedPetsChanged.Invoke(instance._playerData.UnlockedPets);
		}
		public static int GetFoodCount(Food food)
		{
			return GetFoodCount(food.name);
		}
		public static int GetFoodCount(string foodName)
		{
			if (instance._foodInStorage.Keys.Contains(foodName) == false)
			{
				return 0;
			}
			return instance._foodInStorage[foodName];
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
			if (_instance._foodInStorage.Keys.Contains(food.name) == false)
			{
				return;
			}
			instance._foodInStorage[food.name] = (int)Mathf.Clamp(instance._foodInStorage[food.name] - count, 0, Mathf.Infinity);
			if (_instance._foodInStorage[food.name] == 0)
			{
				_instance._foodInStorage.Remove(food.name);
			}
			SaveData();
			instance._foodCountChanged.Invoke(food, instance._foodInStorage[food.name]);
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
			instance._ingridiendsInStorage[ingridient.name] = (int)Mathf.Clamp(instance._ingridiendsInStorage[ingridient.name] - count, 0, Mathf.Infinity);
			instance._ingridiendCountChanged.Invoke(ingridient, instance._ingridiendsInStorage[ingridient.name]);
			SaveData();
		}
		public static int GetWaterCount(Water water)
		{
			if (instance._waterInStorage.Keys.Contains(water.name) == false)
			{
				return 0;
			}
			return instance._waterInStorage[water.name];
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
			if (_instance._waterInStorage.Keys.Contains(water.name) == false)
			{
				return;
			}
			instance._waterInStorage[water.name] = (int)Mathf.Clamp(instance._waterInStorage[water.name] - count, 0, Mathf.Infinity);
			if (_instance._waterInStorage[water.name] == 0)
			{
				_instance._waterInStorage.Remove(water.name);
			}
			SaveData();
			instance._waterCountChanged.Invoke(water, instance._waterInStorage[water.name]);
		}
		public static string[] GetAllFurnitureId()
		{
			return instance._furnitureById.Keys.ToArray();
		}
		public static Furniture GetFurnitureByID(string id)
		{
			if (_instance._furnitureById.Keys.Contains(id) == false)
			{
				return null;
			}
			return _instance._furnitureById[id];
		}
		public static void PlaceFurniture(Furniture furniture)
		{
			if (GetFurnitureCountOnStorage(furniture.SystemName) <= 0)
			{
				return;
			}
			_instance._furnitureById.Add(furniture.ID, furniture);
			RemoveFurnitureFromStorage(furniture.SystemName, 1);
			SaveData();
		}
		public static void AddFurnitureInStorage(string systemName, int count)
		{
			if (_instance._furnitureInStorage.Keys.Contains(systemName) == false)
			{
				_instance._furnitureInStorage.Add(systemName, 0);
			}
			_instance._furnitureInStorage[systemName] += count;
			_instance._furnitureOnStorageCountChanged?.Invoke(systemName, GetFurnitureCountOnStorage(systemName));
			SaveData();
		}
		public static void RemoveFurnitureFromStorage(string systemName, int count)
		{
			if (_instance._furnitureInStorage.Keys.Contains(systemName) == false)
			{
				return;
			}
			_instance._furnitureInStorage[systemName] = (int)Mathf.Clamp(_instance._furnitureInStorage[systemName] - count, 0, Mathf.Infinity);
			if (_instance._furnitureInStorage[systemName] == 0)
			{
				_instance._furnitureInStorage.Remove(systemName);
			}
			_instance._furnitureOnStorageCountChanged?.Invoke(systemName, GetFurnitureCountOnStorage(systemName));
			SaveData();
		}
		public static int GetFurnitureCountOnStorage(string systemName)
		{
			if (_instance._furnitureInStorage.Keys.Contains(systemName) == false)
			{
				return 0;
			}
			return _instance._furnitureInStorage[systemName];
		}
		public static string[] GetAllFurnitureFromStorage()
		{
			return _instance._furnitureInStorage.Keys.ToArray();
		}

		private void SaveData(int value) => SaveData();
		private void SaveData(Food food, int value) => SaveData();
		private void SaveData(Water food, int value) => SaveData();
		public static void SaveData()
		{
			PlayerData data = instance._playerData;

			data.MoraCount = MoraCount;
			data.FateCount = FateCount;
			data.GemsCount = GemsCount;

			data.FoodInStorage = JsonUtility.ToJson(instance._foodInStorage);
			data.IngridientsInStorage = JsonUtility.ToJson(instance._ingridiendsInStorage);
			data.WaterInStorage = JsonUtility.ToJson(instance._waterInStorage);
			data.FoodCookCount = JsonUtility.ToJson(instance._foodCookCount);
			data.FurnitureList = JsonUtility.ToJson(instance._furnitureById);
			data.FurnitureInStorage = JsonUtility.ToJson(instance._furnitureInStorage);

			data.LastLaunchTime = DateTime.UtcNow.ToString(DATETIME_FORMAT);

			// Менять ниже
#if DEBUG_SAVE_LOAD
			Debug.Log("Save player data");
#endif
			PlayerPrefs.SetString(JSON_SAVE_KEY, JsonUtility.ToJson(instance._playerData, true));
			// Менять выше
		}
		public static PlayerData LoadPlayerData()
		{
			// Менять ниже
#if DEBUG_SAVE_LOAD
			Debug.Log("Load player data");
#endif
			string playerDataJson = PlayerPrefs.GetString(JSON_SAVE_KEY);
			instance._playerData = JsonUtility.FromJson<PlayerData>(playerDataJson);
			// Менять выше

			instance._playerData ??= StartPlayerData;

			instance._foodInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.FoodInStorage);
			instance._waterInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.WaterInStorage);
			instance._ingridiendsInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.IngridientsInStorage);
			instance._foodCookCount = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.FoodCookCount);
			instance._furnitureById = JsonUtility.FromJson<UnityDictionarity<string, Furniture>>(instance._playerData.FurnitureList);
			instance._furnitureInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance._playerData.FurnitureInStorage);

			instance._foodInStorage ??= new();
			instance._waterInStorage ??= new();
			instance._ingridiendsInStorage ??= new();
			instance._foodCookCount ??= new();
			instance._furnitureById ??= new();
			instance._furnitureInStorage ??= new();

			GemsCount = instance._playerData.GemsCount;
			MoraCount = instance._playerData.MoraCount;
			FateCount = instance._playerData.FateCount;

			return instance._playerData;
		}

#if UNITY_EDITOR
		[MenuItem("PlayerPrefs/Load start values")]
		public static void LoadStartSavedPrefs()
		{
			instance._playerData = StartPlayerData;
			LoadPlayerData();
			Debug.Log($"Prefs loaded");
		}

		private static PlayerData StartPlayerData
		{
			get => new()
			{
				UnlockedPets = new List<PetSaveInfo>()
				{
					new PetSaveInfo()
					{
						SystemName = instance._startPet.name,
						Food = 50,
						Energy = 50,
						Joy = 50,
						Water = 50
					}
				},
			};
		}

		[Header("Editor")]
		[SerializeField] private int selectedPetIndex;
		[SerializeField] private Food feedingFood;
		[SerializeField] private Water drinkingWater;
		[SerializeField] private Food addingFood;
		[SerializeField] private Water addingWater;
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
				if (GUILayout.Button("Add water"))
				{
					AddWater(target.addingWater, 1);
				}
			}
		}
#endif
	}
}