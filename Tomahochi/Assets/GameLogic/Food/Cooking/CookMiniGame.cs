using System.Collections;
using UnityEditor;
using UnityEngine;

public class CookMiniGame : MonoBehaviour
{
	[SerializeField] private float _timeInterval = 5f;
	[SerializeField] private Vector2 _acceptebleTimeInterval = new(1,2);
	[SerializeField] private float _currentTime = 0;

	public delegate void GameEndedHandler(Result result);
	private event GameEndedHandler _gameEnded;
	public delegate void GameStartedHandler();
	private event GameStartedHandler _gameStarted;

	private Coroutine _timerCoroutine;

	public event GameEndedHandler GameEnded
	{
		add => _gameEnded += value;
		remove => _gameEnded -= value;
	}
	public event GameStartedHandler GameStarted
	{
		add => _gameStarted += value;
		remove => _gameStarted -= value;
	}
	public bool IsLaunched => _timerCoroutine != null;
	public Vector2 AcceptebleInterval => _acceptebleTimeInterval;
	public float TimeInterval => _timeInterval;
	public float CurrentTime => _currentTime;

	public void SetUpSettings(float timeInterval, Vector2 acceptebleTimeInterval)
	{
		_timeInterval = timeInterval;
		_acceptebleTimeInterval = acceptebleTimeInterval;
	}

	public void StartGame()
	{
		_timerCoroutine = StartCoroutine(TimerCoroutine());
		_gameStarted();
	}

	private IEnumerator TimerCoroutine()
	{
		_currentTime = 0;
		while (_currentTime < _timeInterval)
		{
			_currentTime += Time.deltaTime;
			yield return null;
		}
		OnTimeEnd();
	}

	public void OnPlayerClick()
	{
		if (IsLaunched == false)
		{
			return;
		}

		StopCoroutine(_timerCoroutine);
		if (_currentTime > _acceptebleTimeInterval[0] && _currentTime < _acceptebleTimeInterval[1])
		{
			_gameEnded(Result.Success);
			return;
		}
		_gameEnded(Result.Miss);
	}

	public void OnTimeEnd()
	{
		_gameEnded(Result.Timeout);
	}

	public enum Result
	{
		Success,
		Timeout,
		Miss
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(CookMiniGame))]
	public class CookMiniGameEditor : Editor
	{
		private new CookMiniGame target => base.target as CookMiniGame;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("COOK!"))
			{
				target.OnPlayerClick();
			}
			if (GUILayout.Button("Luanch mini-game"))
			{
				target.StartGame();
			}
		}
	}
#endif
}
