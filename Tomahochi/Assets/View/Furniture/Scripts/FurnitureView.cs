using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FurnitureView : MonoBehaviour
{
	private Furniture _source;
	public Furniture Source => _source;

	private static Dictionary<string, FurnitureView> _byID = new();

	public static Dictionary<string, FurnitureView> ByID => new(_byID);

	public static FurnitureView CreatyByFurniture(Furniture furniture)
	{
		FurnitureInfo instanceInfo = Resources.Load<FurnitureInfo>(furniture.SystemName);
		FurnitureView instance = Instantiate(instanceInfo.ViewPrefab).GetComponent<FurnitureView>();
		instance._source = furniture;
		instance.transform.position = furniture.Position;

		_byID.Add(furniture.ID, instance);
		return instance;
	}
}
