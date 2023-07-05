using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityExtentions;

namespace OrhidsAttack
{
	[AddComponentMenu("Mini-games/Orhids Attack/Shooter")]
	public class Shooter : MonoBehaviour
	{
		[SerializeField] private Bullet _bulletPrefab;
		[SerializeField] private Transform _bulletSpawnPosition;
		[SerializeField] private Range _rateOfFire = new Range(0.5f, 0.8f);

		public delegate void ShootHandler(Bullet bullet);
		private event ShootHandler _shot;

		public event ShootHandler Shot
		{
			add => _shot += value;
			remove => _shot -= value;
		}

		private float _timeBetweenShoots => 1f/_rateOfFire;
		private float _shootTimer = 0;
		private Coroutine _shootCoroutine;

		private bool CanShoot => _shootCoroutine == null && _shootTimer == 0;

		public void Shoot()
		{
			if (CanShoot == false)
			{
				return;
			}
			_shootTimer = _timeBetweenShoots;
			Bullet instance = SmartMonoBehaivor.Create(_bulletPrefab, _bulletSpawnPosition.position, _bulletSpawnPosition.rotation);
			_shootCoroutine = StartCoroutine(ShootTimerCoroutine());
			_shot?.Invoke(instance);
		}

		private IEnumerator ShootTimerCoroutine()
		{
			while (_shootTimer > 0)
			{
				_shootTimer = Mathf.Clamp(_shootTimer - Time.deltaTime, 0, Mathf.Infinity);
				yield return null;
			}

			_shootCoroutine = null;
		}

		private void OnDisable()
		{
			_shootCoroutine = null;
			_shootTimer = 0;
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(Shooter))]
		private class ShooterEditor : Editor
		{
			private new Shooter target => base.target as Shooter;
			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();
				if (Application.IsPlaying(target.gameObject) && GUILayout.Button("Shoot"))
				{
					target.Shoot();
				}
			}
		}
#endif
	}
}