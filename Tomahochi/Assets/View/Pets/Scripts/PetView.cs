using AICore;
using Pets;
using Saving;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
		_petInfo.SleepingBedID = sleepSpot.FurnitureView.Source.ID;
		_brain.AddFactor(factor);
	}

	private void Awake()
	{
		_petViewByName.Clear();
	}

	private void Start()
	{
		_petInfo = PlayerDataContainer.PetInfoBySystemName(_pet.name);
		_petViewByName.Add(PetInfo.Pet.name, this);

		if (_petInfo.IsSleeping())
		{
			FurnitureView view = FurnitureView.ByID[_petInfo.SleepingBedID];
			view.MovedOnStorage += OnFurnutureDestroy;
			SleepSpotView spot = view.GetComponent<SleepSpotView>();
			_agent.Warp(new Vector3(spot.SleepTransform.position.x, spot.SleepTransform.position.y, _agent.transform.position.z));
			LaySleep(spot);
		}

		_agent.updateRotation = false;
		_view.transform.localEulerAngles = new Vector3(90,0,0);
	}

	private void OnFurnutureDestroy()
	{
		_petInfo.SleepingBedID = string.Empty;
	}

	private void LateUpdate()
	{
		_animator.SetBool(WALK_PARAMETR_NAME, InMoving);
		_animator.SetBool(SLEEPING_PARAMERT_NAME, _petInfo.IsSleeping());

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
