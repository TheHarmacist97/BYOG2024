using QTEs.SoundDesignQTE;
using UnityEngine;
public class SoundQTEDetector : MonoBehaviour
{
	[SerializeField] private SoundQTE _soundQTE;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("QTE Keys"))
		{
			return;
		}
		SoundQTEKey soundQTEKey = other.GetComponent<SoundQTEKey>();
		_soundQTE.RegisterKey(soundQTEKey);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("QTE Keys"))
		{
			return;
		}
		SoundQTEKey soundQTEKey = other.GetComponent<SoundQTEKey>();
		_soundQTE.UnregisterKey(soundQTEKey);
	}
}