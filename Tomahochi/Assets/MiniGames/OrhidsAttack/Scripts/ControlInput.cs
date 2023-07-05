using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OrhidsAttack
{
	[AddComponentMenu("Mini-games/Orhids Attack/ControlInput")]
	public class ControlInput : MonoBehaviour
	{
		[SerializeField] private UnityEvent<int> _inputChanged = new();
		[SerializeField] private UnityEvent _shootPressed = new();
		[SerializeField] private KeyCode _leftKeyCode = KeyCode.A;
		[SerializeField] private KeyCode _righeKeyCode = KeyCode.D;
		[SerializeField] private List<KeyCode> _shootKeyCodes = new();

		private int _input = 0;

		private void Update()
		{
			MoveInput();
			ShootInput();
		}

		private void MoveInput()
		{
			bool pressed = 
				   Input.GetKeyUp(_leftKeyCode) 
				|| Input.GetKeyUp(_righeKeyCode) 
				|| Input.GetKeyDown(_leftKeyCode) 
				|| Input.GetKeyDown(_righeKeyCode);


			if (Input.GetKeyUp(_leftKeyCode))
				_input += 1;
			if (Input.GetKeyDown(_leftKeyCode))
				_input -= 1;

			if (Input.GetKeyUp(_righeKeyCode))
				_input -= 1;
			if (Input.GetKeyDown(_righeKeyCode))
				_input += 1;
			if (pressed)
				_inputChanged.Invoke(_input);
		}

		private void ShootInput()
		{
			foreach (KeyCode keyCode in _shootKeyCodes) 
			{
				if (Input.GetKey(keyCode))
				{
					_shootPressed.Invoke();
				}
			}
			if (Input.GetMouseButton(0))
			{
				_shootPressed.Invoke();
			}
		}
	}
}