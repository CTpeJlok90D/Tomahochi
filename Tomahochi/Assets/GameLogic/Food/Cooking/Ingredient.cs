using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Ingridient", menuName = "Cooking/Ingridient")]
public class Ingredient : Storageble
{
	public override void AddOnStorage(int count)
	{
		PlayerDataContainer.AddIngridient(this, count);
	}

	public override int GetStorageCount()
	{
		return PlayerDataContainer.GetIngridientCount(this);
	}

	public override UnityEvent<Storageble, int> OnStorageCountChanged => PlayerDataContainer.IngridiendCountChanged;

	public override void RemoveFromStorage(int count)
	{
		PlayerDataContainer.RemoveIngridient(this, count);
	}

	public override void ApplyLoot()
	{
		AddOnStorage(10);
		GotLoot.Invoke();
	}
}
