using Saving;
using System.Collections.Generic;
using UnityEngine;

public class StoragembleListView : MonoBehaviour
{
	[SerializeField] private Transform _content;
	[SerializeField] private StoragebleListElement _element;
	[SerializeField] private UnityDictionarity<Consumeble, StoragebleListElement> _consumebleCount;
	[SerializeField] private ElementShowType Type = ElementShowType.Food;

	private void Start()
	{
		LoadFoodList();
	}

	private void OnEnable()
	{
		if (Type == ElementShowType.Food || Type == ElementShowType.All)
		{
			PlayerDataContainer.FoodCountChanged.AddListener(UpdateConsumebleList);
		}
		if (Type == ElementShowType.Water || Type == ElementShowType.All)
		{
			PlayerDataContainer.WaterCountChanged.AddListener(UpdateConsumebleList);
		}
	}

	private void OnDisable()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			PlayerDataContainer.FoodCountChanged.RemoveListener(UpdateConsumebleList);
			PlayerDataContainer.WaterCountChanged.RemoveListener(UpdateConsumebleList);
		}
	}

	private void UpdateConsumebleList(Consumeble consumeble, int count)
	{
		if (count == 0)
		{
			Destroy(_consumebleCount[consumeble].gameObject);
			_consumebleCount.Remove(consumeble);
			return;
		}
		if (_consumebleCount.Keys.Contains(consumeble) == false)
		{
			_consumebleCount.Add(consumeble, Instantiate(_element, _content).Init(consumeble, count));
		}
		else
		{
			_consumebleCount[consumeble].Count = count;
		}
	}
	private void LoadFoodList()
	{
		ClearContent();
		Consumeble[] foods = GetStoragebleObject();
		foreach (Consumeble consumeble in foods)
		{
			int currentFoodCount = consumeble.GetStorageCount();
			if (currentFoodCount == 0)
			{
				continue;
			}
			if (_consumebleCount.Keys.Contains(consumeble) == false)
			{
				_consumebleCount.Add(consumeble, Instantiate(_element, _content).Init(consumeble, currentFoodCount));
			}
			else
			{
				_consumebleCount[consumeble].Count = currentFoodCount;
			}
		}
	}

	private void ClearContent()
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
		_consumebleCount.Clear();
	}

	private Consumeble[] GetStoragebleObject()
	{
		switch (Type)
		{
			case ElementShowType.Food: 
				return Resources.LoadAll<Food>("");
			case ElementShowType.Water: 
				return Resources.LoadAll<Water>("");
			case ElementShowType.All: 
				List<Consumeble> result = new();
				result.AddRange(Resources.LoadAll<Food>(""));
				result.AddRange(Resources.LoadAll<Water>(""));
				return result.ToArray();
		}
		return null;
	}

	private enum ElementShowType
	{
		Food,
		Water,
		All
	}
}
