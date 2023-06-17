using AICore;
using UnityEngine;
using UnityExtentions;

public class PetRandomWalk : Task
{
	[SerializeField] private Transform _target;
	[SerializeField] private Range _moveTime = new(1,3);
	[SerializeField] private Range _iDLETime = new(0.5f, 2.5f);
	[SerializeField] private Vector2 _moveSpeed;

	private float _timer;
	private bool _isMoving;

	private void Awake()
	{
		_target.SetParent(Agent.transform);
	}

	public override void OnUpdate()
	{
		_timer = Mathf.Clamp(_timer - Time.deltaTime, 0, Mathf.Infinity);
		if (_timer == 0)
		{
			_isMoving = _isMoving == false;
			if (_isMoving)
			{
				_timer = _moveTime;
				_target.localPosition = new Vector2(new Range(-1,1) * _moveSpeed.x, new Range(-1,1) * _moveSpeed.y);
			}
			else
			{
				_timer = _iDLETime;
				_target.localPosition = Vector3.zero;
			}
		}
		Agent.destination = _target.position;
	}
}
