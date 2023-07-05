using UnityEditor;
using UnityEngine;
using UnityExtentions;

namespace OrhidsAttack
{

	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField] private Grid _grid;
		[SerializeField] private Random<SmartEnemy> _enemyPrefab = new();
		[SerializeField] private Range _xSpawnPosition = new(-3, 3);
		[SerializeField] private int _ySpawnPosition;
		[SerializeField] private float _timeBetweenSpawn = 10;

		private float _spawnTimer;
		[SerializeField] private EnemyFormation _formation;


		private void Awake()
		{
			int enemyFormationLenght = (int)(Mathf.Abs(_xSpawnPosition.Min) + Mathf.Abs(_xSpawnPosition.Max));
			_formation = new EnemyFormation(enemyFormationLenght);
			_spawnTimer = _timeBetweenSpawn;
		}

		private void Update()
		{
			if (_spawnTimer > _timeBetweenSpawn)
			{
				_spawnTimer = 0;
				SpawnEnemyRow();
			}
			_spawnTimer += Time.deltaTime;
		}

		private void SpawnEnemyRow()
		{
			_formation.AddRow();
			for (int i = (int)_xSpawnPosition.Min, x = 0; i < (int)_xSpawnPosition.Max; i++, x++)
			{
				Vector3 spawnPosition = _grid.CellToWorld(new Vector3Int(i, _ySpawnPosition));

				SmartEnemy instance = SmartMonoBehaivor.Create(_enemyPrefab, spawnPosition, Quaternion.identity) as SmartEnemy;
				instance.Init(_formation, new(x, _formation.RowCount - 1));
			}
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_enemyPrefab.OnValidate();
		}

		[CustomEditor(typeof(EnemySpawner))]
		public class EnemySpawnerEditor : Editor
		{
			private new EnemySpawner target => base.target as EnemySpawner;
			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();
				GUILayout.Space(3);
				GUILayout.Label($"Debug", EditorStyles.boldLabel);
				EditorGUILayout.LabelField($"Spawn time {target._spawnTimer}");
			}
		}
#endif
	}
}