using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Pacman
{
	public class PelletUtil : MonoBehaviour
	{
		[SerializeField] private Pellet _pelletPrefab;
		[SerializeField] private Transform _pelletParent;
		[SerializeField] private Tilemap _tileMap;
		[SerializeField] private int _x, _y;
		[SerializeField] private List<Pellet> _pellets; 
		// Start is called before the first frame update

		[ContextMenu("Generate Pellets")]
		public void GeneratePellets()
		{
			List<GameObject> pelletsToDestroy = (from Transform t in _pelletParent select t.gameObject)
				.ToList();
			foreach (GameObject pellet in pelletsToDestroy)
			{
				DestroyImmediate(pellet);
			}
			for (int i = 0; i < _x; i++)
			{
				for (int j = 0; j < _y; j++)
				{
					Vector3Int index = new(i - (int)(_x * 0.5f), j - (int)(_y * 0.5f), 0);
					if (!_tileMap.HasTile(index))
					{
						Instantiate(_pelletPrefab, _pelletParent).transform.position =
							_tileMap.GetCellCenterWorld(index);
					}
				}
			}
		}
		[ContextMenu("Get Pellets")]
		public void GetPelletList()
		{
			_pellets = new List<Pellet>();
			foreach (Transform t in _pelletParent)
			{
				_pellets.Add(t.gameObject.GetComponent<Pellet>());
			}
		}

		public int GetPelletCount()
		{
			return _pellets.Count;
		}
		public void SkinPellets(Sprite sprite)
		{
			foreach (Pellet pellet in _pellets)
			{
				pellet._spriteRenderer.sprite = sprite;
			}
		}
	}
}