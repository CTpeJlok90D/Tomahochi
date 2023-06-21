using UnityEngine;

public class ZOffsetChanger : MonoBehaviour
{
	[SerializeField] private Transform _target;
	private void LateUpdate()
	{
		transform.position = new(transform.position.x, transform.position.y, _target.position.y);
	}

	private void OnValidate()
	{
		_target ??= transform;
	}
}
