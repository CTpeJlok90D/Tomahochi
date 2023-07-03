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
		_spikeObject.SetActive(true);
		State = GameState.InProcess;
		_player.DragStarted -= OnPlayerMoveStart;
		StartCoroutine(TimerCoroutine());
	}
}
