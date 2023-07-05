using PingPong;
using UnityEngine;

namespace PingPong
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField] private MiniGame _miniGame;
		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.TryGetComponent(out Ball ball))
			{
				ball.Remove();
				_miniGame.Score++;
			}
		}
	}
}