using Saving;
using UnityEngine;

public class MoraItem : MonoBehaviour
{
	[SerializeField] private int _count;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IMiniGamePlayer player))
		{
			PlayerDataContainer.MoraCount += _count;
			Destroy(gameObject);
		}
	}
}
