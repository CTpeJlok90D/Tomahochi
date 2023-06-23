using AICore;
using Pets;
using UnityEngine;

public class SleepTask : Task
{
	[SerializeField] private PetView _petView;
	[SerializeField] private Brain _brain;
	[SerializeField] private SleepSpotView _sleepPlace;
	public override void OnBegin()
	{
		base.OnBegin();
		_sleepPlace = Info.Source as SleepSpotView;
		Agent.updatePosition = false;
		Agent.Warp(_sleepPlace.SleepTransform.position);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		

		if (_petView.PetInfo.IsSleeping() == false)
		{
			_brain.RemoveFactor(new Factor("Sleep", this));
		}
	}

	public override void OnCancel()
	{
		base.OnCancel();
		Agent.updatePosition = true;
	}
}
