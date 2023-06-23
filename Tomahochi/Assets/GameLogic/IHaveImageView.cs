using UnityEngine;

public interface ILootDrop
{
	public Sprite ViewSprite { get; }
	public void ApplyLoot();
}
