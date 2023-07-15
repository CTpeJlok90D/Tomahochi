using Pets;
using Saving;
using UnityEngine;
using UnityEngine.Events;

public abstract class MiniGame : MonoBehaviour
{
	[SerializeField] protected GameState State;
	[SerializeField] private LosePanel _loseWindow;
	[SerializeField] private float _joyRestorePerSecond = 1.5f;
	[SerializeField] private float _xpPerSecond = 0.15f;
	[SerializeField] private float _score;
	[SerializeField] private UnityEvent<float> _scoreChanged = new();
	public float Score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			_scoreChanged.Invoke(_score);
		}
	}

	protected bool IncreaseXP;
	protected bool IncreaseJoy;
	public UnityEvent<float> ScoreChanged => _scoreChanged;

	public PetSaveInfo PlayingPet => PlayerDataContainer.UnlockedPets[PlayerDataContainer.PlayingPetIndex];

	protected virtual void Start()
	{
#if UNITY_EDITOR
		if (PlayerDataContainer.HaveInstance == false)
		{
			Debug.LogWarning("Init was not found!");
			return;
		}
#endif
	}

	protected virtual void Update()
	{
#if UNITY_EDITOR
		if (PlayerDataContainer.HaveInstance == false)
		{
			return;
		}
#endif
		if (PlayingPet.Joy < 100 && IncreaseJoy)
		{
			PlayingPet.GainXP(_xpPerSecond * Time.deltaTime);
		}
		if (IncreaseJoy)
		{
			PlayingPet.Joy = Mathf.Clamp(PlayingPet.Joy + _joyRestorePerSecond * Time.deltaTime, 0, 100);
		}
	}

	public void EndGame()
	{
		if (State == GameState.End)
		{
			return;
		}

		State = GameState.End;
		_loseWindow.Show((int)Score);
		IncreaseJoy = false;
		IncreaseXP = false;
		OnGameEnd();
	}
	protected virtual void OnGameEnd()
	{

	}
	protected enum GameState
	{
		Prepair,
		InProcess,
		End
	}
}
