using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SoldItem", menuName = "Shop/SoldItem")]
public class SoldItem : ScriptableObject
{
	[SerializeField] private string _soldName = "{sold name}";
	[TextArea(3,10)]
	[SerializeField] private string _soldDescription = "{sold description}";
	[SerializeField] private int _moraPrice;
	[SerializeField] private int _gemsPrice;
	[SerializeField] private int _count = 1;
	[SerializeField] private Storageble _item;

	private UnityEvent<Storageble> _itemSolden = new();

	public int MoraPrice => _moraPrice;
	public int GemsPrice => _gemsPrice;
	public Storageble Item => _item;
	public int Count => _count;
	public string Name => _soldName;
	public string Description => _soldDescription;
	public UnityEvent<Storageble> ItemSolden => _itemSolden;

	public bool CanBuy() => _moraPrice < PlayerDataContainer.MoraCount && _gemsPrice < PlayerDataContainer.GemsCount;
	public void Buy()
	{
		if (CanBuy() == false)
		{
			return;
		}

		PlayerDataContainer.GemsCount -= _gemsPrice;
		PlayerDataContainer.MoraCount -= _moraPrice;
		_item.AddOnStorage(_count);
		ItemSolden.Invoke(_item);
	}
}
