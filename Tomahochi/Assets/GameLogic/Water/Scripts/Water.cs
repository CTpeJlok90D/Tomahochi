using Pets;
using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Water", menuName = "Cooking/Water")]
public class Water : Consumeble
{
	public override void AddOnStorage(int count)
	{
		PlayerDataContainer.AddWater(this, count);
	}

	public override void RemoveFromStorage(int count)
	{
		PlayerDataContainer.RemoveWater(this, count);
	}

	public override void Consume(PetSaveInfo pet)
	{
		pet.Drink(this);
	}

	public override int GetStorageCount()
	{
		return PlayerDataContainer.GetWaterCount(this);
	}

	public override void ApplyLoot()
	{
		AddOnStorage(3);
		GotLoot.Invoke();
	}

	public override UnityEvent<Storageble, int> OnStorageCountChanged => PlayerDataContainer.WaterCountChanged;
}
