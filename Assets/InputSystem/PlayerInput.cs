using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
	public class PlayerInput : MonoBehaviour
	{
		[Header("Character Input Values")]
		[SerializeField]
		Vector2 _move;
		[SerializeField]
		Vector2 _look;
		[SerializeField]
		bool _jump;
		[SerializeField]
		bool _sprint;
		[SerializeField]
		bool _aim;
		[SerializeField]
		bool _shoot;
		[SerializeField]
		bool _pause;

		[Header("Movement Settings")]
		[SerializeField]
		bool _analogMovement;

		[Header("Mouse Cursor Settings")]
		[SerializeField]
		bool _cursorLocked = true;
		[SerializeField]
		bool _cursorInputForLook = true;

		public Vector2 Move { get { return _move; } }
		public Vector2 Look{ get { return _look; } }
		public bool Jump { get { return _jump; } set { _jump = value; } }
		public bool Sprint { get { return _sprint; } }
		public bool Aim { get { return _aim; } }
		public bool Shoot { get { return _shoot; } set { _shoot = value; } }
		public bool Pause { get { return _pause; } }
		public bool AnalogMovement { get { return _analogMovement; } }

		public void OnMove(InputAction.CallbackContext value)
		{
			MoveInput(value.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext value)
		{
			if(_cursorInputForLook)
			{
				LookInput(value.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext value)
		{
			JumpInput(value.action.triggered);
		}

		public void OnSprint(InputAction.CallbackContext value)
		{
			SprintInput(value.action.ReadValue<float>() == 1);
		}

		public void OnAim(InputAction.CallbackContext value)
		{
			AimInput(value.action.ReadValue<float>() == 1);
		}

		public void OnShoot(InputAction.CallbackContext value)
		{
			ShootInput(value.action.triggered);
		}
		public void OnPause(InputAction.CallbackContext value)
		{
			PauseInput(value.action.triggered);
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			_move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			_look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			_jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			_sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
			_aim = newAimState;
		}

		public void ShootInput(bool newShootState)
		{
			_shoot = newShootState;
		}

		public void PauseInput(bool newPauseState)
		{
			_pause = newPauseState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(_cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}