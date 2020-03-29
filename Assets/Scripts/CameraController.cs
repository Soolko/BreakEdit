using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics.CodeAnalysis;
using static BreakEdit.InputStrings;

namespace BreakEdit
{
	[AddComponentMenu("BreakEdit/Camera Controller")]
	public sealed class CameraController : MonoBehaviour
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
		
		[Header("Input System")]
		public InputActionAsset inputAsset;
		private InputAction movementAction, movementVerticalAction;
		private InputAction sprintAction;
		private InputAction panAction;
		private InputAction lookAction, lookAlwaysAction;
		
		#pragma warning restore 0649
		
		private void OnMovementPress(InputAction.CallbackContext context) => buttonHeld = true;
		
		private void OnEnable()
		{
			movementAction.Enable();
			movementAction.started += OnMovementPress;
			
			movementVerticalAction.Enable();
			movementVerticalAction.started += OnMovementPress;
			
			sprintAction.Enable();
			
			panAction.Enable();
			
			lookAction.Enable();
			lookAlwaysAction.Enable();
		}
		
		private void OnDisable()
		{
			movementAction.started -= OnMovementPress;
			movementAction.Disable();
			
			movementVerticalAction.started -= OnMovementPress;
			movementVerticalAction.Disable();
			
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
			// Get the map
			InputActionMap map = inputAsset.FindActionMap(ViewportMapID);
			
			// Define the actions
			movementAction = map.FindAction(MovementActionID);
			movementVerticalAction = map.FindAction(MovementVerticalActionID);
			
			sprintAction = map.FindAction(SprintActionID);
			
			panAction = map.FindAction(PanActionID);
			
			lookAction = map.FindAction(LookActionID);
			lookAlwaysAction = map.FindAction(LookAlwaysActionID);
			
			// Set speed initial state
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
			
			Vector2 xz = movementAction.ReadValue<Vector2>();
			float y = movementVerticalAction.ReadValue<float>();
			HandleMovement
			(
				new Vector3(xz.x, y, xz.y), 
				Mathf.Abs(sprintAction.ReadValue<float>() - 1.0f) < Mathf.Epsilon
			);
		}
		
		private void HandleMovement(Vector3 input, bool sprint)
		{
			// Check if key released
			if(input == Vector3.zero && buttonHeld) buttonHeld = false;
			
			Transform t = transform;
			
			// Scale
			input *= speed * Time.deltaTime;
			if(sprint) input *= sprintMultiplier;
			
			// Move in camera direction
			Vector3 moveVector = input.z * t.forward;
			moveVector += input.x * t.right;
			moveVector += input.y * Vector3.up;
			
			// Move
			t.position += moveVector;
			
			// Accelerate Camera
			if(buttonHeld) speed += acceleration * Time.deltaTime;
		}
		
		// Rotation vector
		private Vector3 rotation;
		
		private void HandleLook(Vector2 input, Vector2 inputAlways, bool shouldPan)
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