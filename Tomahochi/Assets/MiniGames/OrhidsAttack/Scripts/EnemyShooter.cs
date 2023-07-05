using OrhidsAttack;
using UnityEngine;

namespace OrhidsAttack
{
	[RequireComponent(typeof(Shooter))]
	public class EnemyShooter : MonoBehaviour
	{
		[SerializeField] private Shooter _shooter;
		[SerializeField] private SmartEnemy _smartEnemy;

		private void Update()
		{
			TryShoot();
		}

		private void TryShoot()
		{
			Vector2Int position = _smartEnemy.FormationPosition;
			if (_smartEnemy.Formation[position.x, position.y - 1] != null)
			{
				return;
			}
			_shooter.Shoot();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_shooter ??= GetComponent<Shooter>();
			_smartEnemy ??= GetComponent<SmartEnemy>();
		}
#endif
	}
}