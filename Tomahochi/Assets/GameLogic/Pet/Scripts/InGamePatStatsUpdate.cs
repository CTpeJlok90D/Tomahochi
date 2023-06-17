using UnityEditor;
using UnityEngine;
using Saving;

namespace Pets
{
	public class InGamePatStatsUpdate : MonoBehaviour
	{
		private void Update()
		{
			Pet.FallRatePetsByTime(PlayerDataContainer.UnlockedPets, Time.deltaTime);
		}
	}
}