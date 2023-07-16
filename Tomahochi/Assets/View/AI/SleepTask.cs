using AICore;
using UnityEngine;

public class SleepTask : Task
{
	[SerializeField] private PetView _petView;
	[SerializeField] private Brain _brain;
	private SleepSpotView _sleepPlace;
	public override void OnBegin()
	{
		base.OnBegin();
		_sleepPlace = Info.Source as SleepSpotView;
		Agent.Warp(new Vector3(_sleepPlace.SleepTransform.position.x, _sleepPlace.SleepTransform.position.y, Agent.transform.position.z));
	}

	public override void OnUpdate()
	{
		base.OnUpdate();


		if (_petView.PetInfo.IsSleeping() == false)
		{
			_brain.RemoveFactor(new Factor("Sleep", this));
		}
	}
}
