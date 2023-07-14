using Pets;
using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetLoader : MonoBehaviour
{
	[SerializeField] private Grid _buildGrid;
	[SerializeField] private Vector2[] _petSpawnPoints;
	[SerializeField] private Vector2 _spawnOffcet = new Vector2(0,-3f);
	private int _spawnIndex = 0;
	private void Start()
	{
		foreach (PetSaveInfo petInfo in PlayerDataContainer.UnlockedPets)
		{
			AddPet(petInfo);
		}
	}

	private void GetSpawnPoints()
	{
		Vector2Int[] roomCoords = PlayerDataContainer.GetRooms();
		List<Vector2> result = new(); ;
		for (int i = 0; i < roomCoords.Length; i++)
		{
			Vector2 spawnPosition = _buildGrid.CellToWorld((Vector3Int)roomCoords[i]) - _buildGrid.cellSize/2;
			result.Add(spawnPosition);
		}
		_petSpawnPoints = result.ToArray();
	}

	private void OnEnable()
	{
		PlayerDataContainer.UnlockedNewPet.AddListener(AddPet);
	}

	private void OnDisable()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			PlayerDataContainer.UnlockedNewPet.RemoveListener(AddPet);
		}
	}

	private void AddPet(PetSaveInfo info)
	{
		GetSpawnPoints();
		GameObject instance = Instantiate(info.Pet.ViewPrefab, transform);
		NavMeshAgent agent = instance.GetComponent<NavMeshAgent>();
		agent.Warp(_petSpawnPoints[_spawnIndex] + _spawnOffcet);
		_spawnIndex++;
		if (_spawnIndex >= _petSpawnPoints.Length)
		{
			_spawnIndex = 0;
		}
	}
}
