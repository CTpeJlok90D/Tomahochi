using UnityEngine;
using UnityEngine.UI;
// ����� ��� �� �� ���������. ��� ���� ���� ��� ������ xd
public class InWorldHint : MonoBehaviour
{
	public Transform Target; // ����
	public Image PointerUI;
	public RectTransform PointerTransform;

	private Vector3 startPointerSize;
	private Camera mainCamera;


	private void Awake()
	{
		startPointerSize = PointerTransform.sizeDelta;
		mainCamera = Camera.main;
	}
	private void LateUpdate()
	{
		Vector3 realPos = mainCamera.WorldToScreenPoint(Target.position);

		Vector3 outPos = realPos;

		float offset = PointerTransform.sizeDelta.x / 2;
		outPos.x = Mathf.Clamp(outPos.x, offset, Screen.width - offset);
		outPos.y = Mathf.Clamp(outPos.y, offset, Screen.height - offset);

		PointerTransform.sizeDelta = new Vector2(startPointerSize.x / 100, startPointerSize.y / 100);
		PointerTransform.anchoredPosition = outPos;
	}
}