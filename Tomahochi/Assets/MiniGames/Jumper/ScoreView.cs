using System;
using TMPro;
using UnityEngine;

[Serializable]
public class ScoreView : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreCaption;
	[SerializeField] private MiniGame _jumperMiniGame;

	private string _scoreFormat;

	private void Awake()
	{
		_scoreFormat = _scoreCaption.text;
		_scoreCaption.text = string.Format(_scoreFormat, 0);
	}

	private void OnEnable()
	{
		_jumperMiniGame.ScoreChanged.AddListener(OnScoreChange);
	}

	private void OnDisable()
	{
		_jumperMiniGame.ScoreChanged.RemoveListener(OnScoreChange);
	}

	private void OnScoreChange(float newValue)
	{
		_scoreCaption.text = string.Format(_scoreFormat, (int)newValue);
	}
}
