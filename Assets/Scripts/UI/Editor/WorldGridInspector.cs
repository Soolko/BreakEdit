using UnityEditor;
using UnityEngine;

namespace BreakEdit.UI
{
	[CustomEditor(typeof(WorldGrid))]
	internal class WorldGridInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			EditorGUILayout.Separator();
			if(GUILayout.Button("Generate"))	(target as WorldGrid)?.Generate();
			if(GUILayout.Button("Clear"))		(target as WorldGrid)?.Clear();
		}
	}
}