using UnityEngine;

public class DownSpike : Spike
{
	[SerializeField] private Vector3 _moveDirection = new Vector3(0, 1f,0);
	[SerializeField] private Transform _player;
	[SerializeField] private AnimationCurve _speedPerPlayerDistance;
	private void Update()
	{
		Vector3 offcet = _moveDirection * _speedPerPlayerDistance.Evaluate(Vector2.Distance(_player.position, transform.position)) * Time.deltaTime;
		transform.position += offcet;
	}
}