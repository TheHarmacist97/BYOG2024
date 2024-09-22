using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pacman
{
	public class PacmanLogic : MonoBehaviour
	{
		public bool _isAlive = true;
		[SerializeField] private Tilemap _tilemap;
		[SerializeField] private Vector3Int _pickedDir;
		[SerializeField] private AvailablePaths _availableDirEnum;
		[SerializeField] private float _speed = 5f;
		[SerializeField] private Collider2D _collider;
		[SerializeField] private float _time;
		[SerializeField] private Vector3 _currentPosition, _nextPosition;

		public SpriteRenderer _spriteRenderer;

		private Vector2 _dir;
		private bool _xInput, _xPos, _xNeg;
		private bool _yInput, _yPos, _yNeg;
		private void Awake()
		{
			_isAlive = true;
		}

		private void Start()
		{
			transform.position = _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(transform.position));
			_currentPosition = transform.position;
			_nextPosition = transform.position;
		}

		// Update is called once per frame
		private void Update()
		{
			if (!_isAlive)
			{
				return;
			}

			GetInput();
			
			_time += Time.deltaTime * _speed;
			transform.position = Vector3.Lerp(_currentPosition, _nextPosition, _time);

			if (_time < 1)
				return;
			
			GetEmptyDir(transform.position);
				
			if (Collision())
			{
				_pickedDir = Vector3Int.zero;
			}
				
			_currentPosition = _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(transform.position));
			_nextPosition = GetNextCellPos();
			MovementBasedVisuals();
			_time = 0;
		}
		private void MovementBasedVisuals()
		{
			if (_pickedDir == Vector3Int.up)
			{
				_spriteRenderer.flipX = false;
				transform.rotation = Quaternion.Euler(0,0,90);
			}
			if (_pickedDir == Vector3Int.down)
			{
				_spriteRenderer.flipX = false;
				transform.rotation = Quaternion.Euler(0,0,-90);
			}
			if (_pickedDir == Vector3Int.left)
			{
				_spriteRenderer.flipX = true;
				transform.rotation = Quaternion.identity;
			}
			if (_pickedDir == Vector3Int.right)
			{
				_spriteRenderer.flipX = false;
				transform.rotation = Quaternion.identity;
			}
		}
		
		private Vector3 GetNextCellPos()
		{
			return _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(_currentPosition) + _pickedDir);
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			OnCollisionWithGhost(other);
		}
		private void GetInput()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				_pickedDir = Vector3Int.left;
				
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				_pickedDir = Vector3Int.right;
				
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				_pickedDir = Vector3Int.down;
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				_pickedDir = Vector3Int.up;
				
			}
		}
		private void OnCollisionWithGhost(Collision2D other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Ghost"))
			{
				return;
			}
			_isAlive = false;
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

		private void GetEmptyDir(Vector3 position)
		{
			Vector3Int index = _tilemap.WorldToCell(position);

			if (!_tilemap.HasTile(index + Vector3Int.up))
			{
				_availableDirEnum |= AvailablePaths.Up;
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Up;
			}

			if (!_tilemap.HasTile(index + Vector3Int.down))
			{
				_availableDirEnum |= AvailablePaths.Down;
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Down;
			}

			if (!_tilemap.HasTile(index + Vector3Int.left))
			{
				_availableDirEnum |= AvailablePaths.Left;
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Left;
			}

			if (!_tilemap.HasTile(index + Vector3Int.right))
			{
				_availableDirEnum |= AvailablePaths.Right;
			}
			else
			{
				_availableDirEnum &= ~AvailablePaths.Right;
			}

		}

		public void KillSelf()
		{
			_isAlive = false;
		}
		
	}
}