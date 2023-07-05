using UnityEngine;

namespace OrhidsAttack
{
	[AddComponentMenu("Mini-games/Orhids Attack/Bullet")]
	[RequireComponent(typeof(Rigidbody2D))]
	public class Bullet : SmartMonoBehaivor
	{
		[SerializeField] private float _moveSpeed = 1;
		[SerializeField] private Vector3 _moveDirection = new(0,1);
		[SerializeField] private int _damage = 1;

		public delegate void DamagedHandler(Health health);
		private event DamagedHandler _damaged;

		public event DamagedHandler Damaged
		{
			add => _damaged += value;
			remove => _damaged -= value;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.TryGetComponent(out Health health))
			{
				health.Value -= _damage;

				_damaged?.Invoke(health);
			}
			Remove(this);
		}

		private void Update()
		{
			transform.position += transform.TransformDirection(_moveDirection) * _moveSpeed * Time.deltaTime;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_damaged = null;
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
			rigidbody.gravityScale = 0;
		}
#endif
	}
}