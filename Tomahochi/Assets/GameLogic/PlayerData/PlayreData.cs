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
		public string WaterInStorage;
		public string FurnitureList;
		public string FurnitureInStorage;
		public string Home;
		public int GemsCount = 0;
		public int MoraCount = 0;
		public int FateCount = 0;
		public int RollCount = 0;
		public List<PetSaveInfo> UnlockedPets = new();

		public string LastLaunchTime;
	}
}