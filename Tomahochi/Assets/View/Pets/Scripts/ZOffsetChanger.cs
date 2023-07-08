using System.Collections.Generic;
using UnityEngine;

public class ZOffsetChanger : MonoBehaviour
{
	private const int Z_ORDER_MULTYPLY = 100;
	private const float Z_OFFCEST_COEFFICIENT = 0.1f;

	[SerializeField] private Transform _target;
	[SerializeField] private List<SpriteRenderer> _spriteRenderers;
	[SerializeField] private bool _static = false;

	private Dictionary<SpriteRenderer, int> _defualtOrder = new();


	private void Awake()
	{
		foreach (SpriteRenderer renderer in _spriteRenderers)
		{
			_defualtOrder.Add(renderer, renderer.sortingOrder);
		}
		SetRenderOrders();
	}

	private void LateUpdate()
	{
		if (_static)
		{
			return;
		}
		SetRenderOrders();
	}

	private void SetRenderOrders()
	{
		transform.position = new(transform.position.x, transform.position.y, _target.position.y * Z_OFFCEST_COEFFICIENT);
		foreach (SpriteRenderer renderer in _spriteRenderers)
		{
			renderer.sortingOrder = (int)(_defualtOrder[renderer] - Z_ORDER_MULTYPLY * _target.position.y);
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_target ??= transform;
	}
#endif
}
