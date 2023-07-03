using AICore;
using Pets;
using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetView : MonoBehaviour
{
	[SerializeField] private GameObject _view;
	[SerializeField] private Transform _target;
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private Animator _animator;
	[SerializeField] private Brain _brain;
	[SerializeField] private Pet _pet;
	[SerializeField] private Vector3 _rightLookScale = new Vector3(-1, 1, 1);
	[SerializeField] private Vector3 _leftLookScale = new Vector3(1, 1, 1);

	private static Dictionary<string, PetView> _petViewByName = new();

	private PetSaveInfo _petInfo;

	public const string WALK_PARAMETR_NAME = "IsWalking";
	public const string SLEEPING_PARAMERT_NAME = "IsSleeping";

	private bool InMoving => _agent.velocity != Vector3.zero;

	public Pet Pet => _pet;
	public PetSaveInfo PetInfo => _petInfo;

	public static PetView GetPetViewByInfo(PetSaveInfo info)
	{
		return _petViewByName[info.Pet.name];
	}

	public void LaySleep(SleepSpotView sleepSpot)
	{
		Factor factor = new("Sleep", sleepSpot);
		_brain.AddFactor(factor);
	}

	private void Start()
	{
		_petViewByName.Clear();
		_petInfo = PlayerDataContainer.PetInfoBySystemName(_pet.name);
		_petViewByName.Add(PetInfo.Pet.name, this);

		if (_petInfo.IsSleeping())
		{
			SleepSpotView spot = FurnitureView.ByID[_petInfo.SleepingBedID].GetComponent<SleepSpotView>();
			_agent.Warp(spot.SleepTransform.position);
			LaySleep(spot);
		}
	}

	private void LateUpdate()
	{
		_animator.SetBool(WALK_PARAMETR_NAME, InMoving);
		_animator.SetBool(SLEEPING_PARAMERT_NAME, InMoving == false && _petInfo.IsSleeping());

		if (InMoving)
		{
			RotateWithMove();
		}
	}

	private void RotateWithMove()
	{
		float direction = _agent.velocity.x;
		if (direction > 0)
		{
			_view.transform.localScale = _leftLookScale;
		}
		else if (direction < 0)
		{
			_view.transform.localScale = _rightLookScale;
		}
	}
}
