using UnityEngine;
namespace QTEs.SoundDesignQTE
{
	[CreateAssetMenu(menuName = "QTEs/Keypress Accuracy")]
	public class KeypressAccuracyConfig : ScriptableObject
	{
		public float _minEarlyDistance;
		public float _maxPerfectDistance;
		public float _minLateDistance;
	}
}
