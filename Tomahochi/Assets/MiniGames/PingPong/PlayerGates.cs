using UnityEngine;

namespace PingPong
{
	public class PlayerGates : MonoBehaviour
	{
		[SerializeField] private Health _playerHealth;
		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.TryGetComponent(out Ball ball))
			{
				if (ball.DamagePlayer)
				{
					_playerHealth.Value--;
				}
				ball.Remove();
			}
		}
	}
}