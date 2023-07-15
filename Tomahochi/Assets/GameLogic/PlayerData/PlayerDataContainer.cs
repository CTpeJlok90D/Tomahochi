using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using Pets;
using System.Linq;
using UnityExtentions;
using System.Data;

namespace Saving
{
	public class PlayerDataContainer : Init
	{
		public static int PlayingPetIndex = 0;
		public const int PetCountPerRoom = 4;
		[Space(50)]
		[Header("PlayerDataContainer")]
		[SerializeField] private int _secondsPassed = 0;
		[SerializeField] private UnityDictionarity<string, int> _foodInStorage = new();
		[SerializeField] private UnityDictionarity<string, int> _foodCookCount = new();
		[SerializeField] private UnityDictionarity<string, int> _ingridiendsInStorage = new();
		[SerializeField] private UnityDictionarity<string, int> _waterInStorage = new();
		[SerializeField] private UnityDictionarity<string, Furniture> _furnitureById = new();
		[SerializeField] private UnityDictionarity<string, int> _furnitureInStorage = new();
		[SerializeField] private UnityDictionarity<int, float> _minigameRecords = new();
		[SerializeField] private SerializedList<PetSaveInfo> _unlockedPets = new();
		[SerializeField] private Home _home = new();
		

		private ReactiveVariable<int> _gemsCount = new(0);
		private ReactiveVariable<int> _moraCount = new(0);
		private ReactiveVariable<int> _fateCount = new(0);
		private ReactiveVariable<int> _rollCount = new(0);
		private bool _isLoaded = false;
		private UnityEvent<PetSaveInfo> _unlockedNewPet = new();
		private UnityEvent<Storageble, int> _foodCountChanged = new();
		private UnityEvent<Storageble, int> _ingridiendCountChanged = new();
		private UnityEvent<Storageble, int> _waterCountChanged = new();
		private UnityEvent<Storageble, int> _furnitureOnStorageCountChanged = new();
		private UnityEvent _loaded = new();

		public static UnityEvent<PetSaveInfo> UnlockedNewPet => instance._unlockedNewPet;
		public static UnityEvent<Storageble, int> FoodCountChanged => instance._foodCountChanged;
		public static UnityEvent<Storageble, int> WaterCountChanged => instance._waterCountChanged;
		public static UnityEvent<Storageble, int> IngridiendCountChanged => instance._ingridiendCountChanged;
		public static UnityEvent<Storageble, int> FurnitureOnStarageCountChanged => _instance._furnitureOnStorageCountChanged;
		public static UnityEvent Loaded => _instance._loaded;
		public static PetSaveInfo[] UnlockedPets => _instance._unlockedPets.List.ToArray();
		public static bool HaveInstance => _instance != null;

		private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";
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
		public static Pet PetBySystemName(string name) => PetInfoBySystemName(name).Pet;
		public static PetSaveInfo PetInfoBySystemName(string name) 
		{
			Pet lookingPet = Resources.Load<Pet>(name);
			PetSaveInfo[] info = UnlockedPets.Where((pet) => pet.Pet == lookingPet).ToArray();
			if (info.Length == 0)
			{
				return null;
			}
			return info[0];
		}

		protected new void Awake()
		{
			base.Awake();
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
			_instance = this;

			LoadPlayerData();
			try
			{
				_secondsPassed = (int)(DateTime.UtcNow - DateTime.ParseExact(instance.PlayerData.LastLaunchTime, DATE_TIME_FORMAT, CultureInfo.InvariantCulture)).TotalSeconds;
			}
			catch
			{
				_secondsPassed = 0;
			}
			Pet.FallRatePetsByTime(UnlockedPets, _secondsPassed);
		}

		protected void Start()
		{
			_loaded.Invoke();
		}

		private void OnDisable()
		{
			if (HaveInstance == false)
			{
				return;
			}
			SavePlayerData();
		}

