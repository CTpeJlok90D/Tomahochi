using Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureView : MonoBehaviour
{
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
		PlayerDataContainer.PlaceFurniture(furniture);
		instance._created?.Invoke();
		return instance;
	}


	[SerializeField] private FurnitureInfo _info;

	private Furniture _source;
	private event Action _created;
	private event Action _movedOnStorage;

	public event Action Created
	{
		add => _created += value;
		remove => _created -= value;
	}

	public event Action MovedOnStorage
	{
		add => _movedOnStorage += value;
		remove => _movedOnStorage -= value;
	}

	public Furniture Source => _source;
	public FurnitureInfo Info => _info;

	public void MoveOnStorage()
	{
		PlayerDataContainer.RemoveFurniture(_source);
		_byID.Remove(_source.ID);
		_movedOnStorage?.Invoke();
		Destroy(gameObject);
	}
}
