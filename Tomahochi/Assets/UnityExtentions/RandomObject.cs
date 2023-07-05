using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtentions
{
	[Serializable]
    public class Random<T>
    {
        [SerializeField] private List<ObjectInfo> _gameObjects = new();

		private List<T> _objectList = new();
		private bool _generated = false;

		public static implicit operator T(Random<T> @object)
		{
			return @object.GetRandomObject();
		}

		public T GetRandomObject()
		{
			GenerateList();
			return _objectList[UnityEngine.Random.Range(0, _objectList.Count)];
		}

		private void GenerateList()
		{
			if (_generated)
			{
				return;
			}
			_generated = true;
			foreach (ObjectInfo info in _gameObjects)
			{
				float wight = info.Weight;
				while (wight > 0)
				{
					wight -= 1;
					_objectList.Add(info.Object);
				}
			}
		}

		private float GetWeightSum()
		{
			float result = 0;

			foreach (ObjectInfo info in _gameObjects)
			{
				result += info.Weight;
			}

			return result;
		}

		[Serializable]
		private class ObjectInfo
		{
			public T Object;
			public int Weight;
			public float Chance;
		}
#if UNITY_EDITOR
		public void OnValidate()
		{
			GenerateList();

			float sum = GetWeightSum();
			foreach (ObjectInfo info in _gameObjects)
			{
				info.Chance = info.Weight / sum;
			}
		}
#endif
	}
}
