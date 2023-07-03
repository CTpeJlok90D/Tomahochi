using UnityEngine;
using UnityExtentions;

public class MoveblePlatform : MonoBehaviour
{
	[SerializeField] private Range _moveDistanceXRange;
	[SerializeField] private Range _moveDistanceYRange;
	[SerializeField] private Range _speedRange;

	private Vector3 _startPosition;
	private int _moveDirection;
	private Vector2 _moveDistance;
	private float _speed;
	private float _process;

	private Vector3 _targetPoint => _startPosition + (Vector3)_moveDistance * _moveDirection;
	private Vector3 _startPoint => _startPosition + (Vector3)_moveDistance * -_moveDirection;


	private void OnEnable()
	{
		_moveDirection = 0;
		while (_moveDirection == 0)
		{
			_moveDirection = Random.Range(-1, 1);
		}
		_startPosition = transform.position;
		_moveDistance = new(_moveDistanceXRange, _moveDistanceYRange);
		_speed = _speedRange;
	}

	private void Update()
	{
		if (_process >= 1)
		{
			_moveDirection *= -1;
			_process = 0;
		}

		_process += Time.deltaTime * _speed;
		transform.position = Vector3.Lerp(_targetPoint, _startPoint, _process);
	}
}
