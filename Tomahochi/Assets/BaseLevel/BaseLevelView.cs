using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseLevelView : MonoBehaviour
{
	[SerializeField] private BaseLevel _baseLevel;
	[SerializeField] private TMP_Text _levelCaption;
	[SerializeField] private Slider _xpView;

	private string _levelFormat;
	private bool _loadFormats = true;

	private void OnEnable()
	{
		_baseLevel.XPCountChanged.AddListener(OnXpCountChanged);
		_baseLevel.LevelChanged.AddListener(OnLevelChanged);
		OnLevelChanged(_baseLevel.Level);
		OnXpCountChanged(_baseLevel.XP);
	}

	private void OnDisable()
	{
		_baseLevel.XPCountChanged.RemoveListener(OnXpCountChanged);
		_baseLevel.LevelChanged.RemoveListener(OnLevelChanged);
	}

	private void LoadFormats()
	{
		_levelFormat = _levelCaption.text;
		_loadFormats = false;
	}

	private void OnLevelChanged(int newLevel)
	{
		if (_loadFormats)
		{
			LoadFormats();
		}
		_levelCaption.text = string.Format(_levelFormat, newLevel);
	}

	private void OnXpCountChanged(float newXP)
	{
		_xpView.value = newXP / _baseLevel.XpToLevelUp;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_baseLevel ??= FindObjectOfType<BaseLevel>();
	}
#endif
}
