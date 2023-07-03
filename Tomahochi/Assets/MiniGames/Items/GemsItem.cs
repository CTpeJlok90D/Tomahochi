using Saving;
using UnityEngine;

public class GemsItem : MonoBehaviour
{
	[SerializeField] private int _count;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			PlayerDataContainer.GemsCount += _count;
			Destroy(gameObject);
		}
	}
}
