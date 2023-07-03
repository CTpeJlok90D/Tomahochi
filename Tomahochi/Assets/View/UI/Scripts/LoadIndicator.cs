
using UnityEngine;

public class LoadIndicator : MonoBehaviour
{
	[SerializeField] private float _rotateSpeed;

	private void Update()
	{
		transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
	}
}
