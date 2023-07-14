using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootView : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private TMP_Text _text;

	public void Init(ILootDrop drop)
	{
		_image.sprite = drop.ViewSprite;
		_text.text = drop.ViewCation;
	}
}
