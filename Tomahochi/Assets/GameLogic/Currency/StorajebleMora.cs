using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Currency/Mora")]
public class StorajebleMora : Storageble
{
	public override UnityEvent<Storageble, int> OnStorageCountChanged => _countChanged;
	private UnityEvent<Storageble, int> _countChanged = new();

	private void OnEnable()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			PlayerDataContainer.MoraCountChanged.AddListener(OnMoraCountChanged);
		}
	}

	private void OnMoraCountChanged(int count)
	{
		_countChanged.Invoke(this, count);
	}

	public override void AddOnStorage(int count)
	{
		PlayerDataContainer.MoraCount += count;
	}

	public override void ApplyLoot()
	{
		PlayerDataContainer.MoraCount++;
	}

	public override int GetStorageCount()
	{
		return PlayerDataContainer.MoraCount;
	}

	public override void RemoveFromStorage(int count)
	{
		PlayerDataContainer.MoraCount -= count;
	}
}
