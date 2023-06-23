using Pets;
using UnityEngine;

public abstract class Consumeble : Storageble
{
	[SerializeField] private float _nutritional = 30;
	[SerializeField] private int _xpGain = 100;

	public float Nutritional => _nutritional;
	public int XpCount => _xpGain;

	public abstract void Consume(PetSaveInfo pet);
}