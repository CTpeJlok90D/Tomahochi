using Pets;
using Saving;
using UnityEngine;

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

	public override int GetStorageCount()
	{
		return PlayerDataContainer.GetWaterCount(this);
	}

	public override void Consume(PetSaveInfo pet)
	{
		pet.Drink(this);
	}
}
