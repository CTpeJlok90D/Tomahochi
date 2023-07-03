using Pets;
using Saving;
using UnityEngine;

public class PetLoader : MonoBehaviour
{
	private void Start()
	{
		foreach (PetSaveInfo petInfo in PlayerDataContainer.UnlockedPets)
		{
			Instantiate(petInfo.Pet.ViewPrefab, transform);
		}
	}

	private void OnEnable()
	{
		PlayerDataContainer.UnlockedNewPet.AddListener(OnPetAdd);
	}

	private void OnDisable()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			PlayerDataContainer.UnlockedNewPet.RemoveListener(OnPetAdd);
		}
	}

	private void OnPetAdd(PetSaveInfo info)
	{
		Instantiate(info.Pet.ViewPrefab, transform);
	}
}
