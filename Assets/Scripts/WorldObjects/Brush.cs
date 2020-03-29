using UnityEngine;

namespace BreakEdit.WorldObjects
{
	[AddComponentMenu("BreakEdit/World Objects/Brush")]
	[RequireComponent(typeof(MeshFilter))]
	public class Brush : MonoBehaviour
	{
		private Mesh mesh;
		
		private void Awake()
		{
			mesh = GetComponent<MeshFilter>().mesh;
		}
	}
}