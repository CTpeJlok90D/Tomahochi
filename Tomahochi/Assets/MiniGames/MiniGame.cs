using Pets;
using Saving;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MiniGame : MonoBehaviour
{
	[SerializeField] protected GameState State;
	[SerializeField] private GameObject _loseWindow;
	[SerializeField] private TMP_Text _scoreCaption;
	[SerializeField] private TMP_Text _bestScoreCaption;
	[SerializeField] private TMP_Text _newRecordText;
	[SerializeField] private float _joyRestorePerSecond = 1.5f;
	[SerializeField] private float _xpPerSecond = 0.15f;

	private float _score;
	private string _scoreFormat;
	private string _bestScoreFormat;
	public PetSaveInfo PlayingPet => PlayerDataContainer.UnlockedPets[PlayerDataContainer.PlayingPetIndex];

	public GameObject LoseWindow => _loseWindow;
	protected virtual void Start()
	{
		_scoreFormat = _scoreCaption.text;
		_bestScoreFormat = _bestScoreCaption.text;
	}

	protected IEnumerator TimerCoroutine()
	{
		while (State == GameState.InProcess)
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
		int miniGameIndex = SceneManager.GetActiveScene().buildIndex;
		float bestRecord = PlayerDataContainer.GetRecord(miniGameIndex);
		if (State == GameState.End)
		{
			return;
		}

		State = GameState.End;
		LoseWindow.SetActive(true);
		_scoreCaption.text = string.Format(_scoreFormat, (int)_score);
		_bestScoreCaption.text = string.Format(_bestScoreFormat, (int)bestRecord);

		if (bestRecord < _score)
		{
			_newRecordText.gameObject.SetActive(true);
			PlayerDataContainer.WriteRecord(miniGameIndex, _score);
		}
	}

	protected enum GameState
	{
		Prepair,
		InProcess,
		End
	}
}
