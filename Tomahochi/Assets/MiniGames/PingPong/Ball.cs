using UnityEngine;
using UnityExtentions;

namespace PingPong
{
	public class Ball : SmartMonoBehaivor
	{
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private Range _xMoveDirection;
		[SerializeField] private Range _yMoveDirection;
		[SerializeField] private float _moveSpeed = 400;
		[SerializeField] private bool _damagePlayer = true;

		public bool DamagePlayer => _damagePlayer;

		protected override void Start()
		{
			_rigidbody2D.gravityScale = 0;
			_rigidbody2D.AddForce(new Vector2(_xMoveDirection, _yMoveDirection).normalized * _moveSpeed);
		}


#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			_rigidbody2D ??= GetComponent<Rigidbody2D>();
		}
#endif
	}
}