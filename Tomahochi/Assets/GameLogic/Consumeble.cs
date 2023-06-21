using Pets;
using UnityEngine;

public abstract class Consumeble : ScriptableObject
{
	[SerializeField] private string _viewName = "<view name>";
	[SerializeField] private string _viewDescription = "<view description>";
	[SerializeField] private Sprite _viewSprite;
	[SerializeField] private float _nutritional = 30;
	[SerializeField] private int _xpGain = 100;

	public string ViewName => _viewName;
	public string ViewDescription => _viewDescription;
	public Sprite ViewSprite => _viewSprite;
	public float Nutritional => _nutritional;
	public int XpCount => _xpGain;

	public abstract void AddOnStorage(int count);
	public abstract void RemoveFromStorage(int count);
	public abstract int GetStorageCount();
	public abstract void Consume(PetSaveInfo pet);
}