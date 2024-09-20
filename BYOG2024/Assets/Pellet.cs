using UnityEngine;
public class Pellet : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.gameObject.name);
		if (other.gameObject.layer != LayerMask.NameToLayer("Pacman"))
		{
			return;
		}
		Destroy(gameObject);
	}
}