		public static int GemsCount
		{
			get
			{
				return instance._gemsCount.Value;
			}
			set
			{
				instance.PlayerData.GemsCount = value;
				instance._gemsCount.Value = value;
			}
		}
		public static int MoraCount
		{
			get
			{
				return instance._moraCount.Value;
			}
			set
			{
				instance._moraCount.Value = value;
				instance.PlayerData.MoraCount = value;
			}
		}
		public static int FateCount
		{
			get
			{
				return instance._fateCount.Value;
			}
			set
			{
				instance._fateCount.Value = value;
				instance.PlayerData.FateCount = value;
			}
		}
		public static int RollCount
		{
			get
			{
				return _instance._rollCount.Value;
			}
			set
			{
				_instance._rollCount.Value = value;
				_instance.PlayerData.RollCount = value;
			}
		}
		public static UnityEvent<int> GemsCountChanged => instance._gemsCount.Changed;
		public static UnityEvent<int> MoraCountChanged => instance._moraCount.Changed;
		public static UnityEvent<int> FadeCountChanged => instance._gemsCount.Changed;
		public static void AddPet(PetSaveInfo info)
		{
			PetSaveInfo[] petInfos = UnlockedPets.Where(haveInfo => info.Pet == haveInfo.Pet).ToArray();
			if (petInfos.Length > 0)
			{
				petInfos[0].DuplicatCount++;
				return;
			}
			_instance._unlockedPets.List.Add(info);
			instance._unlockedNewPet.Invoke(info);
		}
		public static int GetCookedFoodCount(Food food)
		{
			if (instance._foodCookCount.Keys.Contains(food.name) == false)
			{
				return 0;
			}
			return instance._foodCookCount[food.name];
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
			instance._foodCountChanged?.Invoke(food, GetFoodCount(food));
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
		}
		public static void RemoveIngridient(Ingredient ingridient, int count = 1)
		{
			instance._ingridiendsInStorage[ingridient.name] = (int)Mathf.Clamp(instance._ingridiendsInStorage[ingridient.name] - count, 0, Mathf.Infinity);
			instance._ingridiendCountChanged.Invoke(ingridient, instance._ingridiendsInStorage[ingridient.name]);
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
			instance._waterCountChanged.Invoke(water, GetWaterCount(water));
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
			_instance._furnitureById.Add(furniture.ID, furniture);
			RemoveFurnitureFromStorage(furniture.SystemName, 1);
		}
		public static void RemoveFurniture(Furniture furniture)
		{
			_instance._furnitureById.Remove(furniture.ID);
			_instance._furnitureInStorage[furniture.SystemName] += 1;
			FurnitureInfo info = Resources.Load<FurnitureInfo>(furniture.SystemName);
			instance._furnitureOnStorageCountChanged.Invoke(info, GetFurnitureCountOnStorage(furniture.SystemName));
		}
		public static void AddFurnitureInStorage(string systemName, int count)
		{
			if (_instance._furnitureInStorage.Keys.Contains(systemName) == false)
			{
				_instance._furnitureInStorage.Add(systemName, 0);
			}
			_instance._furnitureInStorage[systemName] += count;
			FurnitureInfo info = Resources.Load<FurnitureInfo>(systemName);
			_instance._furnitureOnStorageCountChanged.Invoke(info, GetFurnitureCountOnStorage(systemName));
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
			FurnitureInfo info = Resources.Load<FurnitureInfo>(systemName);
			_instance._furnitureOnStorageCountChanged.Invoke(info, GetFurnitureCountOnStorage(systemName));
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
		public static bool AddRoom(Vector2Int room)
		{
			return _instance._home.AddRoom(room);
		}
		public static Vector2Int[] GetRooms()
		{
			return _instance._home.Rooms.ToArray();
		}
		public static Vector2Int[] GetBuildPlaces()
		{
			return _instance._home.BuildPlaces().ToArray();
		}
		public static bool CanBuildHere(Vector2Int position) => _instance._home.CanBuildHere(position);
		public static int BuildCost => _instance._home.BuildHomeCost;
		public static void WriteRecord(int miniGame, float record)
		{
			if (instance._minigameRecords.Keys.Contains(miniGame) == false)
			{
				instance._minigameRecords.Add(miniGame, record);
				return;
			}
			instance._minigameRecords[miniGame] = record;
		}
		public static float GetRecord(int miniGame)
		{
			if (instance._minigameRecords.Keys.Contains(miniGame) == false)
			{
				return 0;
			}
			return instance._minigameRecords[miniGame];
		}

		public static void ClearPetList()
		{
			instance._unlockedPets = StartPetList;
		}
		public static void SavePlayerData()
		{
			if (instance._isLoaded == false)
			{
				return;
			}

			PlayerData data = instance.PlayerData;

			data.MoraCount = MoraCount;
			data.FateCount = FateCount;
			data.GemsCount = GemsCount;

			data.FoodInStorage = JsonUtility.ToJson(instance._foodInStorage);
			data.IngridientsInStorage = JsonUtility.ToJson(instance._ingridiendsInStorage);
			data.WaterInStorage = JsonUtility.ToJson(instance._waterInStorage);
			data.FoodCookCount = JsonUtility.ToJson(instance._foodCookCount);
			data.FurnitureList = JsonUtility.ToJson(instance._furnitureById);
			data.FurnitureInStorage = JsonUtility.ToJson(instance._furnitureInStorage);
			data.Home = JsonUtility.ToJson(instance._home);
			data.UnlockedPets = JsonUtility.ToJson(_instance._unlockedPets);
			data.MiniGamesRecords = JsonUtility.ToJson(_instance._minigameRecords);
			
			data.LastLaunchTime = DateTime.UtcNow.ToString(DATE_TIME_FORMAT);

			_instance.Save();
		}
		public static void LoadPlayerData()
		{
			_instance.Load();

			instance._foodInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance.PlayerData.FoodInStorage);
			instance._waterInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance.PlayerData.WaterInStorage);
			instance._ingridiendsInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance.PlayerData.IngridientsInStorage);
			instance._foodCookCount = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance.PlayerData.FoodCookCount);
			instance._furnitureById = JsonUtility.FromJson<UnityDictionarity<string, Furniture>>(instance.PlayerData.FurnitureList);
			instance._furnitureInStorage = JsonUtility.FromJson<UnityDictionarity<string, int>>(instance.PlayerData.FurnitureInStorage);
			instance._home = JsonUtility.FromJson<Home>(instance.PlayerData.Home);
			instance._unlockedPets = JsonUtility.FromJson<SerializedList<PetSaveInfo>>(instance.PlayerData.UnlockedPets);
			instance._minigameRecords = JsonUtility.FromJson<UnityDictionarity<int, float>>(_instance.PlayerData.MiniGamesRecords);

			instance._foodInStorage ??= new();
			instance._waterInStorage ??= new();
			instance._ingridiendsInStorage ??= new();
			instance._foodCookCount ??= new();
			instance._furnitureById ??= new();
			instance._furnitureInStorage ??= new();
			instance._home ??= new();
			instance._unlockedPets ??= StartPetList;
			instance._minigameRecords ??= new();

			instance._gemsCount.Value = instance.PlayerData.GemsCount;
			instance._moraCount.Value = instance.PlayerData.MoraCount;
			instance._fateCount.Value = instance.PlayerData.FateCount;
			instance._isLoaded = true;
		}

		private static SerializedList<PetSaveInfo> StartPetList
		{
			get
			{
				return new SerializedList<PetSaveInfo>()
				{
					List = new()
				};
			}
		}
	}
}