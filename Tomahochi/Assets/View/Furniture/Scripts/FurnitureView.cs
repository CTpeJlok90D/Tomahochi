using Saving;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FurnitureView : MonoBehaviour
{
	[SerializeField] private FurnitureInfo _info;
	private Furniture _source;
	public Furniture Source => _source;
	public FurnitureInfo Info => _info;
	private static Dictionary<string, FurnitureView> _byID = new();

	public static Dictionary<string, FurnitureView> ByID => new(_byID);

	public static FurnitureView CreatyByFurniture(Furniture furniture, Transform parent = null)
	{
		FurnitureInfo instanceInfo = Resources.Load<FurnitureInfo>(furniture.SystemName);
		FurnitureView instance = Instantiate(instanceInfo.ViewPrefab).GetComponent<FurnitureView>();
		instance._info = instanceInfo;
		instance._source = furniture;
		instance.transform.SetParent(parent);
		instance.transform.localPosition = furniture.Position;

		_byID.Add(furniture.ID, instance);
		return instance;
	}

	public void MoveOnStorage()
	{
		PlayerDataContainer.RemoveFurniture(_source);
		_byID.Remove(_source.ID);
		_info.AddOnStorage(1);
		Destroy(gameObject);
	}
}
