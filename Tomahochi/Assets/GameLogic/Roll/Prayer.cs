using System.Collections.Generic;
using UnityEngine;

public class Prayer : MonoBehaviour
{
	[SerializeField] private PrayDropTable _dropTable;
	[SerializeField] private Transform _content;

	public void Pray() 
	{ 
		ILootDrop drop = _dropTable.Pray();
		drop.ApplyLoot();
	}

	public void PrayX(int count)
	{
		
		List<ILootDrop> drops = new();
		for (int i = 0; i < count; i++)
		{
			drops.Add(_dropTable.Pray());
		}
		ShowLoot(drops);
	}

	public void ShowLoot(ILootDrop loot)
	{
		ClearLoot();
	}
	public void ShowLoot(List<ILootDrop> loot)
	{
		ClearLoot();
	}
	public void ClearLoot()
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
	}
}
