using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace OrhidsAttack
{
	[AddComponentMenu("Mini-games/Orhids Attack/Player")]
	public class Mob : MonoBehaviour, IMiniGamePlayer
	{
		[SerializeField] private SplineContainer _moveSpline;
		[SerializeField] private float _moveSpeed = 0.1f;
		[Range(-1,1)]
		[SerializeField] private int _input = 0;

		private float _linePosition = 0.5f;

		public int Input
		{
			get
			{
				return _input;
			}
			set
			{
				_input = Mathf.Clamp(value, -1, 1);
			}
		}

		private void Update()
		{
			float offcet = _input * _moveSpeed * Time.deltaTime;
			_linePosition = Mathf.Clamp(_linePosition + offcet, 0, 1);

			_moveSpline.Spline.Evaluate(_linePosition, out float3 position, out float3 tangent, out float3 upVector);
			transform.position = position;
		}	
	}
}