using System;
using UnityEngine;

[Serializable]
public class Furniture
{
	[SerializeField] private string _id;
	
	public Vector2 Position;
	public string PlaceDate;
	public string SystemName;


	public string ID => _id;

	public Furniture()
	{
		_id = Guid.NewGuid().ToString();
	}

	public Furniture(string id)
	{
		_id = id;
	}
}
