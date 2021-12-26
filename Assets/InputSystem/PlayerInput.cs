using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

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
		public bool Jump { get { return _jump; } }
		public bool Sprint { get { return _sprint; } }
		public bool Aim { get { return _aim; } }
		public bool Shoot { get { return _shoot; } set { _shoot = value; } }
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

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(_cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}