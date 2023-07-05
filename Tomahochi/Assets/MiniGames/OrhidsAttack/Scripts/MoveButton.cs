using OrhidsAttack;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Mob _player;
	[SerializeField] private int _moveDirection;

	public void OnPointerDown(PointerEventData eventData)
	{
		_player.Input += _moveDirection;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_player.Input -= _moveDirection;
	}
}
