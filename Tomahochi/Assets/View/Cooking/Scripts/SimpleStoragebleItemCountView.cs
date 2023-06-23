using TMPro;
using UnityEngine;

public class SimpleStoragebleItemCountView : MonoBehaviour
{
	[SerializeField] private Storageble _storageble;
	[SerializeField] private TMP_Text _countCapltion;

	private string _countFormat;

	public void Init(Storageble stotageble)
	{
		_storageble = stotageble;
		OnEnable();
		Start();
	}

	private void Start()
	{
		_countFormat = _countCapltion.text;
		if (_storageble == null)
		{
			return;
		}
		UpdateCount();
	}

	private void OnEnable()
	{
		_storageble?.OnStorageCountChanged.AddListener(OnCountChanged);
		if (string.IsNullOrEmpty(_countFormat) == false)
		{
			UpdateCount();
		}
	}

	private void OnDisable()
	{
		_storageble?.OnStorageCountChanged.RemoveListener(OnCountChanged);
	}

	private void OnCountChanged(Storageble item, int count)
	{
		if (item == _storageble)
		{
			UpdateCount();
		}
	}

	public void UpdateCount()
	{
		_countCapltion.text = string.Format(_countFormat, _storageble.GetStorageCount());
	}
}
