using System;
using System.Collections.Generic;
using Pets;

namespace Saving
{
	[Serializable]
	public class PlayerData
	{
		public string FoodInStorage;
		public string IngridientsInStorage;
		public string FoodCookCount;
		public string WaterStorage;
		public int GemsCount = 0;
		public int MoraCount = 0;
		public int FateCount = 0;
		public int RareRollCount = 0;
		public int LegendaryRollCount = 0;
		public List<PetSaveInfo> UnlockedPets = new();

		public string LastLaunchTime;
	}
}