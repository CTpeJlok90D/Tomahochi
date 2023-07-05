using System.Collections;
using UnityEngine;
using UnityExtentions;

public class FlyAndGrapPlayer : MonoBehaviour, IMiniGamePlayer
{
	[SerializeField] private MiniGame _game;
	[SerializeField] private GameObject _arrow;
	[SerializeField] private Rigidbody2D _rigidbody2D;
	[SerializeField] private Vector2 _sizeDiapason = new(1.1f, 3.5f);
	[SerializeField] private float _forceStrench = 250f;
	[SerializeField] private Range _forceDiapason = new(0, 350);

	private bool _canDrag = true;

	public delegate void MoveStarted();
	private event MoveStarted _dragStarted;

	public event MoveStarted DragStarted
	{
		add => _dragStarted+=value;
		remove => _dragStarted -= value;
	}

	private void OnMouseDown()
	{
		if (_canDrag == false)
		{
			return;
		}

		_dragStarted?.Invoke();
		StartCoroutine(ArrowCoroutine());
	}

	private IEnumerator ArrowCoroutine()
	{
		GameObject instance = Instantiate(_arrow, transform);
		_rigidbody2D.gravityScale = 0;
		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		while (Input.GetMouseButton(0))
		{
			mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			instance.transform.up = (Vector2)instance.transform.position - mouseWorldPosition;

			float yScale = Mathf.Clamp(Vector2.Distance(instance.transform.position, mouseWorldPosition), _sizeDiapason[0], _sizeDiapason[1]);
			instance.transform.localScale = new(1, yScale, 1);
			yield return null;
		}
		_rigidbody2D.gravityScale = 1;

		Vector2 delta = ((Vector2)instance.transform.position - mouseWorldPosition);
		float force = Mathf.Clamp(delta.magnitude * _forceStrench, _forceDiapason.Min, _forceDiapason.Max);
		_rigidbody2D.AddForce(delta.normalized * force);
		_canDrag = false;
		Destroy(instance);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Spike spike))
		{
			_game.EndGame();
			return;
		}

		_rigidbody2D.gravityScale = 0;
		_rigidbody2D.velocity = Vector2.zero;
		_rigidbody2D.angularVelocity = 0;
		_canDrag = true;
	}
}
