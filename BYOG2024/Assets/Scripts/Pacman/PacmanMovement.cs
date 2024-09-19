using System;
using UnityEngine;
namespace Pacman
{
	public class PacmanMovement : MonoBehaviour
	{
		private enum MoveDir
		{
			None,
			Vertical,
			Horizontal,
		}
	
		[SerializeField] private LayerMask _wallLayer;
		[SerializeField] private float _speed = 5f;
		[SerializeField] private MoveDir _currentMoveDir = MoveDir.None;
		[SerializeField] private Collider2D _collider;
		private bool _xInput, _xPos, _xNeg;
		private bool _yInput, _yPos, _yNeg;
	
		private Vector2 _dir;
		// Update is called once per frame
		private void Update()
		{
			_xNeg = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
			_xPos = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
			_xInput = _xNeg || _xPos;

			_yPos = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
			_yNeg = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
			_yInput = _yNeg || _yPos;

		
			if (_xInput)
			{
				_currentMoveDir = MoveDir.Horizontal;
				_dir = new Vector2(_xNeg ? -1f : 1f, 0f);
			}
			else if (_yInput)
			{
				_currentMoveDir = MoveDir.Vertical;
				_dir = new Vector2(0f, _yNeg ? -1f : 1f);
			}
			
			if(_currentMoveDir != MoveDir.None)
				transform.Translate(_dir * (_speed * Time.deltaTime), Space.World);
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
			{
				Debug.Log(other.gameObject.name);
				_currentMoveDir = MoveDir.None;
			}
		}
	}
}