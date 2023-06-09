using System;
using System.Collections.Generic;
using Pets;

namespace Saving
{
	[Serializable]
	public class PlayerData
	{
		public List<PetSaveInfo> UnlockedPets = new();
		public int GemsCount = 0;
		public int MoraCount = 0;
		public int FateCount = 0;

		public string LastLaunchTime;
	}
}