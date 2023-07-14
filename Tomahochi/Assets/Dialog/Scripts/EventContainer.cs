using UnityEngine;
using UnityEngine.Events;

public abstract class EventContainer : MonoBehaviour
{
	public abstract UnityEvent Event { get; }
}
