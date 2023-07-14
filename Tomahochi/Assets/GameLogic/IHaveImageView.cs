using UnityEngine;

public interface ILootDrop
{
	public Sprite ViewSprite { get; }
	public string ViewCation { get; }
	public void ApplyLoot();
}
