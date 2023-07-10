using Memory;
using Saving;
using System;
using UnityEngine;

public class StoragebleFacture : MonoBehaviour
{
	[SerializeField] private Storageble _storageble;
	[SerializeField] private int _maxCount = 1; 
	[SerializeField] private float _secondsToCreate = 3600;
	[SerializeField] private SaveData _data;
	[SerializeField] private FurnitureView _view;

	public int Ready => _data.Ready;
	public int Max => _maxCount;

	private void OnEnable()
	{
		PlayerDataContainer.Loaded.AddListener(OnLoad);
	}

	private void OnDisable()
	{
		PlayerDataContainer.Loaded.AddListener(OnLoad);
	}

	private void OnLoad()
	{
		_data = new Load<SaveData>(_view.Source.ID);
		_data ??= new();
		if (_data.Ready < _maxCount)
		{
			_data.Time = Mathf.Clamp(PlayerDataContainer.SecondsPassed + _data.Time, 0, _secondsToCreate);
		}
		while (_data.Time >= _secondsToCreate)
		{
			_data.Ready++;
			_data.Time -= _secondsToCreate;
		}
	}

	private void OnDestroy()
	{
		new Save(_view.Source.ID, _data);
	}

	private void Update()
	{
		_data.Time = Mathf.Clamp(Time.unscaledDeltaTime + _data.Time, 0, _secondsToCreate * _maxCount);
	}

	private void OnMouseDown()
	{
		if (_data.Ready == 0)
		{
			return;
		}
		_storageble.AddOnStorage(_data.Ready);
		_data.Ready = 0;
	}

	[Serializable]
	private class SaveData
	{
		public float Time;
		public int Ready;
	}
}
