using UnityEngine;

[CreateAssetMenu]
public class Water : ScriptableObject
{
	[SerializeField] private string _viewName = "<view name>";
	[SerializeField] private string _viewDescription = "view description";
	[SerializeField] private Sprite _viewSprite;
	[SerializeField] private float _nutritional = 30;
	[SerializeField] private int _xpGain = 100;

	public string ViewName => _viewName;
	public string ViewDescription => _viewDescription;
	public Sprite ViewSprite => _viewSprite;
	public float Nutritional => _nutritional;
	public int XpCount => _xpGain;
}
