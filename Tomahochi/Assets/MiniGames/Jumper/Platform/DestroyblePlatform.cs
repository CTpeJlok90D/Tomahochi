using System.Collections;
using UnityEngine;
using UnityExtentions;

public class DestroyblePlatform : MonoBehaviour
{
	[SerializeField] private Range _destroyTime;
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private Collider2D _collider;
	[SerializeField] private Color _color;

	private Color _defualtColor;
	private Collision2D _playerCollision;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent(out IMiniGamePlayer player) == false || isActiveAndEnabled == false)
		{
			return;
		}

		StartCoroutine(DestroyCoroutine(_destroyTime));
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision != _playerCollision)
		{
			return;
		}

		_playerCollision = null;
	}

	private IEnumerator DestroyCoroutine(float time)
	{
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		if (_playerCollision != null)
		{
			_playerCollision.rigidbody.gravityScale = 1;
		}
		_collider.enabled = false;
		_renderer.enabled = false;
	}

	private void OnEnable()
	{
		_defualtColor = _renderer.color;
		_renderer.color = _color;
	}

	private void OnDisable()
	{
		_collider.enabled = true;
		_renderer.enabled = true;
		_renderer.color = _defualtColor;
	}
}
