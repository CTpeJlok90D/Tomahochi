using Saving;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Praying/MoraDropItem")]
public class MoraPrayDropItem : ScriptableObject, ILootDrop
{
	[SerializeField] private Sprite _viewSprite;
	[SerializeField] private Vector2Int _moraCountRange = new(100, 2000);
	private UnityEvent _gotLoot;

	public Sprite ViewSprite => _viewSprite;
	private int _moraCount;

	public int MoraCount => _moraCount;

	public string ViewCation => _moraCount.ToString();

	public UnityEvent GotLoot => _gotLoot;

	public void ApplyLoot()
	{
		_moraCount = Random.Range(_moraCountRange[0], _moraCountRange[1]+1);
		PlayerDataContainer.MoraCount += _moraCount;

		_gotLoot.Invoke();
	}
}
