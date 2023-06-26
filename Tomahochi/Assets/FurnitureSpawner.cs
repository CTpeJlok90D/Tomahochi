using Saving;
using UnityEngine;

public class FurnitureSpawner : MonoBehaviour
{
	private void Awake()
	{
		PlaceAllFurnitureFromPlayerData();
	}
	[ContextMenu("Place furniture")]
	public void PlaceAllFurnitureFromPlayerData()
	{
		foreach (string guid in PlayerDataContainer.GetAllFurnitureId())
		{
			FurnitureView instance = FurnitureView.CreatyByFurniture(PlayerDataContainer.GetFurnitureByID(guid), transform);
		}
	}
}
