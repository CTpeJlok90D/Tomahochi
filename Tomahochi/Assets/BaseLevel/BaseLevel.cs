using UnityEngine;
using System;
using Memory;
using UnityEngine.Events;
using Saving;
using Pets;
using System.Linq;
using UnityEngine.Assertions.Must;

public class BaseLevel : MonoBehaviour
{
	private const string SAVE_KEY = "Base level";

	[SerializeField] private ReactiveVariable<int> _level = new(0);
	[SerializeField] private ReactiveVariable<float> _xp = new(0);
	[SerializeField] private UnityDictionarity<int, float> _xpCountByPetStars = new();
	[SerializeField] private UnityDictionarity<int, float> _xpCountPerPetLevelUpByPetStars = new();
	[SerializeField] private float _xpToLevelUp = 1000;
	[SerializeField] private float _xpPerFurnitureBy = 300;

	private FurnitureInfo[] _furniture;
	private SoldItem[] _soldFurniture;

	public UnityEvent<float> XPCountChanged => _xp.Changed;
	public UnityEvent<int> LevelChanged => _level.Changed;
	public int Level => _level.Value;
	public float XpToLevelUp => _xpToLevelUp;
	public float XP
	{
		get
		{
			return _xp.Value;
		}
		set
		{
			_xp.Value = value;
			while (_xp.Value > _xpToLevelUp)
			{
				_level.Value++;
				_xp.Value -= _xpToLevelUp;
			}
		}
	}
	private void Awake()
	{
		_furniture = Resources.LoadAll<FurnitureInfo>("");
		_soldFurniture = Resources.LoadAll<SoldItem>("Furniture").Where(item => item.Item is FurnitureInfo).ToArray();
		Load();
	}

	private void OnEnable()
	{
		PlayerDataContainer.UnlockedNewPet.AddListener(OnUnlockNewPet);
		PlayerDataContainer.Loaded.AddListener(AfterLoad);

		foreach (FurnitureInfo info in _furniture)
		{
			info.GotLoot.AddListener(OnGotFurniture);
		}

		foreach (SoldItem item in _soldFurniture)
		{
			item.ItemSolden.AddListener(OnGotFurniture);
		}
	}

	private void OnDisable()
	{
		foreach (FurnitureInfo info in _furniture)
		{
			info.GotLoot.RemoveListener(OnGotFurniture);
		}

		foreach (SoldItem item in _soldFurniture)
		{
			item.ItemSolden.RemoveListener(OnGotFurniture);
		}

		if (PlayerDataContainer.HaveInstance == false)
		{
			return;
		}
		PlayerDataContainer.UnlockedNewPet.RemoveListener(OnUnlockNewPet);
		PlayerDataContainer.Loaded.RemoveListener(AfterLoad);
		foreach (PetSaveInfo info in PlayerDataContainer.UnlockedPets)
		{
			info.LevelUppped.RemoveListener(OnPetLevelUp);
		}
	}
	private void AfterLoad()
	{
		foreach (PetSaveInfo info in PlayerDataContainer.UnlockedPets)
		{
			info.LevelUppped.AddListener(OnPetLevelUp);
		}
	}

	private void OnGotFurniture(Storageble storageble) => OnGotFurniture();
	private void OnGotFurniture()
	{
		XP += _xpPerFurnitureBy;
	}

	private void OnPetLevelUp(PetSaveInfo info)
	{
		XP += _xpCountPerPetLevelUpByPetStars[info.Pet.StarCount];
	}

	public void OnUnlockNewPet(PetSaveInfo info)
	{
		XP += _xpCountByPetStars[info.Pet.StarCount];
	}

	private void OnDestroy()
	{
		Save();
	}

	private void Load()
	{
		SaveData data = new Load<SaveData>(SAVE_KEY);
		_level.Value = data.Level;
		_xp.Value = data.XP;
	}

	private void Save()
	{
		SaveData data = new()
		{
			Level = _level.Value,
			XP = _xp.Value
		};
		new Save(SAVE_KEY, data);
	}


	[Serializable]
	private struct SaveData
	{
		public int Level;
		public float XP;
	}
}
