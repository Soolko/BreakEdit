using UnityEngine;

namespace BreakEdit.UI
{
	public class WorldGrid : MonoBehaviour
	{
		[Header("Prefabs")]
		public LineRenderer linePrefab;
		[Range(1.0f, 2.0f)]
		public float thicknessMultiplier = 1.25f;
		
		[Header("Bounds")]
		public Vector2Int xBounds = new Vector2Int(-1000, 1000);
		public Vector2Int zBounds = new Vector2Int(-1000, 1000);
		
		private void OnValidate()
		{
			if(xBounds.y <= xBounds.x) xBounds.y = xBounds.x + 1;
			if(zBounds.y <= zBounds.x) zBounds.y = zBounds.x + 1;
		}
		
		private void OnEnable() => Generate();
		private void OnDisable() => Clear();
		
		public void Generate()
		{
			int xMaxDigit = 10;
			while(xMaxDigit < Mathf.Max(Mathf.Abs(xBounds.x), Mathf.Abs(xBounds.y))) xMaxDigit *= 10;
			
			for(int x = xBounds.x; x < xBounds.y; x++)
			{
				LineRenderer line = Instantiate(linePrefab, transform);
				line.SetPositions(new Vector3[]
				{
					new Vector3(x, 0.0f, zBounds.x),
					new Vector3(x, 0.0f, zBounds.y)
				});

				for(int digit = 10; digit <= xMaxDigit; digit *= 10)
				{
					if(x % digit != 0) break;
					
					line.startWidth *= thicknessMultiplier;
					line.endWidth *= thicknessMultiplier;
				}
			}
			
			int zMaxDigit = 10;
			while(zMaxDigit < Mathf.Max(Mathf.Abs(zBounds.x), Mathf.Abs(zBounds.y))) zMaxDigit *= 10;
			
			for(int z = zBounds.x; z < zBounds.y; z++)
			{
				LineRenderer line = Instantiate(linePrefab, transform);
				line.SetPositions(new Vector3[]
				{
					new Vector3(xBounds.x, 0.0f, z),
					new Vector3(xBounds.y, 0.0f, z)
				});
				
				for(int digit = 10; digit <= xMaxDigit; digit *= 10)
				{
					if(z % digit != 0) break;
					
					line.startWidth *= thicknessMultiplier;
					line.endWidth *= thicknessMultiplier;
				}
			}
		}
		
		public void Clear()
		{
			foreach(LineRenderer obj in GetComponentsInChildren<LineRenderer>())
			{
				DestroyImmediate(obj.gameObject, false);
			}
		}
	}
}