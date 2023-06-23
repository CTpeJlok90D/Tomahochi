using UnityEngine;
using UnityEngine.UI;

public class ChangeUIModeButton : MonoBehaviour
{
	[SerializeField] private Button _button;
	[SerializeField] private UIMode _mode;
	protected void OnEnable()
	{
		_button.onClick.AddListener(OnButtonClick);
	}

	protected void OnDisable()
	{
		_button.onClick.RemoveListener(OnButtonClick);
	}

	private void OnButtonClick()
	{
		if (UI.Mode == _mode)
		{
			UI.Mode = UIMode.Defualt;
		}
		else
		{
			UI.Mode = _mode;
		}
	}
}
