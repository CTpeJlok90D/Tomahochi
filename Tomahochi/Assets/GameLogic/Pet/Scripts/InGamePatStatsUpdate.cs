using UnityEditor;
using UnityEngine;
using Saving;

namespace Pets
{
	public class InGamePatStatsUpdate : MonoBehaviour
	{
		private void Update()
		{
			Pet.FallRatePetsByTime(PlayerDataContainer.UnlockedPets, Time.deltaTime);
		}
#if UNITY_EDITOR
		[SerializeField] private float _seconds;
		[CustomEditor(typeof(InGamePatStatsUpdate))]
		private class InGamePatStatsUpdateEditor : Editor
		{
			private new InGamePatStatsUpdate target => base.target as InGamePatStatsUpdate;

			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();
				if (GUILayout.Button("Simulate seconds"))
				{
					Pet.FallRatePetsByTime(PlayerDataContainer.UnlockedPets, target._seconds);
				}
			}
		}
#endif
	}
}