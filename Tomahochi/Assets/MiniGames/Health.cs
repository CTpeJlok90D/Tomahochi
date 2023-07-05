using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	[SerializeField] private int _value = 100;
	[SerializeField] private int _maxValue = 100;
	[SerializeField] private bool _maxOnAwake = true;
	[SerializeField] private UnityEvent _die = new();

	private void Awake()
	{
		if (_maxOnAwake)
		{
			_value = _maxValue;
		}
	}

	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = Mathf.Clamp(value, 0, _maxValue);
			if (_value == 0)
			{
				_die.Invoke();
			}
		}
	}
}
