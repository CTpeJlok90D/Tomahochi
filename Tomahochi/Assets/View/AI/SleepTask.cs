using AICore;
using UnityEngine;

public class SleepTask : Task
{
	[SerializeField] private Animator _animator;
	[SerializeField] private PetView _petView;
	[SerializeField] private Brain _brain;
	private SleepSpotView _sleepPlace;
	public override void OnBegin()
	{
		base.OnBegin();
		_sleepPlace = Info.Source as SleepSpotView;
		Agent.destination = _sleepPlace.transform.position;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (Agent.velocity == Vector3.zero && Vector2.Distance(_sleepPlace.transform.position, Agent.transform.position) < 0.25f)
		{
			_animator.SetBool(PetView.SLEEPING_PARAMERT_NAME, true);
		}
		if (_petView.PetInfo.Energy == 100)
		{
			_brain.RemoveFactor(new Factor("Sleep", this));
		}
	}

	public override void OnCancel()
	{
		base.OnCancel();
		_animator.SetBool(PetView.SLEEPING_PARAMERT_NAME, false);
	}
}
