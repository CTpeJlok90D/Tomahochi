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
				_pet ??= Resources.Load<Pet>(SystemName);
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
			return _pet.ElevateMoraBaseCost + _pet.ElevateCostRise * ElevateCount;
		}
		public void Elevate()
		{
			if (PlayerDataContainer.MoraCount < ElevateCost())
			{
				return;
			}
			PlayerDataContainer.MoraCount -= ElevateCost();
			NeedEvelate = false;
		}
		public override int MoraStorage => _pet.BaseMoraStorage + _pet.MoraStorageExpandCount * (CurrentLevel / _pet.LevelsToMoraStorageExpand) + _pet.MoraStoragePerDuplicatBonus * DuplicatCount;

		public override int GemsStorage => _pet.BaseGemsStorage + _pet.GemsStorageExpandCount * (CurrentLevel / _pet.LevelsToGemsStorageExpand);

		public override float MoraPerSecond => (_pet.MoraPerSecond + _pet.MoraPerSecondDuplicatBonus * DuplicatCount);

		public override float GemsPerSecond => (_pet.GemsPerSecond + _pet.GemsPerSecondDuplicatBonus * DuplicatCount);

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
				Joy = Mathf.Clamp(Joy + _pet.JoyPerStroke, 0, 100);
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
			if (CurrentLevel == _pet.MaxLevel || NeedEvelate)
			{
				return;
			}

			CurrentXP += count;

			while (CurrentXP > _pet.XPToLevelUp)
			{
				LevelUp();
				CurrentXP -= _pet.XPToLevelUp;
			}
		}

		public void LevelUp()
		{
			if (CurrentLevel == _pet.MaxLevel || NeedEvelate)
			{
				return;
			}

			CurrentLevel++;
			GemsCount = Mathf.Clamp(GemsCount + _pet.GemsPerLevel, 0, GemsStorage);
			if (CurrentLevel % _pet.EvelateEveryLevel == 0)
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

			Joy = Mathf.Clamp(Joy - _pet.JoyFallRate * seconds, 0, 100);
			Food = Mathf.Clamp(Food - _pet.HungerFallRate * seconds, 0, 100);
			Water = Mathf.Clamp(Water - _pet.ThiestFallRate * seconds, 0, 100);
			if (IsSleeping() == false)
			{
				Energy = Mathf.Clamp(Energy - _pet.FatigueFallRate * seconds, 0, 100);
			}
			else
			{
				float oldFatigue = Energy;
				Energy = Mathf.Clamp(Energy + _pet.FatigueUpRate * seconds, 0, 100);
				float sleepTime = (Energy - oldFatigue) / _pet.FatigueUpRate;
				GainXP(sleepTime * _pet.XpWhileSleepRate);
				if (Energy == 100 || Joy == 0 || Water == 0 || Food == 0)
				{
					SleepingBedID = string.Empty;
				}
			}
		}
	}
}