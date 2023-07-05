using UnityEngine;
using UnityEngine.UI;

public class EndGameButton : MonoBehaviour
{
	[SerializeField] private MiniGame _miniGame;
	[SerializeField] private Button _button;

	private void OnEnable()
	{
		_button.onClick.AddListener(OnClick);
	}

	private void OnDisable()
	{
		_button.onClick.RemoveListener(OnClick);
	}

	private void OnClick()
	{
		_miniGame.EndGame();
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_button ??= GetComponent<Button>();
	}
#endif
}
