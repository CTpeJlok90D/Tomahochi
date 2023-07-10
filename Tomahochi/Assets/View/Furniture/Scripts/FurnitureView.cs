using Saving;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		instance._created?.Invoke();
		return instance;
	}

	private void OnEnable()
	{
		SceneManager.sceneUnloaded += OnSceneLoad;
	}

	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= OnSceneLoad;
	}

	public static void OnSceneLoad(Scene oldScene)
	{
		_byID.Clear();
	}

	[SerializeField] private FurnitureInfo _info;

	private Furniture _source;
	private event Action _created;

	public event Action Created
	{
		add => _created += value;
		remove => _created -= value;
	}
	public Furniture Source => _source;
	public FurnitureInfo Info => _info;

	public void MoveOnStorage()
	{
		PlayerDataContainer.RemoveFurniture(_source);
		_byID.Remove(_source.ID);
		_info.AddOnStorage(1);
		Destroy(gameObject);
	}
}
