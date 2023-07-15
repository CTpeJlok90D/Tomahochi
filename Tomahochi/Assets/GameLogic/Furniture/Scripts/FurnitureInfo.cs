using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Furniture/Furniture", fileName = "Furniture")]
public class FurnitureInfo : Storageble
{
	[SerializeField] private GameObject _viewPrefab;
	[SerializeField] private Vector2[] _buildCheckPoints;

	public GameObject ViewPrefab => _viewPrefab;
	public Vector2[] BuildCheckPoints => _buildCheckPoints;

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
		GotLoot.Invoke();
	}
}
