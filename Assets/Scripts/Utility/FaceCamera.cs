using UnityEngine;

namespace BreakEdit.Utility
{
	[AddComponentMenu("BreakEdit/Utility/Face Camera")]
	public class FaceCamera : MonoBehaviour
	{
		[Header("Axis Freezing")]
		public bool freezeX = false;
		public bool freezeY = false;
		public bool freezeZ = false;
		
		// Components
		protected Transform cameraTransform { get; private set; }
		
		private void Awake() => cameraTransform = FindObjectOfType<Camera>().transform;
		
		private Vector3 rotation;
		private void Update()
		{
			Transform t = transform;
			rotation = t.localEulerAngles;
			t.LookAt(cameraTransform);
			
			Vector3 eulerAngles = t.localEulerAngles;
			eulerAngles = new Vector3
			(
				freezeX ? rotation.x : eulerAngles.x,
				freezeY ? rotation.y : eulerAngles.y,
				freezeZ ? rotation.z : eulerAngles.z
			);
			t.localEulerAngles = eulerAngles;
		}
	}
}