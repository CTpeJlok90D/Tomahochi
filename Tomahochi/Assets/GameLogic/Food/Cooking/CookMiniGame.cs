using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CookMiniGame : MonoBehaviour
{
	[SerializeField] private float _timeInterval = 5f;
	[SerializeField] private Vector2 _acceptebleTimeInterval = new(1,2);
	[SerializeField] private float _currentTime = 0;
	[SerializeField] private UnityEvent<Result> _gameEnded;

	private Coroutine _timerCoroutine;

	public UnityEvent<Result> GameEnded => _gameEnded;
	public bool IsLaunched => _timerCoroutine != null;

	public void SetUpSettings(float timeInterval, Vector2 acceptebleTimeInterval)
	{
		_timeInterval = timeInterval;
		_acceptebleTimeInterval = acceptebleTimeInterval;
	}

	public void StartGame()
	{
		_timerCoroutine = StartCoroutine(TimerCoroutine());
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
			_gameEnded.Invoke(Result.Success);
			return;
		}
		_gameEnded.Invoke(Result.Miss);
	}

	public void OnTimeEnd()
	{
		_gameEnded.Invoke(Result.Timeout);
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
		}
	}
#endif
}
