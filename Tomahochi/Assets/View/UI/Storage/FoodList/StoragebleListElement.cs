using Saving;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoragebleListElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private TMP_Text _countCaption;
	[SerializeField] private TMP_Text _nameCaption;
	[SerializeField] private int _count;
	[SerializeField] private Consumeble _consumeble;
	[SerializeField] private Image _image;

	private string _countCaptionFormat;
	private string _nameCaptionFormat;
	private Coroutine _dragCoroutine;
	private int _elementNumber;

	private Transform _parent;

	public int Count
	{
		get => _count;
		set
		{
			_count = value;
			_countCaption.text = string.Format(_countCaptionFormat, value);
		}
	}

	private void Awake()
	{
		_countCaptionFormat = _countCaption.text;
		_nameCaptionFormat = _nameCaption.text;
	}

	public StoragebleListElement Init(Consumeble consumeble, int count)
	{
		_nameCaption.text = string.Format(_nameCaptionFormat, consumeble.ViewName);
		_image.sprite = consumeble.ViewSprite;
		_consumeble = consumeble;
		Count = count;
		_consumeble.OnStorageCountChanged.AddListener(UpdateCount);
		return this;
	}

	private void OnDestroy()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			_consumeble?.OnStorageCountChanged?.RemoveListener(UpdateCount);
		}
	}

	public void UpdateCount(Storageble item, int count)
	{
		if (item == _consumeble)
		{
			Count = count;
		}
	}

	private IEnumerator DragCoroutine()
	{
		while (true)
		{
			transform.position = Input.mousePosition;
			yield return null;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_parent = transform.parent;
		_elementNumber = transform.GetSiblingIndex();
		transform.SetParent(_image.canvas.transform);
		_dragCoroutine = StartCoroutine(DragCoroutine());
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, worldMousePosition);

		if (hit.collider.TryGetComponent(out PetView pet))
		{
			_consumeble.Consume(pet.PetInfo);
		}

		transform.SetParent(_parent);
		transform.SetSiblingIndex(_elementNumber);
		if (_dragCoroutine != null)
		{
			StopCoroutine(_dragCoroutine);
		}
	}
}
