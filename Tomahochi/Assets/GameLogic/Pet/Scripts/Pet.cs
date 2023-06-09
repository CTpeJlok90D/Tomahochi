using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
	[CreateAssetMenu]
	public class Pet : ScriptableObject
	{
		[SerializeField] private string _viewName = "<view name>";
		[SerializeField] private string _viewDescription = "<view description>";
		[SerializeField] private Sprite _viewSprite;
		[SerializeField] private int _starCount = 3;
		[SerializeField] private int _maxElevateCount = 4;
		[SerializeField] private int _requestXPCountToLevelUp = 1000;
		[SerializeField] private float _hungerTime = 8;
		[SerializeField] private float _fatigueTime = 12;
		[SerializeField] private float _pollutuionTime = 12;
		[SerializeField] private float _playFallTime = 6;

		private const int SECONDS_IN_HOUR = 3600;

		public string ViewName => _viewName;
		public string ViewDescription => _viewDescription;
		public int StarCount => _starCount;
		public int ReqestXPCountToLevelUp => _requestXPCountToLevelUp;
		public int MaxElevateCount => _maxElevateCount;
		public float HungerFallTime => _hungerTime;
		public float FatigueFallTime => _fatigueTime;
		public float PollutuionFallTime => _pollutuionTime;
		public float PlayFallTime => _playFallTime;

		public float HungerFallRate => HungerFallTime / SECONDS_IN_HOUR;
		public float FatigueFallRate => FatigueFallTime / SECONDS_IN_HOUR;
		public float PollutuionFallRate => PollutuionFallTime / SECONDS_IN_HOUR;
		public float PlayFallRate => PlayFallTime / SECONDS_IN_HOUR;

		public static void FallRatePetsByTime(List<PetSaveInfo> petList, float seconds)
		{
			foreach (PetSaveInfo info in petList)
			{
				Pet currentPet = Resources.Load<Pet>(info.SystemName);

				info.Play = Mathf.Clamp(info.Play - currentPet.PlayFallRate * seconds, 0, 100);
				info.Hunger = Mathf.Clamp(info.Hunger - currentPet.HungerFallRate * seconds, 0, 100);
				info.Polution = Mathf.Clamp(info.Polution - currentPet.PollutuionFallRate * seconds, 0, 100);
				info.Fatigue = Mathf.Clamp(info.Fatigue - currentPet.FatigueFallRate * seconds, 0, 100);
			}
		}
	}

	[Serializable]
	public class PetSaveInfo
	{
		public string SystemName;
		public int ElevateCount;
		public int CurrentXP;
		public int CurrentLevel;
		public float Hunger;
		public float Fatigue;
		public float Polution;
		public float Play;
	}
}