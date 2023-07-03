using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Furniture/Furniture", fileName = "Furniture")]
public class FurnitureInfo : Storageble
{
	[SerializeField] private GameObject _viewPrefab;
	[SerializeField] private Sprite _viewIcon;
	[SerializeField] private Vector2[] _buildCheckPoints;
	[Tooltip("Life time of this object in seconds. -1 - Infinity")]
	[SerializeField] private int _lifeTime = -1;
	[SerializeField] private List<Boost> _boosts = new();

	public GameObject ViewPrefab => _viewPrefab;
	public Vector2[] BuildCheckPoints => _buildCheckPoints;
	public int LifeTime => _lifeTime;

	public override void AddOnStorage(int count)
	{
		PlayerDataContainer.AddFurnitureInStorage(name, count);
	}

	public override int GetStorageCount()
	{
		return PlayerDataContainer.GetFurnitureCountOnStorage(name);
	}

	public override UnityEvent<Storageble, int> OnStorageCountChanged => PlayerDataContainer.FurnitureOnStarageCountChanged;

	public override void RemoveFromStorage(int count)
	{
		PlayerDataContainer.RemoveFurnitureFromStorage(name, count);
	}

	public override void ApplyLoot()
	{
		AddOnStorage(1);
	}
}
