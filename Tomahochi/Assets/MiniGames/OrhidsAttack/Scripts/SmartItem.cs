using UnityEngine;
using UnityExtentions;

namespace OrhidsAttack
{
	public class SmartItem : SmartEnemy
	{
		[SerializeField] private Random<Storageble> _storageble = new();
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private Range _count = new(1, 3);
		
		private Storageble _currentStorageble;

		public Storageble CurrentStorageble
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
			CurrentStorageble = _storageble;
		}

		public void AddOnStorage()
		{
			_currentStorageble.AddOnStorage(_count);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_storageble.OnValidate();
			_renderer ??= GetComponent<SpriteRenderer>();
		}
#endif
	}
}