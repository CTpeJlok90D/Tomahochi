using UnityEngine;
using UnityEngine.Events;

public interface ILootDrop
{
	public Sprite ViewSprite { get; }
	public string ViewCation { get; }
	public UnityEvent GotLoot { get; }
	public void ApplyLoot();
}
