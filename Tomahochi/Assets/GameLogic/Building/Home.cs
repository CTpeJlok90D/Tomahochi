using Saving;
using System;
using System.Collections.Generic;
using UnityEngine;
using Range = UnityExtentions.Range;

[Serializable]
public class Home
{
	[SerializeField] private List<Vector2Int> _rooms = new()
	{
		Vector2Int.zero
	};

	[SerializeField] private Range _xSize = new Range(0,0);
	[SerializeField] private Range _ySize = new Range(0, 6);
	public const int BASE_BUILD_HOME_MORA_COST = 0;
	public const int COST_PER_ROOM_COUNT_RISE = 5000;
	public int BuildHomeCost => BASE_BUILD_HOME_MORA_COST + COST_PER_ROOM_COUNT_RISE * _rooms.Count;

	public List<Vector2Int> Rooms => _rooms;
	private Vector2Int[] _nearVectors => new Vector2Int[] { new(0, 1), new(0, -1), new(-1, 0), new(1, 0) };

	public bool HaveNeigbotrn(Vector2Int postion)
	{
		bool result = false;

		foreach (Vector2Int neirRoomCoord in _nearVectors)
		{
			if (_rooms.Contains(postion + neirRoomCoord))
			{
				return true;
			}
		}

		return result;
	}

	public List<Vector2Int> BuildPlaces()
	{
		List<Vector2Int> result = new();
		Vector2Int[] rooms = PlayerDataContainer.GetRooms();
		foreach (Vector2Int buildPlace in rooms)
		{
			foreach (Vector2Int nearVector in _nearVectors)
			{
				Vector2Int buildPosition = nearVector + buildPlace;
				if (CanBuildHere(buildPosition))
				{
					result.Add(buildPosition);
				}
			}
		}
		return result;
	}

	public bool InAcceptebleDiapason(Vector2Int position)
	{
		return _xSize.Contains(position.x) && _ySize.Contains(position.y);
	}

	public bool CanBuildHere(Vector2Int position)
	{
		return HaveNeigbotrn(position) && InAcceptebleDiapason(position) && _rooms.Contains(position) == false;
	}

	public bool AddRoom(Vector2Int position)
	{
		if (CanBuildHere(position) && PlayerDataContainer.MoraCount >= BuildHomeCost)
		{
			PlayerDataContainer.MoraCount -= BuildHomeCost;
			_rooms.Add(position);
			return true;
		}
		return false;
	}
}
