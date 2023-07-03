using UnityEngine;
using UnityExtentions;

public class HideblePlatform : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private Color _color;
	[SerializeField] private Collider2D _collider;
	[SerializeField] private Range _viewTime = new Range(2, 3);
	[SerializeField] private Range _hideTime = new Range(0.2f, 0.8f);

	private float _viewTimeValue;
	private float _hideTimeValue;
	private float _timer;
	private bool _enabled = true;
	private Color _defualtColor;

	private void OnEnable()
	{
		_viewTimeValue = _viewTime;
		_hideTimeValue = _hideTime;
		_defualtColor = _renderer.color;
		_renderer.color = _color;
	}

	private void Update()
	{
		_timer += Time.deltaTime;
		if (_enabled && _timer >= _viewTimeValue)
		{
			_timer = 0;
			_renderer.enabled = false;
			_collider.enabled = false;
			_enabled = false;
			return;
		}
		if (_enabled == false && _timer >= _hideTimeValue)
		{
			_timer = 0;
			_renderer.enabled = true;
			_collider.enabled = true;
			_enabled = true;
			return;
		}
	}

	private void OnDisable()
	{
		_renderer.enabled = true;
		_collider.enabled = true;
		_renderer.color = _defualtColor;
	}
}
