using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BreakEdit.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using static BreakEdit.InputStrings;

namespace BreakEdit.WorldObjects
{
	[AddComponentMenu("BreakEdit/World Objects/Brush Face")]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshCollider))]
	public class BrushFace : MonoBehaviour
	{
		[Header("Input System")]
		public InputActionAsset inputAsset;
		private InputAction mouseClickAction, mousePositionAction;
		
		private void OnEnable()
		{
			mouseClickAction.Enable();
			mouseClickAction.performed += OnMouseClick;
			
			mousePositionAction.Enable();
		}
		
		private void OnDisable()
		{
			mouseClickAction.performed -= OnMouseClick;
			mouseClickAction.Disable();
			
			mousePositionAction.Disable();
		}
		
		[Header("UI")]
		public AxisHandle handlePrefab;
		public AxisHandle handle { get; protected set; }
		
		public bool selected;
		
		[Header("Face Data"), SerializeField]
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private Vector3[] _vertices;
		public Vector3[] vertices
		{
			get => _vertices;
			set
			{
				_vertices = value;
				CalculateCentrePosition();
			}
		}
		public int[] triangles;
		
		public Mesh mesh { get; protected set; }
		
		public Vector3 centrePosition { get; private set; }
		
		// Components
		private MeshFilter meshFilter;
		private MeshCollider meshCollider;
		
		private void Awake()
		{
			// Input System
			InputActionMap map = inputAsset.FindActionMap(ViewportMapID);
			mouseClickAction = map.FindAction(MouseClickActionID);
			mousePositionAction = map.FindAction(MousePositionActionID);
			
			// Components
			meshFilter = GetComponent<MeshFilter>();
			meshCollider = GetComponent<MeshCollider>();
		}
		
		private void Start()
		{
			CalculateCentrePosition();
			
			// Handle
			handle = Instantiate(handlePrefab, transform, false);
			handle.transform.position = centrePosition;
			handle.OnDeselected();
			
			// Generate mesh
			GenerateMesh();
		}

		public void GenerateMesh()
		{
			mesh = new Mesh
			{
				vertices = vertices,
				triangles = triangles
			};
			meshFilter.mesh = mesh;
			meshCollider.sharedMesh = mesh;
		}
		
		protected virtual void OnMouseClick(InputAction.CallbackContext context)
		{
			// Raycast on click
			Ray ray = FindObjectOfType<Camera>().ScreenPointToRay(mousePositionAction.ReadValue<Vector2>());
			
			// Check if this was hit
			if(!Physics.Raycast(ray, out RaycastHit hit) || hit.collider.gameObject != gameObject)
			{
				handle.OnDeselected();
				return;
			}
			
			// Set all brushes to not selected
			foreach(BrushFace face in FindObjectsOfType<BrushFace>()) face.selected = false;
			
			// Set this one to selected
			selected = true;
			handle.OnSelected();
		}
		
		private void CalculateCentrePosition()
		{
			Vector3 total = vertices.Aggregate(Vector3.zero, (current, vertex) => current + vertex);
			centrePosition = total / (float) vertices.Length;
		}
	}
}