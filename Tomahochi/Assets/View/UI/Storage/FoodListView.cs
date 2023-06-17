using Saving;
using UnityEngine;

public class FoodListView : MonoBehaviour
{
	[SerializeField] private Transform _content;
	[SerializeField] private FoodListElement _element;
	[SerializeField] private UnityDictionarity<Food, FoodListElement> _foodCount;

	private void Start()
	{
		LoadFoodList();
	}

	private void OnEnable()
	{
		PlayerDataContainer.FoodCountChanged.AddListener(UpdateFoodList);
	}

	private void OnDisable()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			PlayerDataContainer.FoodCountChanged.RemoveListener(UpdateFoodList);
		}
	}

	private void UpdateFoodList(Food food, int count)
	{
		if (count == 0)
		{
			Destroy(_foodCount[food].gameObject);
			_foodCount.Remove(food);
			return;
		}
		if (_foodCount.Keys.Contains(food) == false)
		{
			_foodCount.Add(food, Instantiate(_element, _content).Init(food, count));
		}
		else
		{
			_foodCount[food].Count = count;
		}
	}
	private void LoadFoodList()
	{
		ClearContent();
		Food[] foods = Resources.LoadAll<Food>("");
		foreach (Food food in foods)
		{
			if (PlayerDataContainer.FoodInStorage.Keys.Contains(food.name) == false)
			{
				return;
			}
			int currentFoodCount = PlayerDataContainer.FoodInStorage[food.name];
			if (currentFoodCount == 0)
			{
				continue;
			}
			if (_foodCount.Keys.Contains(food) == false)
			{
				_foodCount.Add(food, Instantiate(_element, _content).Init(food, currentFoodCount));
			}
			else
			{
				_foodCount[food].Count = currentFoodCount;
			}
		}
	}

	private void ClearContent()
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
		_foodCount.Clear();
	}
}
