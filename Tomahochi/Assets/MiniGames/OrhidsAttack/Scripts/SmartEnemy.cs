using UnityEngine;

namespace OrhidsAttack
{
	public class SmartEnemy : SmartMonoBehaivor
	{
		[SerializeField] private float _yMoveSpeed = 0.25f;
		[SerializeField] private Vector3 _moveDirection = new(0, -1, 0);

		[SerializeField] private Vector2Int _formationPosition;
		private EnemyFormation _formation;

		public EnemyFormation Formation => _formation;
		public Vector2Int FormationPosition => _formationPosition;

		public SmartEnemy Init(EnemyFormation formation, Vector2Int formationPosition)
		{
			_formation = formation;
			_formationPosition = formationPosition;

			_formation.AddEnemy(this, formationPosition);

			return this;
		}

		protected void Update()
		{
			transform.position += _moveDirection.normalized * _yMoveSpeed * Time.deltaTime;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_formation.Remove(_formationPosition);
		}
	}
}