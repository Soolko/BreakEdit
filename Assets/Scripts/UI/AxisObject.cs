using UnityEngine;
using UnityEngine.InputSystem;
using static BreakEdit.InputStrings;

namespace BreakEdit.UI
{
	[AddComponentMenu("BreakEdit/UI/Axis Object")]
	[RequireComponent(typeof(Renderer))]
	[RequireComponent(typeof(Collider))]
	public class AxisObject : MonoBehaviour
	{
		// Input System
		[Header("Input System")]
		public InputActionAsset inputAsset;
		private InputAction mousePositionAction;
		
		// Settings
		public enum Axis { none, x, y, z }
		[Header("Settings")]
		public Axis locked = Axis.none;
		
		// Components
		private Camera mainCamera;
		private AxisHandle parent;
		private Renderer r;
		private Rigidbody c;
		
		private void Awake()
		{
			// Input System
			InputActionMap map = inputAsset.FindActionMap(ViewportMapID);
			mousePositionAction = map.FindAction(MousePositionActionID);
			
			// Components
			mainCamera = FindObjectOfType<Camera>();
			parent = GetComponentInParent<AxisHandle>();
			r = GetComponent<Renderer>();
			c = GetComponent<Rigidbody>();
		}

		private void OnEnable() => mousePositionAction.Enable();
		private void OnDisable() => mousePositionAction.Disable();
		
		private bool selected = false;
		
		private Vector3 screenPoint;
		private Vector3 offset;
		
		private void OnMouseDown()
		{
			if(!selected) return;
			
			Vector3 position = parent.transform.position;
			screenPoint = mainCamera.WorldToScreenPoint(position);
			
			Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();
			offset = position - mainCamera.ScreenToWorldPoint
			(
				new Vector3
				(
					mousePosition.x,
					mousePosition.y,
					screenPoint.z
				)
			);
		}
		
		private void OnMouseDrag()
		{
			if(!selected) return;

			Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();
			Vector3 cursorPoint = new Vector3(mousePosition.x, mousePosition.y, screenPoint.z);
			Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(cursorPoint) + offset;

			Vector3 parentPosition = parent.transform.position;
			switch(locked)
			{
				case Axis.none:
				default:
					parent.transform.position = cursorPosition;
					break;
				case Axis.x:
					parentPosition.x = cursorPosition.x;
					parent.transform.position = parentPosition;
					break;
				case Axis.y:
					parentPosition.y = cursorPosition.y;
					parent.transform.position = parentPosition;
					break;
				case Axis.z:
					parentPosition.z = cursorPosition.z;
					parent.transform.position = parentPosition;
					break;
			}
		}
		
		public virtual void OnSelected()
		{
			selected = true;
			r.enabled = true;
			c.detectCollisions = true;
		}
		
		public virtual void OnDeselected()
		{
			selected = false;
			r.enabled = false;
			c.detectCollisions = false;
		}
	}
}