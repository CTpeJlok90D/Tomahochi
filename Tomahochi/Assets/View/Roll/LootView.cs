using UnityEngine;
using UnityEngine.UI;

public class LootView : MonoBehaviour
{
	[SerializeField] private Image _image;

	public void Init(ILootDrop drop)
	{
		_image.sprite = drop.ViewSprite;
	}
}
