using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCameraPanel : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
	[SerializeField] private CinemachineVirtualCamera _camera;
	[SerializeField] private CinemachineBrain _brain;
	[SerializeField] private float _moveSensivity = 0.01f;
	[SerializeField] private Vector2 _cameraScrollSizeBorders;
	[SerializeField] private float _zCameraLevel = -50;
	private Coroutine _moveCoroutine;
	public bool ThisCameraIsActive => (_brain.ActiveVirtualCamera as CinemachineVirtualCamera) == _camera;


	public void OnPointerDown(PointerEventData eventData)
	{
		Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, worldMousePosition);

		if (hit.collider.TryGetComponent(out SelectPetCollision collision))
		{
			return;
		}
		SelectPetCollision.SelectedPet?.Deselect();
		if (eventData.button != PointerEventData.InputButton.Left || ThisCameraIsActive == false)
		{
			return;
		}
		_moveCoroutine = StartCoroutine(CameraMoveCoroutine());
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (_moveCoroutine != null)
		{
			StopCoroutine(_moveCoroutine);
		}
		if (_brain.IsBlending == false)
		{
			_camera.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _zCameraLevel);
		}
	}

	public IEnumerator CameraMoveCoroutine()
	{
		Vector3 lastMousePosition;
		while (true)
		{
			lastMousePosition = Input.mousePosition;
			yield return null;
			_camera.transform.position += (lastMousePosition - Input.mousePosition) * _moveSensivity;
		}
	}

	protected void LateUpdate()
	{
		if (ThisCameraIsActive == false)
		{
			_camera.transform.position = _brain.transform.position;
			return;
		}
		ScrollCamera();
	}

	public void ScrollCamera()
	{
		_camera.m_Lens.OrthographicSize = Mathf.Clamp(_camera.m_Lens.OrthographicSize - Input.mouseScrollDelta.y, _cameraScrollSizeBorders[0], _cameraScrollSizeBorders[1]);
	}
}
