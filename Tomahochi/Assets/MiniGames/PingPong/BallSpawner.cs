using UnityEngine;
using UnityExtentions;

namespace PingPong 
{

	public class BallSpawner : MonoBehaviour
	{
		[SerializeField] private Random<SmartMonoBehaivor> _ballPrefab = new();
		[SerializeField] private Transform _spawnPosition;
		[SerializeField] private float _spawnTime = 3;

		private float _timer;

		private void Update()
		{
			if (_timer > 0)
			{
				_timer -= Time.deltaTime;
			}
			if (_timer <= 0)
			{
				SmartMonoBehaivor.Create(_ballPrefab, _spawnPosition.position, _spawnPosition.rotation);
				_timer = _spawnTime;
			}
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_ballPrefab.OnValidate();
		}
#endif
	}
}
