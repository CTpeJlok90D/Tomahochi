using NavMeshPlus.Components;
using Saving;
using System.Collections.Generic;
using UnityEngine;

public class HomeView : MonoBehaviour
{
	[SerializeField] private RoomView _roomPrefab;
	[SerializeField] private BuildPlace _buildPlacePrefab;
	[SerializeField] private Grid _buildGrid;
	[SerializeField] private Transform _roomsContainer;
	[SerializeField] private NavMeshSurface _surface;
	[SerializeField] private Vector2[] _colliderPoints;
	[SerializeField] private UIMode _editMode = UIMode.EditRoomMode;

	private List<BuildPlace> _buildPlacesInstances = new();

	private Vector3 PositionOnGrid(Vector2Int position) => _buildGrid.CellToWorld((Vector3Int)position) - _buildGrid.cellSize / 2;

	private void Awake()
	{
		PlaceRooms();
	}

	private void Start()
	{
		_surface.BuildNavMesh();
	}

	private void OnEnable()
	{
		UI.ModeChanged += OnUIModeChanged;
	}

	private void OnDisable()
	{
		UI.ModeChanged -= OnUIModeChanged;
	}

	public void OnUIModeChanged(UIMode mode)
	{
		if (mode == _editMode)
		{
			PlaceBuildPlaces();
			return;
		}
		ClearBuildPlaces();
	}

	private void PlaceRooms()
	{
		Vector2Int[] rooms = PlayerDataContainer.GetRooms();
		foreach (Vector2Int room in rooms)
		{
			AddRoomWitoutMeshUpdate(room);
		}
	}

	private void PlaceBuildPlaces()
	{
		ClearBuildPlaces();
		Vector2Int[] buildPlaces = PlayerDataContainer.GetBuildPlaces();
		foreach (Vector2Int buildPosition in buildPlaces)
		{
			BuildPlace instance = Instantiate(_buildPlacePrefab).Init(buildPosition);
			instance.gameObject.transform.position = PositionOnGrid(buildPosition);
			_buildPlacesInstances.Add(instance);
		}
	}

	private void ClearBuildPlaces()
	{
		foreach (BuildPlace place in _buildPlacesInstances)
		{
			if (place != null)
			{
				Destroy(place.gameObject);
			}
		}
		_buildPlacesInstances.Clear();
	}

	private void AddRoomWitoutMeshUpdate(Vector2Int roomPosition)
	{
		RoomView view = Instantiate(_roomPrefab, _roomsContainer);
		view.transform.position = PositionOnGrid(roomPosition);
		AddCameraCollider(view.transform.position);
		PlayerDataContainer.AddRoom(roomPosition);
	}
	public void AddRoom(Vector2Int roomPosition)
	{
		AddRoomWitoutMeshUpdate(roomPosition);
		_surface.BuildNavMesh();
	}

	public void AddCameraCollider(Vector2 roomPostion)
	{
		List<Vector2> path = new();
		foreach (Vector2 point in _colliderPoints)
		{
			path.Add(point + roomPostion);
		}
	}
}
