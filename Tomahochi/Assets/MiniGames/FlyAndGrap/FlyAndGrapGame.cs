using Pets;
using Saving;
using System.Collections;
using TMPro;
using UnityEngine;

public class FlyAndGrapGame : MonoBehaviour
{
	[SerializeField] private GameObject _miniGameView;
	[SerializeField] private GameObject _loseWindow;
	[SerializeField] private GameObject _spikeObject;
	[SerializeField] private TMP_Text _scoreCaption;
	[SerializeField] private float _score;
	[SerializeField] private Player _player;
	[SerializeField] private GameState _state;
	[SerializeField] private float _joyRestorePerSecond;
	[SerializeField] private float _xpPerSecond;

	private string _scoreFormat;

	public PetSaveInfo PlayingPet => PlayerDataContainer.UnlockedPets[PlayerDataContainer.PlayingPetIndex];

	private void Start()
	{
		_scoreFormat = _scoreCaption.text;
	}

	private void OnEnable()
	{
		_player.DragStarted += OnPlayerMoveStart;
	}

	private void OnDisable()
	{
		_player.DragStarted -= OnPlayerMoveStart;
	}

	private void OnPlayerMoveStart()
	{
		if (_state != GameState.Prepair)
		{
			return;
		}
		_spikeObject.SetActive(true);
		_state = GameState.InProcess;
		_player.DragStarted -= OnPlayerMoveStart;
		StartCoroutine(TimerCoroutine());
	}

	private IEnumerator TimerCoroutine()
	{
		while (_state == GameState.InProcess)
		{
			_score += Time.deltaTime;

			if (PlayingPet.Joy < Pet.MIN_JOY_TO_PLAY)
			{
				PlayingPet.GainXP(_xpPerSecond * Time.deltaTime);
			}
			PlayingPet.Joy += _joyRestorePerSecond * Time.deltaTime;

			yield return null;
		}
	}

	public void ShowLoseWindow()
	{
		if (_state == GameState.End)
		{
			return;
		}

		_state = GameState.End;
		_loseWindow.SetActive(true);
		_scoreCaption.text = string.Format(_scoreFormat, _score);
	}

	private enum GameState
	{
		Prepair,
		InProcess,
		End
	}
}
