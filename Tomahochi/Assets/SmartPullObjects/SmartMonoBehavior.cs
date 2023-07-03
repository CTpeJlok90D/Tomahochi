using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartMonoBehaivor : MonoBehaviour
{
	private static Dictionary<Type, List<SmartMonoBehaivor>> _instances = new();

	public static T Create<T>(T Original) where T : SmartMonoBehaivor => Create(Original);
	public static T Create<T>(T Original, Vector3 position, Quaternion rotation) where T : SmartMonoBehaivor => Create(Original, position, rotation);
	public static SmartMonoBehaivor Create(SmartMonoBehaivor original) => Create(original, new Vector3(0,0,0), Quaternion.identity);
	public static SmartMonoBehaivor Create(SmartMonoBehaivor original, Vector3 position, Quaternion rotation)
	{
		Type type = original.GetType();
		if (_instances.ContainsKey(type) == false)
		{
			_instances.Add(type, new());
		}
		List<SmartMonoBehaivor> list = _instances[type];

		SmartMonoBehaivor instance;
		if (list.Count == 0)
		{
			instance = Instantiate(original, position, rotation);
			return instance;
		}

		instance = list[0];
		list.RemoveAt(0);
		SetUpObject(instance, position, rotation);
		return instance;
	}

	private static void SetUpObject(SmartMonoBehaivor instance, Vector3 position, Quaternion rotation)
	{
		instance.transform.position = position;
		instance.transform.rotation = rotation;
		instance.Awake();
		instance.gameObject.SetActive(true);
		instance.Start();
	}

	public static void Remove(SmartMonoBehaivor obj)
	{
		obj.gameObject.SetActive(false);
	}

	private void AddInDictionary()
	{
		Type type = GetType();
		if (_instances.ContainsKey(type) == false)
		{
			_instances.Add(type, new());
		}

		_instances[type].Add(this);
	}

	protected virtual void Awake()
	{
		if (_instances.ContainsKey(GetType()))
		{
			return;
		}
		_instances.Add(GetType(), new());
	}

	protected virtual void OnEnable()
	{
		_instances[this.GetType()].Remove(this);
	}

	protected virtual void Start()
	{

	}

	protected virtual void OnDisable()
	{
		AddInDictionary();
	}

	protected virtual void OnDestroy()
	{
		_instances[this.GetType()].Remove(this);
	}
}
