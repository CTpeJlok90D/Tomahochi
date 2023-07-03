using System;
using UnityEngine;
using Range = UnityExtentions.Range;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class JumpPlatform : MonoBehaviour
{
	[SerializeField] private JumpPlatform _linkedPlatform;
	[SerializeField] private JumpPlatform _lastPlatform;
	[SerializeField] private Range _scale = new(0.8f, 1.2f);
	[SerializeField] private Range _spawnDistanceX;
	[SerializeField] private Type _type;
	[SerializeField] private UnityDictionarity<Type, List<Behaviour>> _platformScriptPerType;

	private bool _wasGenerated = false;
	private Vector3 _defualtScale;

	private void Awake()
	{
		_defualtScale = transform.localScale;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		collision.transform.SetParent(transform);
		if (_wasGenerated)
		{
			return;
		}
		 
		if (collision.gameObject.TryGetComponent(out IMiniGamePlayer player))
		{
			_linkedPlatform.Generate();
			_wasGenerated = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		collision.transform.SetParent(null);
	}

	private void Generate()
	{
		transform.localScale = _defualtScale * _scale;
		Vector3 newPosition = _lastPlatform.transform.position + new Vector3(_spawnDistanceX, 0);
		transform.position = newPosition;
		foreach (Behaviour script in _platformScriptPerType[_type])
		{
			script.enabled = false;
		}

		Array types = Enum.GetValues(typeof(Type));
		_type = (Type)types.GetValue(Random.Range(0, types.Length));
		foreach (Behaviour script in _platformScriptPerType[_type])
		{
			script.enabled = true;
		}
		_wasGenerated = false;
	}

	[Serializable]
	private enum Type
	{
		Defualt,
		XMoveble,
		YMoveble,
		Hideble,
		Destroyeble
	}
}
