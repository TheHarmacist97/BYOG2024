using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class GhostAI : MonoBehaviour
{
	[Flags]
	private enum GhostAvailablePaths
	{
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8,
	}

	[SerializeField]
	private float _blockCheckDistance;
	[SerializeField] private LayerMask _wallLayer;
	[SerializeField] private GhostAvailablePaths _availablePaths;
	private GhostAvailablePaths _lastAvailablePaths;
	[SerializeField] private Vector2 _direction;
	[SerializeField] private float _speed;
	[SerializeField] private Vector2 _randomClockMinMax;
	[SerializeField] private float _timeTillPathingDecision;

	// Start is called before the first frame update
	private void Start()
	{
		CheckForAvailablePaths();
		PickRandomPath();
		_timeTillPathingDecision = Random.Range(_randomClockMinMax.x, _randomClockMinMax.y);
	}


	// Update is called once per frame
	private void Update()
	{
		if (_lastAvailablePaths != _availablePaths)
		{
			PickRandomPath();
			_lastAvailablePaths = _availablePaths;
		}
		
		transform.Translate(_direction * (_speed * Time.deltaTime));
	}

	private void FixedUpdate()
	{
		CheckForAvailablePaths();
	}
	private void PickRandomPath()
	{
		_direction = Vector2.zero;
		float dirRandom = Random.value;

		if (dirRandom > 0.5f)
		{
			float vertRandom = Random.value;
			//pick Vertical
			if (_availablePaths.HasFlag(GhostAvailablePaths.Up)&&vertRandom>0.5f)
			{
				_direction = Vector2.up;
			}
			else if (_availablePaths.HasFlag(GhostAvailablePaths.Down))
			{
				_direction = Vector2.down;
			}
		}
		else
		{
			float horRandom = Random.value;
			if (_availablePaths.HasFlag(GhostAvailablePaths.Left)&&horRandom>0.5f)
			{
				_direction = Vector2.left;
			}
			else if (_availablePaths.HasFlag(GhostAvailablePaths.Right))
			{
				_direction = Vector2.right;
			}
		}
		Debug.Log(_direction);
	}

	private void CheckForAvailablePaths()
	{
		_availablePaths = 0;
		_availablePaths = CheckRay(Vector3.up, _blockCheckDistance) ? 
			_availablePaths & ~GhostAvailablePaths.Up : _availablePaths | GhostAvailablePaths.Up;
		
		_availablePaths = CheckRay(Vector3.down, _blockCheckDistance) ? 
			_availablePaths & ~GhostAvailablePaths.Down : _availablePaths | GhostAvailablePaths.Down;
		
		_availablePaths = CheckRay(Vector3.right, _blockCheckDistance) ? 
			_availablePaths & ~GhostAvailablePaths.Right : _availablePaths | GhostAvailablePaths.Right;
		
		_availablePaths = CheckRay(Vector3.left, _blockCheckDistance) ? 
			_availablePaths & ~GhostAvailablePaths.Left : _availablePaths | GhostAvailablePaths.Left;
	}

	private bool CheckRay(Vector3 dir, float distance)
	{
		return Physics2D.Raycast(transform.position, dir, distance, _wallLayer);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _blockCheckDistance);
		Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _blockCheckDistance);
		Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _blockCheckDistance);
		Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _blockCheckDistance);
	}
}