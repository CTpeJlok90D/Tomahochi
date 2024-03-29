using Saving;
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
		_storageble?.OnStorageCountChanged?.AddListener(OnCountChanged);
		_countFormat = _countCapltion.text;
		if (_storageble == null)
		{
			return;
		}
		UpdateCount();
	}

	private void OnEnable()
	{
		if (string.IsNullOrEmpty(_countFormat) == false)
		{
			UpdateCount();
		}
	}

	private void OnDestroy()
	{
		if (PlayerDataContainer.HaveInstance == false)
		{
			return;
		}
		_storageble.OnStorageCountChanged?.RemoveListener(OnCountChanged);
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
