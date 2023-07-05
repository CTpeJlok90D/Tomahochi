using System.Collections;
using UnityEngine;
using UnityExtentions;

public class JumperPlayer : MonoBehaviour, IMiniGamePlayer
{
	[SerializeField] private GameObject _arrow;
	[SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private Vector2 _sizeDiapason = new(1.1f, 3.5f);
	[SerializeField] private float _forceStrench = 250f;
	[SerializeField] private Range _forceDiapason = new(0, 350);
	[SerializeField] private float _loseYPosition;

	public delegate void DeadHandled();
	private event DeadHandled _die;

	public event DeadHandled Die
	{
		add => _die += value;
		remove => _die -= value;
	}

	private bool _canDrag = true;

	private void OnMouseDown()
	{
		if (_canDrag == false)
		{
			return;
		}

		StartCoroutine(ArrowCoroutine());
	}

	private IEnumerator ArrowCoroutine()
	{
		GameObject instance = Instantiate(_arrow, transform);
		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		while (Input.GetMouseButton(0))
		{
			mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			instance.transform.up = (Vector2)instance.transform.position - mouseWorldPosition;

			float yScale = Mathf.Clamp(Vector2.Distance(instance.transform.position, mouseWorldPosition), _sizeDiapason[0], _sizeDiapason[1]);
			instance.transform.localScale = new(1, yScale, 1);
			yield return null;
		}

		Vector2 delta = ((Vector2)instance.transform.position - mouseWorldPosition);
		float force = Mathf.Clamp(delta.magnitude * _forceStrench, _forceDiapason.Min, _forceDiapason.Max);
		_rigidbody2D.AddForce(delta.normalized * force);
		_canDrag = false;
		Destroy(instance);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		_rigidbody2D.velocity = Vector2.zero;
		_rigidbody2D.angularVelocity = 0;
		_canDrag = true;
	}

	private void Update()
	{
		if (transform.position.y < _loseYPosition)
		{
			_die?.Invoke();
		}
	}
}
