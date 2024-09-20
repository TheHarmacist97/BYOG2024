using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace QTEs.SoundDesignQTE
{
	public class SoundQTE : QuickTimeEvent
	{
		[Header("Audio Sources")]
		[SerializeField] private AudioSource _source;
		
		[Space]
		[Header("Current Time")]
		public float _currentTime;

		[Space]
		[Header("Key Colors")]
		[SerializeField] private Color _wColor;
		[SerializeField] private Color _sColor;
		[SerializeField] private Color _aColor;
		[SerializeField] private Color _dColor;

		[Space]
		[Header("Configs")]
		[SerializeField] private KeypressAccuracyConfig _config;
		[SerializeField] private SoundQTEKey _keyPrefab;
		[SerializeField] private float _keySpeed;
		[FormerlySerializedAs("bpm")]
		[SerializeField] private float _bpm;
		[SerializeField, Range(1, 5)] private int _beatTimeModifier;

		[Header("Spawn Points")]
		[SerializeField] private Transform _wSpawnPoint;
		[SerializeField] private Transform _sSpawnPoint;
		[SerializeField] private Transform _aSpawnPoint;
		[SerializeField] private Transform _dSpawnPoint;
		
		[Header("Detection References")]
		[SerializeField] private Transform _detectionArea;
		[SerializeField] private SpriteRenderer _perfectArea;

		[SerializeField] private float _paddingTime;
		private int _keysSpawned, _keysHit, _keysMissed;
		private List<KeyQueueElement> _keyQueue;
		
		[SerializeField] private List<SoundQTEKey> _spawnedKeyList;
		
		[SerializeField] private float _timeTakenToReachFirstNode;
		private WaitForSeconds _musicDelay;

		private float startTime;
		protected override void OnUpdate()
		{
			if(_keyQueue.Count==0) return;
			_currentTime += Time.deltaTime;
			if (_currentTime > _keyQueue[0]._timeToSpawn)
			{
				KeyQueueElement currentElement = _keyQueue[0];
				_keyQueue.RemoveAt(0);
				
				SoundQTEKey currentQTEkey;
				switch (currentElement._key)
				{
					case QTEKey.Up:
					{
						currentQTEkey = Instantiate(_keyPrefab, _wSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_wColor, _keySpeed);
						break;
					}
					case QTEKey.Down:
					{
						currentQTEkey = Instantiate(_keyPrefab, _sSpawnPoint.position, Quaternion.identity);	
						currentQTEkey.SetUp(_sColor, _keySpeed);
						break;
					}
					case QTEKey.Left:
					{
						currentQTEkey = Instantiate(_keyPrefab, _aSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_aColor, _keySpeed);
						break;
					}
					case QTEKey.Right:
					{	
						currentQTEkey = Instantiate(_keyPrefab, _dSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_dColor, _keySpeed);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}
				
				_spawnedKeyList.Add(currentQTEkey);
			}
		}
		protected override void Initialize()
		{
			_musicDelay = new WaitForSeconds(_timeTakenToReachFirstNode);
			_keyQueue = new List<KeyQueueElement>();

			
			float timeBetweenBeats = _beatTimeModifier*(60f/_bpm);
			totalActionCount = (int)(totalAllowedTime/timeBetweenBeats);
			Debug.Log(timeBetweenBeats);
			
			for (int i = 0; i < totalActionCount; i++)
			{
				float dictKeyTime = timeBetweenBeats * i;
				_keyQueue.Add(new KeyQueueElement(dictKeyTime, (QTEKey)Random.Range(0, 4)));
			}

			foreach (KeyQueueElement kvp in _keyQueue)
			{
				Debug.Log($"{kvp._timeToSpawn} || {kvp._key}");
			}

			StartCoroutine(DelayStartOfAudioSource());

		}
		private IEnumerator DelayStartOfAudioSource()
		{
			yield return _musicDelay;
			_source.Play();
		}
		protected override void OnComplete()
		{
			throw new NotImplementedException();
		}
		
		[ContextMenu("Get Time to Reach First Node")]
		private void GetTimeTakenToReachFirstNode()
		{
			_timeTakenToReachFirstNode = (_wSpawnPoint.position.x - _perfectArea.transform.position.x)/_keySpeed;
		}
	}

	[Serializable]
	public class KeyQueueElement
	{
		public float _timeToSpawn;
		public QTEKey _key;

		public KeyQueueElement(float timeToSpawn, QTEKey key)
		{
			_timeToSpawn = timeToSpawn;
			_key = key;
		}
	}
}