using System.Collections.Generic;
using UnityEngine;

namespace BreakEdit.UI
{
	[AddComponentMenu("BreakEdit/UI/Axis Handle")]
	public class AxisHandle : MonoBehaviour
	{
		private AxisObject[] axisObjects;
		
		public readonly List<HandleCallback> callbacks = new List<HandleCallback>();
		
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

		public void SetPosition(Vector3 pos)
		{
			transform.position = pos;
			foreach(HandleCallback callback in callbacks) callback.PositionChange(pos);
		}
		
		public interface HandleCallback
		{
			void PositionChange(Vector3 pos);
		}
	}
}