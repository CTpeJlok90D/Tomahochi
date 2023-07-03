using UnityEngine;

public class DestroyRoomTrigger : MonoBehaviour
{
	[SerializeField] private GameObject _owner;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent(out Player player))
		{
			Destroy(_owner);
		}
	}
}
