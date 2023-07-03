using UnityEngine;

public class ZOffsetChanger : MonoBehaviour
{
	[SerializeField] private Transform _target;
	private const float Z_OFFCEST_COEFFICIENT = 0.1f;
	private void LateUpdate()
	{
		transform.position = new(transform.position.x, transform.position.y, _target.position.y * Z_OFFCEST_COEFFICIENT);
	}

	private void OnValidate()
	{
		_target ??= transform;
	}
}
