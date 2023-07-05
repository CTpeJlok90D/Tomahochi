using System.Collections;
using UnityEngine;

public class FlyAndGrapGame : MiniGame
{
	[SerializeField] private GameObject _spikeObject;
	[SerializeField] private FlyAndGrapPlayer _player;

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
		if (State != GameState.Prepair)
		{
			return;
		}
		IncreaseJoy = true;
		IncreaseXP = true;
		_spikeObject.SetActive(true);
		State = GameState.InProcess;
		_player.DragStarted -= OnPlayerMoveStart;
		StartCoroutine(TimerCoroutine());
	}

	protected IEnumerator TimerCoroutine()
	{
		while (State == GameState.InProcess)
		{
			Score += Time.deltaTime;

			yield return null;
		}
	}
}
