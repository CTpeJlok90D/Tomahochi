using UnityEngine;

public class StoragebleContainer : MonoBehaviour
{
	[SerializeField] private Storageble _storageble;
	[SerializeField] private int _count;
	public Storageble Storageble => _storageble;

	public void AddOnStorage()
	{
		_storageble.AddOnStorage(_count);
	}
}
