using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PelletUtil : MonoBehaviour
{
	[SerializeField] private Transform _pelletParent;
	[SerializeField] private Tilemap _tileMap;
	[SerializeField] private int _x, _y;
	// Start is called before the first frame update

	public void GetPellets(Pellet pelletPrefab)
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
					Instantiate(pelletPrefab, _pelletParent).transform.position =
						_tileMap.GetCellCenterWorld(index);
				}
			}
		}
	}
}