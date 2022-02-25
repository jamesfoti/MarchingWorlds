using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public Vector2 BottomLeftPosition { get; private set; } = new Vector2();
	public Vector2 CenterPosition { get; private set; } = new Vector2();
	public MarchingSquaresHelper MarchingSquaresHelper { get; private set; }

	private Planet planet = new Planet();
	private int chunkSize = 0;
	private float halfChunkSize = 0f;
	private float cellSize = 0f;
	private int numberOfCellsInWidth = 0;
	private int numberOfCellsInHeight = 0;
	private Mesh mesh;
	
	public void Initialize(Planet _planet, Vector3 _bottomLeftPosition)
	{
		planet = _planet;
		chunkSize = planet.chunkSize;
		halfChunkSize = chunkSize * .5f;
		cellSize = 1f / planet.chunkResolution;
		numberOfCellsInWidth = chunkSize * planet.chunkResolution;
		numberOfCellsInHeight = chunkSize * planet.chunkResolution;
		mesh = new Mesh();
	
		BottomLeftPosition = _bottomLeftPosition;
		CenterPosition = new Vector2(BottomLeftPosition.x + halfChunkSize, BottomLeftPosition.y + halfChunkSize);
		MarchingSquaresHelper = new MarchingSquaresHelper(BottomLeftPosition, numberOfCellsInWidth, numberOfCellsInHeight, cellSize, planet.isoLevel);
	}

	public void CreateMesh()
	{
		ClearMesh();
		MarchingSquaresHelper.SetDensitiesProcedural2DPlanet(planet.TotalRadius, planet.amplitude, planet.frequency, planet.xOffset, planet.yOffset);
		MarchingSquaresHelper.MarchTheSquares();
		mesh.vertices = MarchingSquaresHelper.Vertices.ToArray();
		mesh.triangles = MarchingSquaresHelper.Triangles.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void ClearMesh()
	{
		MarchingSquaresHelper.Vertices.Clear();
		MarchingSquaresHelper.Triangles.Clear();

		if (mesh != null)
		{
			mesh.Clear();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(CenterPosition, .1f);
		Handles.Label(CenterPosition, "Chunk Origin \n Position");

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(BottomLeftPosition, .1f);
		Handles.Label(BottomLeftPosition, "Chunk Bottom \n Left Position");
		
		foreach (Cell cell in MarchingSquaresHelper.Cells)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(cell.v1, .05f);
			Gizmos.DrawSphere(cell.v2, .05f);
			Gizmos.DrawSphere(cell.v3, .05f);
			Gizmos.DrawSphere(cell.v4, .05f);
			
			Gizmos.color = Color.Lerp(Color.white, Color.black, cell.d1);
			Gizmos.DrawWireSphere(cell.v1, .2f);
			Handles.Label(cell.v1, cell.d1.ToString());

			Gizmos.color = Color.Lerp(Color.white, Color.black, cell.d2);
			Gizmos.DrawWireSphere(cell.v2, .2f);
			Handles.Label(cell.v2, cell.d2.ToString());

			Gizmos.color = Color.Lerp(Color.white, Color.black, cell.d3);
			Gizmos.DrawWireSphere(cell.v3, .2f);
			Handles.Label(cell.v3, cell.d3.ToString());

			Gizmos.color = Color.Lerp(Color.white, Color.black, cell.d4);
			Gizmos.DrawWireSphere(cell.v4, .2f);
			Handles.Label(cell.v4, cell.d4.ToString());
		}
	}
}
