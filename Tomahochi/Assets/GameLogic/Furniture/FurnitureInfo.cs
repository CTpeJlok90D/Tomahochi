
using UnityEngine;

[CreateAssetMenu(menuName = "Furniture/Furniture", fileName = "Furniture")]
public class FurnitureInfo : ScriptableObject
{
	[SerializeField] private string _viewName;
	[SerializeField] private GameObject _viewPrefab;
	[SerializeField] private Sprite _viewIcon;
	[SerializeField] private Vector2 _size;

	public string ViewName => _viewName;
	public GameObject ViewPrefab => _viewPrefab;
	public Sprite ViewIcon => _viewIcon;
	public Vector2 Size => _size;
}
