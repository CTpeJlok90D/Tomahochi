using UnityEditor;
using UnityEngine;
using Saving;

namespace Pets
{
	public class InGamePatStatsUpdate : MonoBehaviour
	{
		private void Update()
		{
			Pet.FallRatePetsByTime(PlayerDataContainer.PetList, Time.deltaTime);
		}

#if UNITY_EDITOR

		private static InGamePatStatsUpdate _instance;
		[InitializeOnLoadMethod]
		private static void Init()
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<InGamePatStatsUpdate>();
				if (FindObjectsOfType<InGamePatStatsUpdate>().Length == 0)
				{
					GameObject gameObject = new();
					gameObject.name = $"[{nameof(InGamePatStatsUpdate)}]";
					gameObject.AddComponent<InGamePatStatsUpdate>();
					gameObject.transform.SetAsLastSibling();
				}
			}
		}
#endif
	}
}