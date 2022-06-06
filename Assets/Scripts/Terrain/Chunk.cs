using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public Vector2 bottomLeftPosition { get; private set; } = new Vector2();
	public Vector2 centerPosition { get; private set; } = new Vector2();

	private MarchingSquaresHelper marchingSquaresHelper;
	private Mesh mesh;
	private TerrainData terrainData;
	private float halfChunkSize = 0f;
	private float cellSize = 0f;
	private int numberOfCellsInWidth = 0;
	private int numberOfCellsInHeight = 0;

	public void Initialize(Vector3 _bottomLeftPosition, TerrainData _terrainData)
	{
		mesh = new Mesh();
		terrainData = _terrainData;
		halfChunkSize = terrainData.chunkSize * .5f;
		cellSize = 1f / terrainData.chunkResolution;
		numberOfCellsInWidth = terrainData.chunkSize * terrainData.chunkResolution;
		numberOfCellsInHeight = terrainData.chunkSize * terrainData.chunkResolution;
		bottomLeftPosition = _bottomLeftPosition;
		centerPosition = new Vector2(bottomLeftPosition.x + halfChunkSize, bottomLeftPosition.y + halfChunkSize);
		marchingSquaresHelper = new MarchingSquaresHelper(bottomLeftPosition, numberOfCellsInWidth, numberOfCellsInHeight, cellSize, terrainData.isoLevel);
	}

	public void CreateMesh()
	{
		SetDensities();
		marchingSquaresHelper.MarchTheSquares();
		mesh.vertices = marchingSquaresHelper.vertices.ToArray();
		mesh.triangles = marchingSquaresHelper.triangles.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void ClearMesh()
	{
		marchingSquaresHelper.vertices.Clear();
		marchingSquaresHelper.triangles.Clear();
		mesh.Clear();
	}

	public void SetDensities()
	{
		foreach (Cell cell in marchingSquaresHelper.cells)
		{
			cell.d1 = Sdf2D.Planet(cell.v1, terrainData);
			cell.d2 = Sdf2D.Planet(cell.v2, terrainData);
			cell.d3 = Sdf2D.Planet(cell.v3, terrainData);
			cell.d4 = Sdf2D.Planet(cell.v4, terrainData);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(centerPosition, .1f);
		Handles.Label(centerPosition, "Chunk Origin \n Position");

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(bottomLeftPosition, .1f);
		Handles.Label(bottomLeftPosition, "Chunk Bottom \n Left Position");
		
		foreach (Cell cell in marchingSquaresHelper.cells)
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
