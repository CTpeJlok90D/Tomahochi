using Saving;
using System;
using UnityEngine;

namespace Pets
{
	[Serializable]
	public class PetSaveInfo
	{
		[Header("Basics")]
		public string SystemName = "<system name>";
		[Header("Indicators")]
		public float Food = 50;
		public float Water = 50;
		public float Joy = 50;
		public float Sleep = 50;
		public bool IsSleeping = false;
		[Header("Level")]
		public int ElevateCount = 0;
		public bool NeedEvelate = false;
		public float CurrentXP = 0;
		public int CurrentLevel = 1;
		[Header("Currency")]
		public float MoraCount = 0;
		public float GemsCount = 0;
		[Header("Duplicats")]
		public int DuplicatCount = 0;
	}

	public static class PetLogic
	{
		public static Pet Pet(this PetSaveInfo info) => Resources.Load<Pet>(info.SystemName);
		public static int MoraStorage(this PetSaveInfo info)
		{
			Pet pet = info.Pet();
			return pet.BaseMoraStorage + pet.MoraStorageExpandCount * (info.CurrentLevel / pet.LevelsToMoraStorageExpand) + pet.MoraStoragePerDuplicatBonus * info.DuplicatCount;
		}

		public static int GemsStorage(this PetSaveInfo info)
		{
			Pet pet = info.Pet();
			return pet.BaseGemsStorage + pet.GemsStorageExpandCount * (info.CurrentLevel / pet.LevelsToGemsStorageExpand);
		}

		public static float MoraPerSecond(this PetSaveInfo info)
		{
			Pet pet = info.Pet();
			return (pet.MoraPerSecond + pet.MoraPerSecondDuplicatBonus * info.DuplicatCount);
		}

		public static float GemsPerSecond(this PetSaveInfo info)
		{
			Pet pet = info.Pet();
			return (pet.GemsPerSecond + pet.GemsPerSecondDuplicatBonus * info.DuplicatCount);
		}

		public static bool CanFeed(this PetSaveInfo info, Food food) 
		{
			return info.Food < Pets.Pet.MIN_HUNGER_TO_FEED && PlayerDataContainer.FoodInStorage[food.name] > 0;
		}

		public static void Feed(this PetSaveInfo info, Food food)
		{
			if (info.CanFeed(food) == false)
			{
				return;
			}
			PlayerDataContainer.RemoveFood(food, 1);
			info.Food = Mathf.Clamp(info.Food + food.Nutritional, 0, 100);
			info.GainXP(food.XpCount);
		}

		public static bool CanDrink(this PetSaveInfo info, Water water)
		{
			return info.Water < Pets.Pet.MIN_THIRST_TO_DRINK && PlayerDataContainer.WaterInStorage[water.name] > 0;
		}

		public static void Drink(this PetSaveInfo info, Water water)
		{
			if (info.CanDrink(water) == false)
			{
				return;
			}
			PlayerDataContainer.RemoveWater(water, 1);
			info.Water = Mathf.Clamp(info.Water + water.Nutritional, 0, 100);
			info.GainXP(water.XpCount);
		}

		public static bool CanPlay(this PetSaveInfo info)
		{
			return info.Joy < Pets.Pet.MIN_JOY_TO_PLAY;
		}

		public static void Stroke(this PetSaveInfo info)
		{
			Pet pet = info.Pet();

			info.ClaimCurrency();

			if (info.CanPlay())
			{
				info.GainXP(pet.XpPerStroke);
				info.Joy = Mathf.Clamp(info.Joy + pet.JoyPerStroke, 0, 100);
			}
		}

		public static void ClaimCurrency(this PetSaveInfo info)
		{
			PlayerDataContainer.MoraCount += (int)info.MoraCount;
			info.MoraCount -= (int)info.MoraCount;

			PlayerDataContainer.GemsCount += (int)info.GemsCount;
			info.GemsCount -= (int)info.GemsCount;
		}

		public static void GainXP(this PetSaveInfo info, float count)
		{
			Pet pet = info.Pet();
			if (info.CurrentLevel == pet.MaxLevel || info.NeedEvelate)
			{
				return;
			}

			info.CurrentXP += count;

			while (info.CurrentXP > pet.XPToLevelUp)
			{
				info.LevelUp();
				info.CurrentXP -= pet.XPToLevelUp;
			}
		}

		public static void LevelUp(this PetSaveInfo info)
		{
			Pet pet = info.Pet();
			if (info.CurrentLevel == pet.MaxLevel || info.NeedEvelate)
			{
				return;
			}

			info.CurrentLevel++;
			info.GemsCount = Mathf.Clamp(info.GemsCount + pet.GemsPerLevel,0, info.GemsStorage());
			if (info.CurrentLevel % pet.EvelateEveryLevel == 0)
			{
				info.NeedEvelate = true;
			}
		}

		public static void EmitLive(this PetSaveInfo info, float seconds)
		{
			Pet pet = info.Pet();

			if (info.Sleep != 0 && info.Water != 0 && info.Joy != 0 && info.Food != 0)
			{
				info.MoraCount = Mathf.Clamp(info.MoraCount + info.MoraPerSecond() * seconds, 0, info.MoraStorage());
				info.GemsCount = Mathf.Clamp(info.GemsCount + info.GemsPerSecond() * seconds, 0, info.GemsStorage());
			}

			if (info.CurrentLevel == info.Pet().MaxLevel)
			{
				return;
			}

			info.Joy = Mathf.Clamp(info.Joy - pet.JoyFallRate*seconds, 0, 100);
			info.Food = Mathf.Clamp(info.Food - pet.HungerFallRate*seconds, 0, 100);
			info.Water = Mathf.Clamp(info.Water - pet.ThiestFallRate*seconds, 0, 100);
			if (info.IsSleeping == false)
			{
				info.Sleep = Mathf.Clamp(info.Sleep - pet.FatigueFallRate*seconds, 0, 100);
			}
			else
			{
				float oldFatigue = info.Sleep;
				info.Sleep = Mathf.Clamp(info.Sleep + pet.FatigueUpRate*seconds, 0, 100);
				float sleepTime = (info.Sleep - oldFatigue) / pet.FatigueUpRate;
				info.GainXP(sleepTime * pet.XpWhileSleepRate);
			}
		}
	}
}