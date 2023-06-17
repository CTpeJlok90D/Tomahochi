using Pets;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Praying/PrayLootTable")]
public class PrayDropTable : ScriptableObject
{
	[SerializeField] private List<Pet> _4StarPets = new();
	[SerializeField] private List<Pet> _5StarPets = new();
	[SerializeField] private List<Furniture> _furniture = new();
	[SerializeField] private List<Food> _food = new();
	[SerializeField] private List<Water> _water = new();
	[SerializeField] private Vector2 _moraCountRange = new Vector2(100, 2000);

	public const int ROLL_COUNT_FOR_RARE_GUARANT = 10;
	public const int ROLL_COUNT_FOR_LEGENDARY_GUARANT = 10;

	public object Pray(int rareRollCount, int legendaryRollCount)
	{
		object result = null;
		if (legendaryRollCount >= ROLL_COUNT_FOR_LEGENDARY_GUARANT)
		{
			result = _5StarPets[Random.Range(0, _5StarPets.Count)];
		}
		if (rareRollCount >= ROLL_COUNT_FOR_RARE_GUARANT)
		{
			result = _4StarPets[Random.Range(0, _4StarPets.Count)];
		}
		if (result == null)
		{

		}
		return result;
	}
}
