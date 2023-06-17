using UnityEngine;

namespace AICore
{
	public class FactorContainer : MonoBehaviour
	{
		[SerializeField] private FactorInfo _info;

		public FactorInfo Info => _info;
	}
}
