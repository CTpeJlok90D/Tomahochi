using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameSelect : MonoBehaviour
{
	[SerializeField] private List<MiniGameInfo> _miniGames;
	[SerializeField] private Image _miniGameIcon;
	[SerializeField] private TMP_Text _miniGameCaption;
	[SerializeField] private Button _nextButton;
	[SerializeField] private Button _previewButton;
	[SerializeField] private LoadSceneButton _loadSceneButton;

	private int _selectedMiniGameIndex;
	private string _miniGameNameFormat;
	private MiniGameInfo SelectedMiniGame => _miniGames[SelectedMiniGameIndex];
	public int SelectedMiniGameIndex
	{
		get
		{
			return _selectedMiniGameIndex;
		}
		set
		{
			_selectedMiniGameIndex = Mathf.Clamp(value, 0, _miniGames.Count - 1);
			_miniGameCaption.text = string.Format(_miniGameNameFormat, SelectedMiniGame.Name);
			_miniGameIcon.sprite = SelectedMiniGame.Icon;
			_loadSceneButton.SceneNumber = SelectedMiniGame.SceneNumber;
		}
	}

	private void Start()
	{
		_miniGameNameFormat = _miniGameCaption.text;
		SelectedMiniGameIndex = 0;
	}

	private void OnEnable()
	{
		_nextButton.onClick.AddListener(NextMiniGame);
		_previewButton.onClick.AddListener(PreviewMiniGame);
	}

	private void OnDisable()
	{
		_nextButton.onClick.RemoveListener(NextMiniGame);
		_previewButton.onClick.RemoveListener(PreviewMiniGame);
	}

	public void NextMiniGame()
	{
		SelectedMiniGameIndex++;
	}

	public void PreviewMiniGame()
	{
		SelectedMiniGameIndex--;
	}

	[Serializable]
	private struct MiniGameInfo
	{
		public Sprite Icon;
		public string Name;
		public int SceneNumber;
	}
}
