using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Currency/Gems")]
public class StoragebleGems : Storageble
{
	public override UnityEvent<Storageble, int> OnStorageCountChanged => _countChanged;
	private UnityEvent<Storageble, int> _countChanged = new();

	private void OnEnable()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			PlayerDataContainer.MoraCountChanged.AddListener(OnGemsCountChanged);
		}
	}

	private void OnGemsCountChanged(int count)
	{
		_countChanged.Invoke(this, count);
	}

	public override void AddOnStorage(int count)
	{
		PlayerDataContainer.GemsCount += count;
	}

	public override void ApplyLoot()
	{
		PlayerDataContainer.GemsCount++;
	}

	public override int GetStorageCount()
	{
		return PlayerDataContainer.GemsCount;
	}

	public override void RemoveFromStorage(int count)
	{
		PlayerDataContainer.GemsCount -= count;
	}
}
