using Saving;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureList : MonoBehaviour
{
	[SerializeField] private FurnitureListElement _elementPrefab;
	[SerializeField] private Transform _content;
	private void OnEnable()
	{
		UpdateFurnitureList();
	}

	public void UpdateFurnitureList()
	{
		Clear();
		string[] allFurniture = PlayerDataContainer.GetAllFurnitureFromStorage();
		List<FurnitureInfo> furnitureInfos = new List<FurnitureInfo>();
		foreach (string funitureName in allFurniture)
		{
			furnitureInfos.Add(Resources.Load<FurnitureInfo>(funitureName));
		}
		foreach (FurnitureInfo furniture in furnitureInfos)
		{
			FurnitureListElement furnitureListElement = Instantiate(_elementPrefab, _content).Init(furniture);
		}
	}

	private void Clear()
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
	}
}
