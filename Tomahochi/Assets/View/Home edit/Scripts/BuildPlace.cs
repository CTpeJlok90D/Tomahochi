using UnityEngine;

public class BuildPlace : MonoBehaviour
{
	[SerializeField] private Vector2Int _positionOnGrid;

	public Vector2Int PositionOnGrid => _positionOnGrid;

	public BuildPlace Init(Vector2Int positionOnGrid)
	{
		_positionOnGrid = positionOnGrid;

		return this;
	}
}