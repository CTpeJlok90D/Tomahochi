using UnityEngine;

[CreateAssetMenu(fileName = "Ingridient", menuName = "Cooking/Ingridient")]
public class Ingredient : ScriptableObject
{
	[SerializeField] private string _viewName = "<View name>";
	[SerializeField] private string _viewDescription = "<view description>";
	[SerializeField] private Sprite _viewSprite;

	public string ViewName => _viewName;
	public string ViewDescription => _viewDescription;
	public Sprite ViewSprite => _viewSprite;
}
