using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pets
{
	[CreateAssetMenu]
	public class Pet : CurrencyDriveInfo, ILootDrop
	{
		[Header("Basics")]
		[SerializeField] string _viewName = "<view name>";
		[SerializeField] Sprite _viewSprite;
		[SerializeField] int _starCount = 5;
		[SerializeField] private GameObject _viewPrefab;
		private UnityEvent _gotLoot = new();
		public int StarCount => _starCount;
		public string ViewName => _viewName;
		public Sprite ViewSprite => _viewSprite;
		public GameObject ViewPrefab => _viewPrefab;


		[Header("Level")][SerializeField] int _maxElevateCount = 5;
		[SerializeField] int _elevateEveryLevel = 10;
		[SerializeField] int _elevateMoraCost = 200;
		[SerializeField] int _elevateCostRise = 100;
		[SerializeField] float _xPToLevelUp = 1000;
		[SerializeField] float _xpPerStroke = 10;
		public int MaxElevateCount => _maxElevateCount;
		public float XpPerStroke => _xpPerStroke;
		public float XPToLevelUp => _xPToLevelUp;
		public int EvelateEveryLevel => _elevateEveryLevel;
		public int ElevateMoraBaseCost => _elevateMoraCost;
		public int ElevateCostRise => _elevateCostRise;


		[Header("Joy")][SerializeField] float _joyPerStroke = 10;
		[SerializeField] float _joyTime = 6;
		public float JoyPerStroke => _joyPerStroke;
		public float JoyTime => _joyTime;
		public float JoyFallRate => 100 / JoyTime / SECONDS_IN_HOUR;


		[Header("Food and water")]
		[SerializeField] float _hungerTime = 8;
		[SerializeField] float _thirstTime = 6; 
		public float HungerTime => _hungerTime;
		public float ThirstTime => _thirstTime;
		public float HungerFallRate => 100 / (HungerTime * SECONDS_IN_HOUR);
		public float ThiestFallRate => 100 / (ThirstTime * SECONDS_IN_HOUR);

		[Header("Sleep")]
		[SerializeField] float _fatigueTime = 12;
		[SerializeField] float _sleepTime = 8;
		[SerializeField] float _xpWhileSleepPerHous = 12.5f;
		public float FatigueTime => _fatigueTime;
		public float SleepTime => _sleepTime;
		public float XpWhileSleepPerHous => _xpWhileSleepPerHous;
		public float FatigueFallRate => 100 / FatigueTime / SECONDS_IN_HOUR;
		public float FatigueUpRate => 100 / _sleepTime / SECONDS_IN_HOUR;
		public float XpWhileSleepRate => _xpWhileSleepPerHous / SECONDS_IN_HOUR;

		public const float MIN_HUNGER_TO_FEED = 98;
		public const float MIN_THIRST_TO_DRINK = 98;
		public const float MIN_JOY_TO_PLAY = 98;
		public const float MIN_ENERGY_TO_SLEEP = 98;

		[Header("Duplicats")]
		[SerializeField] private int _maxDuplicatCount = 6;
		[SerializeField] private int _moraPerDuplicatBonus = 1200;
		[SerializeField] private int _moraStoragePerDuplicantBonus = 600;
		[SerializeField] private int _gemsPerHourDuplicatBonus = 300;
		[SerializeField] private int _gemsStoragePerDuplicantBonus = 500;

		public int MaxDuplicatCount => _maxDuplicatCount;
		public int MoraPerHourDuplicatBonus => _moraPerDuplicatBonus;
		public int MoraStoragePerDuplicatBonus => _moraStoragePerDuplicantBonus;
		public int GemsPerHourDuplicatBonus => _gemsPerHourDuplicatBonus;
		public int GemsStoragePerDuplicatBonus => _gemsStoragePerDuplicantBonus;
		public float MoraPerSecondDuplicatBonus => (float)MoraPerHourDuplicatBonus / SECONDS_IN_HOUR;
		public float GemsPerSecondDuplicatBonus => (float)_gemsPerHourDuplicatBonus / SECONDS_IN_HOUR;

		public int MaxLevel => _maxElevateCount * _elevateEveryLevel;

		public string ViewCation => ViewName;

		public UnityEvent GotLoot => _gotLoot;

		public static void FallRatePetsByTime(IEnumerable<PetSaveInfo> petList, float seconds)
		{
			foreach (PetSaveInfo info in petList)
			{
				info.EmitLive(seconds);
			}
		}

		public void ApplyLoot()
		{
			PlayerDataContainer.AddPet(new PetSaveInfo(this));
			_gotLoot.Invoke();
		}
	}
}