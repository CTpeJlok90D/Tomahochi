using UnityEditor.Timeline;
using UnityEngine;
using UnityExtentions;

namespace PingPong
{
	public class ItemBall : Ball
	{
		[SerializeField] private Random<Storageble> _storageble = new();
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private Range _count = new(1,3);

		private Storageble _currentStorageble;

		public Storageble CurrentStorajeble
		{
			get
			{
				return _currentStorageble;
			}
			set
			{
				_currentStorageble = value;
				_renderer.sprite = _currentStorageble.ViewSprite;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			CurrentStorajeble = _storageble;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.TryGetComponent(out IMiniGamePlayer player) == false)
			{
				return;
			}

			_currentStorageble.AddOnStorage(_count);
			Remove();
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			_storageble.OnValidate();
			_renderer ??= GetComponent<SpriteRenderer>();
		}
#endif
	}
}
