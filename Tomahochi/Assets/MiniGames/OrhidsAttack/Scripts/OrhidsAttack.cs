using UnityEngine;
using UnityExtentions;

namespace OrhidsAttack
{
	[AddComponentMenu("Mini-games/Orhids Attack/OrhidsAttackComponent")]
	public class OrhidsAttack : MiniGame
	{
		[SerializeField] private TriggerEvents2D _deadZone;
		[SerializeField] private Shooter _player;

		private void OnEnable()
		{
			_deadZone.TriggerEntered += OnTriggerEnter2D;
			_player.Shot += OnShot;
		}

		private void OnShot(Bullet bullet)
		{
			bullet.Damaged += OnDamaged;
		}

		private void OnDamaged(Health health)
		{
			Score++;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent(out SmartEnemy player) == false)
			{
				return;
			}

			EndGame();
		}
	}
}
