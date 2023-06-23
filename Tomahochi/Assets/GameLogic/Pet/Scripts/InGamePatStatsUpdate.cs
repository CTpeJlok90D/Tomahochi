using UnityEditor;
using UnityEngine;
using Saving;
using System;

namespace Pets
{
	public class InGamePatStatsUpdate : MonoBehaviour
	{
		private DateTime _lastRegistredDateTime = DateTime.MinValue;
		private void Update()
		{
			if (_lastRegistredDateTime == DateTime.MinValue)
			{
				_lastRegistredDateTime = DateTime.UtcNow;
			}
			Pet.FallRatePetsByTime(PlayerDataContainer.UnlockedPets, (float)(DateTime.UtcNow - _lastRegistredDateTime).TotalSeconds);
			_lastRegistredDateTime = DateTime.UtcNow;
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