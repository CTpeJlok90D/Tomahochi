using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Selecteble : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera _camera;
	[SerializeField] private int _cameraPryorityChange = 50;
	[SerializeField] private bool _deleselectOnRepeatTab = false;
	[SerializeField] private bool _disableCameraMovement = false;
	[SerializeField] private bool _canSelectOtherWhileSelected = true;
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

	private static bool _canSelectOther = true;

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
		if (IsSelected && _deleselectOnRepeatTab == false)
		{
			return;
		}
		if (_canSelectOther == false)
		{
			if (IsSelected)
			{
				Deselect();
			}
			return;
		}
		IsSelected = IsSelected == false;
	}

	public void Select()
	{
		if (IsSelected)
		{
			return;
		}

		_selectedObject?.Deselect();
		_selectedObject = this;
		_canSelectOther = _canSelectOtherWhileSelected;
		_camera.Priority += _cameraPryorityChange;
		MoveCameraPanel.Singletone.enabled = false || !_disableCameraMovement;
		_selectedObjectChanged.Invoke(_selectedObject);
		_selected?.Invoke(_selectedObject);
	}

	public void Deselect()
	{
		if (IsSelected == false)
		{
			return;
		}
		_selectedObject = null;
		_canSelectOther = true;
		_camera.Priority -= _cameraPryorityChange;
		MoveCameraPanel.Singletone.enabled = true;
		_selectedObjectChanged.Invoke(_selectedObject);
		_deselected?.Invoke();
	}
}
