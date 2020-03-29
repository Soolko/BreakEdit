using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics.CodeAnalysis;
using static BreakEdit.InputStrings;

namespace BreakEdit
{
	[AddComponentMenu("BreakEdit/Camera Controller")]
	public class CameraController : MonoBehaviour
	{
		// Settings
		[Header("Movement Settings")]
		public float beginningSpeed = 4.0f;
		public float sprintMultiplier = 2.0f;
		public float acceleration = 1.1f;

		[Header("Camera Look Settings")]
		[Range(0.01f, 4.0f)] public float sensitivity = 1.0f;
		[Range(0.1f, 4.0f)]  public float keySensitivity = 2.0f;
		
		// Input System
		#pragma warning disable 0649
		
		[Header("Input System"), SerializeField]
		private InputActionAsset inputAsset;
		private InputAction movementAction;
		private InputAction panAction;
		private InputAction lookAction, lookAlwaysAction;
		private InputAction sprintAction;
		
		#pragma warning restore 0649
		
		private void OnMovementPress(InputAction.CallbackContext context) => buttonHeld = true;
		
		private void OnEnable()
		{
			movementAction.Enable();
			movementAction.started += OnMovementPress;
			sprintAction.Enable();
			
			panAction.Enable();
			
			lookAction.Enable();
			lookAlwaysAction.Enable();
		}
		
		private void OnDisable()
		{
			movementAction.started -= OnMovementPress;
			movementAction.Disable();
			sprintAction.Disable();
			
			panAction.Disable();
			
			lookAction.Disable();
			lookAlwaysAction.Disable();
		}
		
		// Vars
		public float speed { get; private set; }

		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private bool _buttonHeld;
		private bool buttonHeld
		{
			get => _buttonHeld;
			set
			{
				_buttonHeld = value;
				if(!value) speed = beginningSpeed;
			}
		}
		
		private void Awake()
		{
			InputActionMap map = inputAsset.FindActionMap(ViewportMapID);
			movementAction = map.FindAction(MovementActionID);
			sprintAction = map.FindAction(SprintActionID);

			panAction = map.FindAction(PanActionID);
			lookAction = map.FindAction(LookActionID);
			lookAlwaysAction = map.FindAction(LookAlwaysActionID);
			
			speed = beginningSpeed;
		}
		
		private void Update()
		{
			HandleLook
			(
				lookAction.ReadValue<Vector2>(),
				lookAlwaysAction.ReadValue<Vector2>(),
				Mathf.Abs(panAction.ReadValue<float>() - 1.0f) < Mathf.Epsilon
			);
			HandleMovement
			(
				movementAction.ReadValue<Vector2>(),
				Mathf.Abs(sprintAction.ReadValue<float>() - 1.0f) < Mathf.Epsilon
			);
		}
		
		protected virtual void HandleMovement(Vector2 input, bool sprint)
		{
			// Check if key released
			if(input == Vector2.zero && buttonHeld) buttonHeld = false;
			
			Transform t = transform;
			
			// Scale
			input *= speed * Time.deltaTime;
			if(sprint) input *= sprintMultiplier;
			
			// Move in camera direction
			Vector3 moveVector = input.y * t.forward;
			moveVector += input.x * t.right;
			
			// Move
			t.position += moveVector;
			
			// Accelerate Camera
			if(buttonHeld) speed += acceleration * Time.deltaTime;
		}
		
		// Rotation vector
		private Vector3 rotation;
		
		protected virtual void HandleLook(Vector2 input, Vector2 inputAlways, bool shouldPan)
		{
			// Mouse
			if(shouldPan) rotation += new Vector3(-input.y, input.x, 0.0f) * sensitivity;
			
			// Arrow keys
			rotation += new Vector3(-inputAlways.y, inputAlways.x, 0.0f) * keySensitivity;
			
			// Set rotation
			transform.eulerAngles = rotation;
		}
	}
}