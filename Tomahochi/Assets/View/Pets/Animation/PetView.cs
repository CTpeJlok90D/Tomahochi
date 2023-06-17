using Pets;
using Saving;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PetView : MonoBehaviour
{
	[SerializeField] private GameObject _view;
	[SerializeField] private Transform _target;
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private Animator _animator;
	[SerializeField] private string _linkedPetSystemName = "<System pet name>";
	[SerializeField] private Vector3 _rightLookScale = new Vector3(-1, 1, 1);
	[SerializeField] private Vector3 _leftLookScale = new Vector3(1, 1, 1);
	[Header("UI")]
	[SerializeField] private TMP_Text _gemsCaption;
	[SerializeField] private TMP_Text _moraCaption;
	[SerializeField] private TMP_Text _nameCaption;
	[SerializeField] private TMP_Text _levelCaption;
	[SerializeField] private Image _joyImage;
	[SerializeField] private Image _foodImage;
	[SerializeField] private Image _waterImage;
	[SerializeField] private Image _sleepImage;

	private string _gemsCaptionFormat;
	private string _moraCaptionFormat;
	private string _nameCaptionFormat;
	private string _levelCaptionFormat;

	private const string WALK_PARAMETR_NAME = "IsWalking";

	private bool InMoving => _agent.velocity != Vector3.zero;
	private Pet _pet;
	private PetSaveInfo _petInfo;
	public Pet Pet => _pet;
	public PetSaveInfo PetInfo => _petInfo;
	

	private void Start()
	{
		_gemsCaptionFormat = _gemsCaption.text;
		_moraCaptionFormat = _moraCaption.text;
		_nameCaptionFormat = _nameCaption.text;
		_levelCaptionFormat = _levelCaption.text;

		_petInfo = PlayerDataContainer.PetInfoBySystemName(_linkedPetSystemName);
		_pet = PetInfo.Pet();
		_nameCaption.text = String.Format(_nameCaptionFormat, Pet.ViewName);
	}

	private void LateUpdate()
	{
		_animator.SetBool(WALK_PARAMETR_NAME, InMoving);

		_levelCaption.text = String.Format(_levelCaptionFormat, PetInfo.CurrentLevel % Pet.EvelateEveryLevel, Pet.EvelateEveryLevel);
		_gemsCaption.text = String.Format(_gemsCaptionFormat, (int)PetInfo.GemsCount, PetInfo.GemsStorage());
		_moraCaption.text = String.Format(_moraCaptionFormat, (int)PetInfo.MoraCount, PetInfo.MoraStorage());

		_joyImage.fillAmount = PetInfo.Joy /100;
		_foodImage.fillAmount = PetInfo.Food / 100;
		_waterImage.fillAmount = PetInfo.Water / 100;
		_sleepImage.fillAmount = PetInfo.Sleep / 100;

		if (InMoving)
		{
			RotateWithMove();
		}

	}

	private void RotateWithMove()
	{
		float direction = _agent.transform.position.x - _target.transform.position.x;
		if (direction < 0)
		{
			_view.transform.localScale = _leftLookScale;
		}
		else if (direction > 0)
		{
			_view.transform.localScale = _rightLookScale;
		}
	}
}
