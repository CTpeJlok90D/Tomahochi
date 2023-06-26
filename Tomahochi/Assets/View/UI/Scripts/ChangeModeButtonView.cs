using TMPro;
using UnityEngine;

public class ChangeModeButtonView : MonoBehaviour
{
	[SerializeField] private TMP_Text _caption;
	[SerializeField] private UIMode _mode;
	[SerializeField] private string _defualtCaption;
	[SerializeField] private string _exitCaption;

	private void OnEnable()
	{
		UI.ModeChanged += OnModeChanged;
	}

	private void OnDisable()
	{
		if (UI.HaveInstance)
		{
			UI.ModeChanged -= OnModeChanged;
		}
	}

	private void OnModeChanged(UIMode mode)
	{
		_caption.text = mode == _mode ? _exitCaption : _defualtCaption;
	}
}
