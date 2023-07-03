using System.Collections;
using UnityEngine;

public class StoragebleItem : MonoBehaviour
{
	[SerializeField] private Storageble _storageble;
	[SerializeField] private int _count;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			_storageble.AddOnStorage(_count);
			Destroy(gameObject);
		}
	}
}
