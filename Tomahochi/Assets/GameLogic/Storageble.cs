using UnityEngine;
using UnityEngine.Events;

public abstract class Storageble : ScriptableObject, ILootDrop
{
	[SerializeField] private string _viewName = "<view name>";
	[SerializeField] private Sprite _viewSprite;

	public string ViewName => _viewName;
	public Sprite ViewSprite => _viewSprite;
	public abstract void AddOnStorage(int count);
	public abstract void RemoveFromStorage(int count);
	public abstract int GetStorageCount();
	public abstract void ApplyLoot();
	public string ViewCation => _viewName;

	public abstract UnityEvent<Storageble, int> OnStorageCountChanged { get; }

}