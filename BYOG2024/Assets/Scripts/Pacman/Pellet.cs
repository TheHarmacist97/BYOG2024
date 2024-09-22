using System;
using UnityEngine;
namespace Pacman
{
	[Serializable]
	public class Pellet : MonoBehaviour
	{
		public SpriteRenderer _spriteRenderer;
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Pacman"))
			{
				return;
			}
			PacmanManager.Instance.PelletEaten();
			Destroy(gameObject);
		}
	}
}