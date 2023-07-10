using Saving;
using System;
using UnityEngine;

namespace Pets
{
	[Serializable]
	public class PetSaveInfo : CurrencyDrive
	{
		[Header("Indicators")]
		public float Food = 50;
		public float Water = 50;
		public float Joy = 50;
		public float Energy = 50;
		public string SleepingBedID;
		[Header("Level")]
		public int ElevateCount = 0;
		public bool NeedEvelate = false;
		public float CurrentXP = 0;
		public int CurrentLevel = 1;
		[Header("Duplicats")]
		public int DuplicatCount = 0;
		public string SystemName;

		[SerializeField] private Pet _pet;
		public Pet Pet
		{
			get 
			{
				if (_pet == null)
					_pet = Resources.Load<Pet>(SystemName);

				return _pet;
			} 
		}

		public PetSaveInfo(Pet pet)
		{
			SystemName = pet.name;
			_pet = pet;
		}

		public bool IsSleeping() => string.IsNullOrEmpty(SleepingBedID) == false;
		public int ElevateCost()
		{
			return Pet.ElevateMoraBaseCost + Pet.ElevateCostRise * ElevateCount;
		}
		public void Elevate()
		{
			if (PlayerDataContainer.MoraCount < ElevateCost())
			{
				return;
			}
			PlayerDataContainer.MoraCount -= ElevateCost();
			ElevateCount++;
			NeedEvelate = false;
		}
		public override int MoraStorage => Pet.BaseMoraStorage + Pet.MoraStorageExpandCount * (CurrentLevel / Pet.LevelsToMoraStorageExpand) + Pet.MoraStoragePerDuplicatBonus * DuplicatCount;

		public override int GemsStorage => Pet.BaseGemsStorage + Pet.GemsStorageExpandCount * (CurrentLevel / Pet.LevelsToGemsStorageExpand);

		public override float MoraPerSecond => (Pet.MoraPerSecond + Pet.MoraPerSecondDuplicatBonus * DuplicatCount);

		public override float GemsPerSecond => (Pet.GemsPerSecond + Pet.GemsPerSecondDuplicatBonus * DuplicatCount);

		public bool CanFeed(Food food)
		{
			return Food < Pet.MIN_HUNGER_TO_FEED && PlayerDataContainer.GetFoodCount(food) > 0 && IsSleeping() == false;
		}

		public void Feed(Food food)
		{
			if (CanFeed(food) == false)
			{
				return;
			}
			PlayerDataContainer.RemoveFood(food, 1);
			Food = Mathf.Clamp(Food + food.Nutritional, 0, 100);
			GainXP(food.XpCount);
		}

		public bool CanDrink(Water water)
		{
			return Water < Pet.MIN_THIRST_TO_DRINK && PlayerDataContainer.GetWaterCount(water) > 0 && IsSleeping() == false;
		}

		public void Drink(Water water)
		{
			if (CanDrink(water) == false)
			{
				return;
			}
			PlayerDataContainer.RemoveWater(water, 1);
			Water = Mathf.Clamp(Water + water.Nutritional, 0, 100);
			GainXP(water.XpCount);
		}

		public bool CanPlay()
		{
			return Joy < Pet.MIN_JOY_TO_PLAY && IsSleeping() == false;
		}

		public void Stroke()
		{
			ClaimCurrency();

			if (CanPlay())
			{
				GainXP(_pet.XpPerStroke);
				Joy = Mathf.Clamp(Joy + Pet.JoyPerStroke, 0, 100);
			}
		}

		public void ClaimCurrency()
		{
			PlayerDataContainer.MoraCount += (int)MoraCount;
			MoraCount -= (int)MoraCount;

			PlayerDataContainer.GemsCount += (int)GemsCount;
			GemsCount -= (int)GemsCount;
		}

		public void GainXP(float count)
		{
			if (CurrentLevel == Pet.MaxLevel || NeedEvelate)
			{
				return;
			}

			CurrentXP += count;

			while (CurrentXP > Pet.XPToLevelUp)
			{
				LevelUp();
				CurrentXP -= Pet.XPToLevelUp;
			}
		}

		public void LevelUp()
		{
			if (CurrentLevel == Pet.MaxLevel || NeedEvelate)
			{
				return;
			}

			CurrentLevel++;
			GemsCount = Mathf.Clamp(GemsCount + Pet.GemsPerLevel, 0, GemsStorage);
			if (CurrentLevel % Pet.EvelateEveryLevel == 0)
			{
				NeedEvelate = true;
			}
		}

		public bool CanSleep()
		{
			return Energy < Pet.MIN_ENERGY_TO_SLEEP && IsSleeping() == false;
		}

		public void LaySleep(Furniture sleepSpot)
		{
			if (CanSleep() == false)
			{
				return;
			}
			SleepingBedID = sleepSpot.ID.ToString();
		}

		public override bool DriveCondition => Energy != 0 && Water != 0 && Joy != 0 && Food != 0;
		public override void EmitLive(float seconds)
		{
			base.EmitLive(seconds);

			if (CurrentLevel == _pet.MaxLevel)
			{
				Joy = 100;
				Food = 100;
				Water = 100;
				Energy = 100;
				return;
			}

			Joy = Mathf.Clamp(Joy - Pet.JoyFallRate * seconds, 0, 100);
			Food = Mathf.Clamp(Food - Pet.HungerFallRate * seconds, 0, 100);
			Water = Mathf.Clamp(Water - Pet.ThiestFallRate * seconds, 0, 100);
			if (IsSleeping() == false)
			{
				Energy = Mathf.Clamp(Energy - Pet.FatigueFallRate * seconds, 0, 100);
			}
			else
			{
				float oldFatigue = Energy;
				Energy = Mathf.Clamp(Energy + Pet.FatigueUpRate * seconds, 0, 100);
				float sleepTime = (Energy - oldFatigue) / Pet.FatigueUpRate;
				GainXP(sleepTime * Pet.XpWhileSleepRate);
				if (Energy == 100 || Joy == 0 || Water == 0 || Food == 0)
				{
					SleepingBedID = string.Empty;
				}
			}
		}
	}
}