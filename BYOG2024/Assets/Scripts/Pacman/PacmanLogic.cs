using System;
using UnityEngine;
namespace Pacman
{
	public class PacmanLogic : MonoBehaviour
	{
		private enum MoveDir
		{
			None,
			Vertical,
			Horizontal,
		}

		[SerializeField] private bool _isAlive = true;
		[SerializeField] private float _wallCheckOffset; 
		[SerializeField] private LayerMask _wallLayer;
		[SerializeField] private float _speed = 5f;
		[SerializeField] private MoveDir _currentMoveDir = MoveDir.None;
		[SerializeField] private Collider2D _collider;
		private bool _xInput, _xPos, _xNeg;
		private bool _yInput, _yPos, _yNeg;
		
		public SpriteRenderer _spriteRenderer;
	
		private Vector2 _dir;
		private void Awake()
		{
			_isAlive = true;
		}
		// Update is called once per frame
		private void Update()
		{
			if(!_isAlive) return;
			
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
				_spriteRenderer.flipX = _xNeg;
				transform.rotation = Quaternion.identity;
			}
			else if (_yInput)
			{
				_currentMoveDir = MoveDir.Vertical;
				_dir = new Vector2(0f, _yNeg ? -1f : 1f);
				_spriteRenderer.flipX = false;
				transform.rotation = Quaternion.Euler(0,0, _yNeg ? -90f : 90f);
			}
			
			if(_currentMoveDir != MoveDir.None)
				transform.Translate(_dir * (_speed * Time.deltaTime), Space.World);
		}
		
		private void OnCollisionEnter2D(Collision2D other)
		{
			OnCollisionWithWall(other);
			OnCollisionWithGhost(other);
		}
		private void OnCollisionWithGhost(Collision2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Ghost"))
			{
				return;
			}
			_isAlive = false;
		}

		private void OnCollisionWithWall(Collision2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Walls"))
			{
				return;
			}
			foreach (ContactPoint2D primary in other.contacts)
			{
				Vector3 contactPoint = primary.point;
				Vector2 diff = contactPoint - transform.position;
				Vector2 diffNormal = diff.normalized;
				Debug.Log(diffNormal+" "+diff+" "+contactPoint+ " "+ transform.position);
				if (_currentMoveDir == MoveDir.Horizontal && Mathf.Abs(diffNormal.x)>0.6f)
				{
					_currentMoveDir = MoveDir.None;
					break;
				}
				if (_currentMoveDir == MoveDir.Vertical && Mathf.Abs(diffNormal.y)>0.6f)
				{
					_currentMoveDir = MoveDir.None;
					break;
				}
			}
		}
		public void KillSelf()
		{
			_isAlive = false;
		}
	}
}