using UnityEngine;

public class NavMeshPoint : MonoBehaviour
{
	[SerializeField] private float _navMeshZPosition = 10;

	private void Update()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, _navMeshZPosition);
	}
}
