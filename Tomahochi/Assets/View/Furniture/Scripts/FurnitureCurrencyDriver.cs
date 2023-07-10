using Memory;
using Saving;
using System;
using UnityEngine;

public class FurnitureCurrencyDriver : MonoBehaviour
{
	[SerializeField] private int _moraMaxCount;
	[SerializeField] private int _gemsMaxCount;
	[SerializeField] private float _moraPerHour;
	[SerializeField] private float _gemsPerHour;
	[SerializeField] private FurnitureView _view;
	[SerializeField] private SaveData _data;

	private const float SECONDS_IN_HOUR = 3600;

	public int MoraCount => (int)_data.MoraCount;

	public int GemsCount => (int)_data.GemsCount;

	private void OnEnable()
	{
		_view.Created += OnCreate;
		PlayerDataContainer.Loaded.AddListener(OnLoad);
	}

	private void OnDisable()
	{
		_view.Created -= OnCreate;
		PlayerDataContainer.Loaded.RemoveListener(OnLoad);
	}

	private void OnDestroy()
	{
		new Save(_view.Source.ID, _data);
	}

	private void OnLoad()
	{
		_data.MoraCount = Mathf.Clamp(_data.MoraCount + PlayerDataContainer.SecondsPassed * _moraPerHour / SECONDS_IN_HOUR, 0, _moraMaxCount);
		_data.GemsCount = Mathf.Clamp(_data.GemsCount + PlayerDataContainer.SecondsPassed * _gemsPerHour / SECONDS_IN_HOUR, 0, _gemsMaxCount);
	}

	private void OnCreate()
	{
		_data = new Load<SaveData>(_view.Source.ID);
		_data ??= new();
	}

	private void Update()
	{
		_data.MoraCount += Time.deltaTime * _moraPerHour / SECONDS_IN_HOUR;
		_data.GemsCount += Time.deltaTime * _gemsPerHour / SECONDS_IN_HOUR;
	}

	private void OnMouseUp()
	{
		PlayerDataContainer.MoraCount += (int)_data.MoraCount;
		PlayerDataContainer.GemsCount += (int)_data.GemsCount;
		_data.MoraCount -= (int)_data.MoraCount;
		_data.GemsCount -= (int)_data.GemsCount;
	}

	[Serializable]
	private class SaveData
	{
		public float MoraCount;
		public float GemsCount;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_view ??= GetComponent<FurnitureView>();
	}
#endif
}
