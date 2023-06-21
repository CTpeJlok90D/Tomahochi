using UnityEngine;

namespace AICore
{
	public class FactorContainer : MonoBehaviour
	{
		[SerializeField] private Factor _info;

		public Factor Info => _info;
	}
}
