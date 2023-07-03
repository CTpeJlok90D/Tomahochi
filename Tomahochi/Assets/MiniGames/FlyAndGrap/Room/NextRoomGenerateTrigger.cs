using UnityEngine;

public class NextRoomGenerateTrigger : MonoBehaviour
{
	[SerializeField] private GameObject _nextRoomPrefab;
	[SerializeField] private Vector3 _nextRoomPositionOffcet = new(0,20);
	[SerializeField] private GameObject _owner;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out FlyAndGrapPlayer player))
		{
			GameObject instance = Instantiate(_nextRoomPrefab);
			instance.transform.position = _owner.transform.position + _nextRoomPositionOffcet;
			Destroy(this);
		}
	}
}
