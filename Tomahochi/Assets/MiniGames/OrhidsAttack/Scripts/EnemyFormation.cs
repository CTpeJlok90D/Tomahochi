using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OrhidsAttack
{
	[Serializable]
	public class EnemyFormation
	{
		[SerializeField] private List<SmartEnemy[]> _formation = new();

		private int _lenght;

		public int RowCount => _formation.Count;
		public SmartEnemy this[int x, int y]
		{
			get
			{
				if (y < 0 || y >= RowCount || x < 0 || x >= _lenght)
				{
					return null;
				}
				return _formation[y][x];
			}
		}

		public EnemyFormation(int lenght)
		{
			_lenght = lenght;
		}

		public void Remove(Vector2Int position)
		{
			_formation[position.y][position.x] = null;
		}

		public void Remove(SmartEnemy enemy)
		{
			foreach (SmartEnemy[] row in _formation)
			{
				int enemyIndex = row.ToList().IndexOf(enemy);
				if (enemyIndex == -1)
				{
					continue;
				}
				row[enemyIndex] = null;
			}
		}

		public void AddEnemy(SmartEnemy enemy, Vector2Int position)
		{
			_formation[position.y][position.x] = enemy;
		}

		public void AddRow()
		{
			_formation.Add(new SmartEnemy[_lenght]);
		}
	}
}