﻿using UnityEngine;

namespace BreakEdit.WorldObjects
{
	[AddComponentMenu("BreakEdit/World Objects/Brush")]
	[RequireComponent(typeof(MeshFilter))]
	public class Brush : MonoBehaviour
	{
		// Mesh
		public Mesh mesh { get; protected set; }
		
		// Cube
		private static readonly Vector3[] CubeVertices =
		{
			new Vector3(0, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(1, 1, 0),
			new Vector3(0, 1, 0),
			new Vector3(0, 1, 1),
			new Vector3(1, 1, 1),
			new Vector3(1, 0, 1),
			new Vector3(0, 0, 1),
		};
		
		private static readonly int[] CubeTriangles =
		{
			// Front
			0, 2, 1,
			0, 3, 2,
			
			// Top
			2, 3, 4,
			2, 4, 5,
			
			// Right
			1, 2, 5,
			1, 5, 6,
			
			// Left
			0, 7, 4,
			0, 4, 3,
			
			// Back
			5, 4, 7,
			5, 7, 6,
			
			// Bottom
			0, 6, 7,
			0, 1, 6,
		};
		
		private void Start()
		{
			mesh = new Mesh
			{
				vertices = CubeVertices,
				triangles = CubeTriangles
			};
			GetComponent<MeshFilter>().mesh = mesh;
		}
	}
}