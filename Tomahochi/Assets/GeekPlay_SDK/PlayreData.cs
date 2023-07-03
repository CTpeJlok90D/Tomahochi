using System;

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
		public string UnlockedPets;
		public string CurrencyDrivers;
		public string Home;
		public string MiniGamesRecords;
		public int GemsCount = 0;
		public int MoraCount = 0;
		public int FateCount = 0;
		public int RollCount = 0;

		public string LastLaunchTime;
	}
}