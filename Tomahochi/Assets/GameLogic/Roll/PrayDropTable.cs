using Pets;
using Saving;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Praying/PrayLootTable")]
public class PrayDropTable : ScriptableObject
{
	[SerializeField] private List<Pet> _4StarPets = new();
	[SerializeField] private List<Storageble> _4StartShopItems = new();
	[SerializeField] private List<Pet> _5StarPets = new();
	[SerializeField] private List<Storageble> _foodAndWater = new();
	[SerializeField] private MoraPrayDropItem _mora;
	[Header("Chanses")]
	[SerializeField] private float _foodWhaterChance = 30;
	[SerializeField] private float _rareChance = 7;
	[SerializeField] private float _legendaryChance = 3;

	private int RollCountForRareGuarant = 10;
	private int RollCcountForLlegendaryGuarant = 70;

	public ILootDrop Pray()
	{
		ILootDrop result = null;
		PlayerDataContainer.RollCount++;
		if (PlayerDataContainer.RollCount % RollCcountForLlegendaryGuarant == 0)
		{
			return RandomLegendaryItem();
		}
		if (PlayerDataContainer.RollCount % RollCountForRareGuarant == 0)
		{
			return RandomRareItem();
		}
		if (result == null)
		{
			float value = Random.Range(0f,100f);
			if (value >= 100 - _legendaryChance)
			{
				return RandomLegendaryItem();
			}
			if (value >= 100 - _rareChance)
			{
				return RandomRareItem();
			}
			if (value >= 100 - _foodWhaterChance)
			{
				return RandomFoodOrWater();
			}
			return _mora;
		}
		return result;
	}

	private ILootDrop RandomRareItem()
	{
		float randomValue = Random.Range(0, 1);
		if (randomValue == 1)
		{
			return _4StarPets[Random.Range(0, _4StarPets.Count)];
		}
		return _4StartShopItems[Random.Range(0, _4StartShopItems.Count)];
	}

	private ILootDrop RandomLegendaryItem()
	{
		return _5StarPets[Random.Range(0, _5StarPets.Count)];
	}

	private ILootDrop RandomFoodOrWater()
	{
		return _foodAndWater[Random.Range(0, _foodAndWater.Count)];
	}
}
