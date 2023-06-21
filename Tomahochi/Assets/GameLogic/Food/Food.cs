using Pets;
using Saving;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Cooking/Food")]
public class Food : Consumeble
{
	public override void AddOnStorage(int count)
	{
		PlayerDataContainer.AddFood(this, count);
	}

	public override void RemoveFromStorage(int count)
	{
		PlayerDataContainer.RemoveFood(this, count);
	}
	public override int GetStorageCount()
	{
		return PlayerDataContainer.GetFoodCount(this);
	}

	public override void Consume(PetSaveInfo pet)
	{
		pet.Feed(this);
	}
}
