using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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
		[Header("Feedback Colors")]
		[SerializeField] private Color _correctColor;
		[SerializeField] private Color _wrongColor;
		[SerializeField] private Color _normalColor;

		[Space]
		[Header("Configs")]
		[SerializeField] private float _clipLength;
		[SerializeField] private float _maxPerfectDistance;
		[SerializeField] private SoundQTEKey _keyPrefab;
		[SerializeField] private float _keySpeed;
		[SerializeField] private float _bpm;
		[SerializeField] [Range(1, 5)] private int _beatTimeModifier;

		[Header("Spawn Points")]
		[SerializeField] private Transform _wSpawnPoint;
		[SerializeField] private Transform _sSpawnPoint;
		[SerializeField] private Transform _aSpawnPoint;
		[SerializeField] private Transform _dSpawnPoint;
		[SerializeField] private Transform _keyHolder;

		[Header("Detection References")]
		[SerializeField] private Transform _detectionArea;
		[SerializeField] private SpriteRenderer _perfectArea;

		[SerializeField] private float _paddingTime;

		[SerializeField] private List<SoundQTEKey> _spawnedKeyList;
		[SerializeField] private List<SoundQTEKey> _triggerableKeys;
		
		[SerializeField] private float _timeTakenToReachFirstNode;
		private List<KeyQueueElement> _keyQueue;
		private WaitForSeconds _musicDelay;
		
		protected override void OnUpdate()
		{
			SpawnKeys();
			GetInputForKeys();
		}
		private void SpawnKeys()
		{

			if (_keyQueue.Count == 0)
			{
				return;
			}
			_currentTime += Time.deltaTime;
			if (_currentTime > _keyQueue[0]._timeToSpawn)
			{
				KeyQueueElement currentElement = _keyQueue[0];
				_keyQueue.RemoveAt(0);

				SoundQTEKey currentQTEkey;
				switch (currentElement._key)
				{
					case QTEKey.W:
					{
						currentQTEkey = Instantiate(_keyPrefab, _wSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_wColor, _keySpeed, currentElement._key);
						break;
					}
					case QTEKey.S:
					{
						currentQTEkey = Instantiate(_keyPrefab, _sSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_sColor, _keySpeed, currentElement._key);
						break;
					}
					case QTEKey.A:
					{
						currentQTEkey = Instantiate(_keyPrefab, _aSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_aColor, _keySpeed, currentElement._key);
						break;
					}
					case QTEKey.D:
					{
						currentQTEkey = Instantiate(_keyPrefab, _dSpawnPoint.position, Quaternion.identity);
						currentQTEkey.SetUp(_dColor, _keySpeed, currentElement._key);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}
				currentQTEkey.transform.SetParent(_keyHolder, true);
				_spawnedKeyList.Add(currentQTEkey);
			}
			return;
		}
		private void GetInputForKeys()
		{
			if (_triggerableKeys.Count == 0)
			{
				return;
			}

			if (Input.GetKeyDown(KeyCode.W))
			{
				AcceptKeyPress(QTEKey.W);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				AcceptKeyPress(QTEKey.S);
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				AcceptKeyPress(QTEKey.A);
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				AcceptKeyPress(QTEKey.D);
			}
		}

		private void AcceptKeyPress(QTEKey keyDir)
		{
			SoundQTEKey currentQTEkey = _triggerableKeys[0];
			bool success = false;
			float distanceFromPerfect = Mathf.Abs(currentQTEkey.transform.position.x - _perfectArea.transform.position.x);
			if (currentQTEkey.TargetKey == keyDir)
			{
				if (distanceFromPerfect <= _maxPerfectDistance)
				{
					//Accurate hit, increase correct counter
					//also give feedback
					IncrementSuccessAction();
					
					success = true;
				}
				else
				{
					IncrementFailedAction();
				}
			}
			else
			{
				IncrementFailedAction();
			}
			Sequence bloomSequence = DOTween.Sequence();
			if (success)
			{
				bloomSequence.Append( _perfectArea.DOColor(_correctColor, 0.2f));	
			}
			else
			{
				bloomSequence.Append( _perfectArea.DOColor(_wrongColor, 0.2f));
			}
			bloomSequence.Append( _perfectArea.DOColor(_normalColor, 0.2f));
			Debug.Log(success? "Successful ": "Failed " + "Keypress" + distanceFromPerfect);
			
			if(_triggerableKeys.Count > 0)
				_triggerableKeys.RemoveAt(0);
			
			currentQTEkey.KillSelf(success);
		}

		protected override void Initialize()
		{
			_musicDelay = new WaitForSeconds(_timeTakenToReachFirstNode);
			_keyQueue = new List<KeyQueueElement>();
			_triggerableKeys = new List<SoundQTEKey>();


			float timeBetweenBeats = _beatTimeModifier * (60f / _bpm);
			totalActionCount = (int)(_clipLength / timeBetweenBeats);
			for (int i = 0; i < totalActionCount; i++)
			{
				float dictKeyTime = timeBetweenBeats * i;
				_keyQueue.Add(new KeyQueueElement(dictKeyTime, (QTEKey)Random.Range(0, 4)));
			}
			
			StartCoroutine(DelayAudioSourceStart());

		}
		private IEnumerator DelayAudioSourceStart()
		{
			yield return _musicDelay;
			_source.Play();
		}
		protected override void OnComplete()
		{
			ResetThisQTE();
		}
		private void ResetThisQTE()
		{
			_currentTime = 0f;
			_spawnedKeyList.Clear();
			_triggerableKeys.Clear();
		}

		[ContextMenu("We have this")]
		private void ResetQTE()
		{
			StartQTE();
		}

		[ContextMenu("Get Time to Reach First Node")]
		private void GetTimeTakenToReachFirstNode()
		{
			_timeTakenToReachFirstNode = (_wSpawnPoint.position.x - _perfectArea.transform.position.x) / _keySpeed;
		}

		public void RegisterKey(SoundQTEKey key)
		{
			_triggerableKeys.Add(key);
		}
		public void UnregisterKey(SoundQTEKey key)
		{
			if (_triggerableKeys.Remove(key))
			{
				IncrementFailedAction();
			}
			key.KillSelf(false);
		}
	}

	[Serializable]
	public struct KeyQueueElement
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