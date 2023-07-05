using System;
using System.Collections.Generic;
using UnityEngine;
using Range = UnityExtentions.Range;
using Random = UnityEngine.Random;

public class ItemsGenerator : MonoBehaviour
{
	[SerializeField] private Transform _owner;
	[SerializeField] private bool _linkGeneratedItemToOwner = true;
	[SerializeField] private Range _ySpawnPositionRange;
	[SerializeField] private Range _xSpawnPositionRange;
	[SerializeField] private ObjectInfo[] _spawnObjects;
	[SerializeField] private Range _itemsCountRange = new(0,1);
	[SerializeField] private bool _generateOnAwake = true;

	private List<ObjectInfo> _randomList = new();

	private void Awake()
	{
		foreach (ObjectInfo info in _spawnObjects)
		{
			float wight = info.Weight;
			while (wight > 0)
			{
				wight -= 1;
				_randomList.Add(info);
			}
		}
		if (_generateOnAwake)
		{
			Generate();
		}
	}

	public void Generate()
	{
		float count = _itemsCountRange.RandomIntValue();
		
		for (int i = 0; i < count; i++)
		{
			ObjectInfo info = _randomList[Random.Range(0, _randomList.Count - 1)];

			GameObject instance =  Instantiate(info.GameObject);

			if (_linkGeneratedItemToOwner)
			{
				instance.transform.SetParent(_owner);
			}

			instance.transform.position = _owner.transform.position + new Vector3(_xSpawnPositionRange, _ySpawnPositionRange);
		}
	}

	private float GetWeightSum()
	{
		float result = 0;

		foreach (ObjectInfo info in _spawnObjects)
		{
			result += info.Weight;
		}

		return result;
	}

	[Serializable]
	private class ObjectInfo
	{
		public GameObject GameObject;
		public float Weight = 1;
		public float Chance;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_owner ??= transform.parent;
		float sum = GetWeightSum();
		foreach (ObjectInfo info in _spawnObjects)
		{
			info.Chance = info.Weight / sum;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;

		Vector3 center = new(_xSpawnPositionRange.Middle, _ySpawnPositionRange.Middle);
		Vector3 Size = new(_xSpawnPositionRange.Max - _xSpawnPositionRange.Min, _ySpawnPositionRange.Max - _ySpawnPositionRange.Min);

		Gizmos.DrawCube(_owner.transform.position + center, Size);
	}
#endif
}
