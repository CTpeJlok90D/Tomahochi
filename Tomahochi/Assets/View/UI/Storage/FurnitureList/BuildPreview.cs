using System.Collections.Generic;
using UnityEngine;

public class BuildPreview : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Color _canBuildColor;
	[SerializeField] private Color _cantBuildColor;
	[SerializeField] private LayerMask _layers;
	private InstallResult _result = InstallResult.InProgress;
	private Vector2 _size;

	public InstallResult Result => _result;

	public BuildPreview Init(Sprite viewSprite, Vector2 size)
	{
		_spriteRenderer.sprite = viewSprite;
		_size = size;
		return this;
	}

	private void Awake()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = mousePosition;
	}

	private void Update()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = mousePosition;

		_spriteRenderer.color = CanBuild() ? _canBuildColor : _cantBuildColor;
	}

	private bool CanBuild()
	{
		List<RaycastHit2D> hits = new();
		Vector2 invertedSize = new Vector2(_size.x, -_size.y);

		RaycastHit2D Raycast(Vector2 size) => Physics2D.Raycast((Vector2)transform.position + size, Vector2.zero, Mathf.Infinity, _layers);
		hits.Add(Raycast(_size/2));
		hits.Add(Raycast(-_size/2));
		hits.Add(Raycast(-invertedSize/2));
		hits.Add(Raycast(invertedSize/2));

		points = new Vector2[]
		{
			_size/2,
			-_size/2,
			-invertedSize/2,
			invertedSize/2
		};

		foreach (RaycastHit2D hit in hits)
		{
			if (hit == false || hit.collider.TryGetComponent(out Floor floor) == false)
			{
				return false;
			}
		}
		return true;
	}

	private void OnMouseDown()
	{
		if (CanBuild())
		{
			_result = InstallResult.Success;
			return;
		}
		_result = InstallResult.Error;
	}

	public enum InstallResult
	{
		Success,
		Error,
		InProgress
	}

	private Vector2[] points;
	private void OnDrawGizmos()
	{
		foreach (Vector2 point in points)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere((Vector2)transform.position + point, 0.25f);
		}
	}
}
