using System.Collections;
using UnityEngine;
using UnityExtentions;

public class Gun : MonoBehaviour
{
	[SerializeField] private TriggerEvents2D _vision;
	[SerializeField] private Transform _bulletSpawnPosition;
	[SerializeField] private Transform _gun;
	[SerializeField] private float _reloadTime;
	[SerializeField] private SmartMonoBehaivor _bulletPrefab;

	private GameObject _target;
	private Coroutine _shootCoroutine;
	private float _reloadTimer;

	private void OnEnable()
	{
		_vision.TriggerEntered += OnVisisionEnter;
		_vision.TriggerExited += OnVisionExit;
	}

	private void OnDisable()
	{
		_vision.TriggerEntered -= OnVisisionEnter;
		_vision.TriggerExited -= OnVisionExit;
	}

	private void OnVisisionEnter(Collider2D collision)
	{
		if (collision.TryGetComponent(out Target target) && _shootCoroutine == null)
		{
			_target = target.gameObject;
			_shootCoroutine = StartCoroutine(ShootCoroutine());
		}
	}

	private void OnVisionExit(Collider2D collision)
	{
		if (_target != null && collision.gameObject == _target.gameObject)
		{ 
			_target = null;
			this.TryStopCoroutine(_shootCoroutine);
		}
	}

	private IEnumerator ShootCoroutine()
	{
		while (_target != null)
		{
			_gun.transform.up = (Vector2)_target.transform.position - (Vector2)_gun.transform.position;

			if (_reloadTimer <= 0)
			{
				SmartMonoBehaivor.Create(_bulletPrefab, _bulletSpawnPosition.position, _bulletSpawnPosition.rotation);
				_reloadTimer = _reloadTime;
			}
			_reloadTimer -= Time.deltaTime;
			yield return null;
		}
		yield return null;
		_shootCoroutine = null;
	}
}
