using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Selecteble : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera _camera;
	[SerializeField] private int _cameraPryorityChange = 50;
	[SerializeField] private bool _deleselectOnRepeatClick = false;
	[SerializeField] private bool _disableCameraMovement = false;
	[SerializeField] private bool _canSelectOtherWhileSelected = true;
	[SerializeField] private List<UIMode> _canBeSelectedInModes = new();

	public delegate void SelectedObjectChangedHandler(Selecteble selecteble);
	public delegate void DeSelectedObjectChangedHandler();
	private event SelectedObjectChangedHandler _selected;
	private event DeSelectedObjectChangedHandler _deselected;

	public event SelectedObjectChangedHandler Selected
	{
		add => _selected += value;
		remove => _selected -= value;
	}

	public event DeSelectedObjectChangedHandler Deselected
	{
		add => _deselected += value;
		remove => _deselected -= value;
	}

	public bool CurrentUIModeIsAccepteble => _canBeSelectedInModes.Contains(UI.Mode);

	private static Selecteble _selectedObject;
	public static Selecteble SelectedObject => _selectedObject;
	private static event SelectedObjectChangedHandler _selectedObjectChanged;
	public static event SelectedObjectChangedHandler SelectObjectChange
	{
		add
		{
			_selectedObjectChanged += value;
		}
		remove
		{
			_selectedObjectChanged -= value;
		}
	}
	public bool IsSelected
	{
		get
		{
			return _selectedObject == this;
		}
		set
		{
			if (value)
			{
				Select();
			}
			else
			{
				Deselect();
			}
		}
	}

	public void OnPlayerClick()
	{
		if (IsSelected && _deleselectOnRepeatClick == false)
		{
			return;
		}
		if (IsSelected)
		{
			Deselect();
			return;
		}
		IsSelected = IsSelected == false;
	}

	public void Select()
	{
		if ((IsSelected || CurrentUIModeIsAccepteble == false) ||
		 (SelectedObject != null && SelectedObject._canSelectOtherWhileSelected == false)) return;

		_selectedObject?.Deselect();
		_selectedObject = this;
		if (_camera != null)
		{
			_camera.Priority += _cameraPryorityChange;
		}
		MoveCameraPanel.Singletone.enabled = false || !_disableCameraMovement;
		_selectedObjectChanged?.Invoke(_selectedObject);
		_selected?.Invoke(_selectedObject);
	}

	public void Deselect()
	{
		if (IsSelected == false)
		{
			return;
		}

		_selectedObject = null;
		if (_camera != null)
		{
			_camera.Priority -= _cameraPryorityChange;
		}
		if (MoveCameraPanel.Singletone != null)
		{
			MoveCameraPanel.Singletone.enabled = true;
		}
		_selectedObjectChanged?.Invoke(_selectedObject);
		_deselected?.Invoke();
	}

	private void OnEnable()
	{
		UI.ModeChanged += OnModeChangedUI;
	}

	private void OnDisable()
	{
		UI.ModeChanged -= OnModeChangedUI;
		Deselect();
	}

	private void OnModeChangedUI(UIMode mode)
	{
		if (CurrentUIModeIsAccepteble == false)
		{
			Deselect();
		}
	}
}
