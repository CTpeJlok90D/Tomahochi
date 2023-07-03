using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : SmartMonoBehaivor
{
	[SerializeField] private Rigidbody2D _rigidbody;
	[SerializeField] private float _startForce = 250;
	protected override void Start()
	{
		base.Awake();
		_rigidbody.AddForce(transform.up * _startForce);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Remove(this);
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_rigidbody ??= GetComponent<Rigidbody2D>();
	}
#endif
}
