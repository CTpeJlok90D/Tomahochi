using Pets;
using Saving;
using UnityEngine;

public class PetLoader : MonoBehaviour
{
	private void Start()
	{
		foreach (PetSaveInfo petInfo in PlayerDataContainer.UnlockedPets)
		{
			Instantiate(petInfo.Pet().ViewPrefab, transform);
		}
	}
}
