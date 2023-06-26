using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpriteShowerOnUIMode : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _sprite;
	[SerializeField] private List<UIMode> _acceptebleModes;
	private void OnEnable()
	{
		UI.ModeChanged += OnModeChange;
	}

	private void OnDisable()
	{
		UI.ModeChanged -= OnModeChange;
	}

	private void Start()
	{
		_sprite.enabled = _acceptebleModes.Contains(UI.Mode);
	}

	private void OnModeChange(UIMode newMode)
	{
		_sprite.enabled = _acceptebleModes.Contains(newMode);
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_sprite ??= GetComponent<SpriteRenderer>();
	}
#endif
}
