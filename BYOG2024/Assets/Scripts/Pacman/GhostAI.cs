using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Pacman
{
	[Flags]
	public enum AvailablePaths
	{
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8,
	}
	public class GhostAI : MonoBehaviour
	{
		public SpriteRenderer _spriteRenderer;
		public bool _gameOver = false;
		[SerializeField] private Tilemap _tilemap;
		[SerializeField] private float _speed;
		[SerializeField] private float _junctionEvaluationPeriod;

		[SerializeField] private AvailablePaths _availableDirEnum;
		[SerializeField] private AvailablePaths _lastAvailableDirEnum;

		[SerializeField] private List<Vector3Int> _availableDirections;
		[SerializeField] private Vector3Int _pickedDir;
		[SerializeField] private Vector3 _nextPosition;
		[SerializeField] private Vector3 _currentPosition;
		[SerializeField] private float _time;
		[SerializeField] private float _timeToNextJunction;
		[SerializeField] private bool _evaluateNextJunction;
		// Start is called before the first frame update
		private void Start()
		{
			_availableDirections = new List<Vector3Int>();
			transform.position = _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(transform.position));
			_timeToNextJunction = _junctionEvaluationPeriod;
			GetEmptyDir(transform.position);
			PickRandomDir();
			_nextPosition = GetNextCellPos();
		}

		private void Update()
		{
			if(_gameOver)
				return;
			
			_time += Time.deltaTime * _speed;
			if (_timeToNextJunction > 0)
			{
				_timeToNextJunction -= Time.deltaTime;
			}
			else
			{
				_timeToNextJunction = 0;
				_evaluateNextJunction = true;
			}
			transform.position = Vector3.Lerp(_currentPosition, _nextPosition, _time);
			

			if (_time < 1)
			{
				return;
			}

			GetEmptyDir(transform.position);
			if (Collision())
			{
				PickRandomDir();
			}
			else if (_evaluateNextJunction)
			{
				if (_lastAvailableDirEnum != _availableDirEnum)
				{
					PickRandomDir();
					_evaluateNextJunction = false;
					_timeToNextJunction = _junctionEvaluationPeriod;
				}
			}
			_lastAvailableDirEnum = _availableDirEnum;

			_currentPosition = transform.position;
			_nextPosition = GetNextCellPos();
			_time = 0;
		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
			{
				PacmanManager.Instance.PacmanDeath();
			}
		}

		private bool Collision()
		{
			if (_pickedDir == Vector3Int.up && !_availableDirEnum.HasFlag(AvailablePaths.Up))
			{
				return true;
			}
			if (_pickedDir == Vector3Int.down && !_availableDirEnum.HasFlag(AvailablePaths.Down))
			{
				return true;
			}
			if (_pickedDir == Vector3Int.left && !_availableDirEnum.HasFlag(AvailablePaths.Left))
			{
				return true;
			}
			if (_pickedDir == Vector3Int.right && !_availableDirEnum.HasFlag(AvailablePaths.Right))
			{
				return true;
			}
			return false;
		}
		private void PickRandomDir()
		{
			_currentPosition = transform.position;
			_pickedDir = _availableDirections[Random.Range(0, _availableDirections.Count)];
		}

		private Vector3 GetNextCellPos()
		{
			return _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(_currentPosition) + _pickedDir);
		}

		private void GetEmptyDir(Vector3 position)
		{
			_availableDirections.Clear();
			Vector3Int index = _tilemap.WorldToCell(position);

			if (!_tilemap.HasTile(index + Vector3Int.up))
			{
				_availableDirEnum |= AvailablePaths.Up;
				_availableDirections.Add(Vector3Int.up);
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Up;
			}

			if (!_tilemap.HasTile(index + Vector3Int.down))
			{
				_availableDirEnum |= AvailablePaths.Down;
				_availableDirections.Add(Vector3Int.down);
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Down;
			}

			if (!_tilemap.HasTile(index + Vector3Int.left))
			{
				_availableDirEnum |= AvailablePaths.Left;
				_availableDirections.Add(Vector3Int.left);
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Left;
			}

			if (!_tilemap.HasTile(index + Vector3Int.right))
			{
				_availableDirEnum |= AvailablePaths.Right;
				_availableDirections.Add(Vector3Int.right);
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Right;
			}

		}
	}
}