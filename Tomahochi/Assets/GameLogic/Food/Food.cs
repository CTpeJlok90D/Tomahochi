using Pets;
using Saving;
using UnityEngine;
using UnityEngine.Events;

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

	public override void ApplyLoot()
	{
		AddOnStorage(3);
		GotLoot.Invoke();
	}

	public override UnityEvent<Storageble, int> OnStorageCountChanged => PlayerDataContainer.FoodCountChanged;
}
