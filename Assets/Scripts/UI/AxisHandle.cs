using UnityEngine;

namespace BreakEdit.UI
{
	[AddComponentMenu("BreakEdit/UI/Axis Handle")]
	public class AxisHandle : MonoBehaviour
	{
		private AxisObject[] axisObjects;

		private void Awake()
		{
			axisObjects = GetComponentsInChildren<AxisObject>();
		}
		
		public virtual void OnSelected()
		{
			foreach(AxisObject obj in axisObjects) obj.OnSelected();
		}
		
		public virtual void OnDeselected()
		{
			foreach(AxisObject obj in axisObjects) obj.OnDeselected();
		}
	}
}