using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
	public List<int> Triangles { get; private set; } = new List<int>();
	public Vector2 BottomLeftPosition { get; private set; } = new Vector2();
	public Vector2 CenterPosition { get; private set; } = new Vector2();

	private Planet planet = new Planet();
	private int chunkSize = 0;
	private float halfSize = 0f;
	private float cellSize = 0f;
	private int numberOfCellsInWidth = 0;
	private int numberOfCellsInHeight = 0;
	private List<Cell> cells = new List<Cell>();
	private Mesh mesh;
	
	public void Initialize(Planet _planet, Vector3 _bottomLeftPosition)
	{
		planet = _planet;
		chunkSize = planet.chunkSize;
		halfSize = chunkSize * .5f;
		cellSize = 1f / planet.chunkResolution;
		numberOfCellsInWidth = chunkSize * planet.chunkResolution;
		numberOfCellsInHeight = chunkSize * planet.chunkResolution;
		cells = new List<Cell>();
		mesh = new Mesh();
	
		Vertices = new List<Vector3>();
		Triangles = new List<int>();
		BottomLeftPosition = _bottomLeftPosition;
		CenterPosition = new Vector2(BottomLeftPosition.x + halfSize, BottomLeftPosition.y + halfSize);

		for (int y = 0; y < numberOfCellsInHeight; y++)
		{
			for (int x = 0; x < numberOfCellsInWidth; x++)
			{
				Vector2 bottomLeftCellPosition = new Vector2(x, y) * cellSize + BottomLeftPosition;
				Cell cell = new Cell(bottomLeftCellPosition, cellSize);
				cells.Add(cell);
			}
		}
	}

	public void CreateMesh()
	{
		ClearMesh();
		SetDensities();
		MarchTheSquares(cells);
		mesh.vertices = Vertices.ToArray();
		mesh.triangles = Triangles.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void SetDensities()
	{
		foreach (Cell cell in cells)
		{
			cell.d1 = Sdf2D.Planet(cell.v1, planet.TotalRadius, planet.amplitude, planet.frequency);
			cell.d2 = Sdf2D.Planet(cell.v2, planet.TotalRadius, planet.amplitude, planet.frequency);
			cell.d3 = Sdf2D.Planet(cell.v3, planet.TotalRadius, planet.amplitude, planet.frequency);
			cell.d4 = Sdf2D.Planet(cell.v4, planet.TotalRadius, planet.amplitude, planet.frequency);
		}
	}

	public void ClearMesh()
	{
		if (Vertices != null)
		{
			Vertices.Clear();
		}

		if (Triangles != null)
		{
			Triangles.Clear();
		}

		if (mesh != null)
		{
			mesh.Clear();
		}
	}

	public void MarchTheSquares(List<Cell> _cells)
	{
		Vertices.Clear();
		Triangles.Clear();

		foreach (Cell cell in _cells)
		{
			March(cell);
		}
	}

	private void March(Cell cell)
	{
		int binaryIndex = CalculateBinaryIndex(cell);

		switch (binaryIndex)
		{
			/*
			 * Vector3 from = new Vector3 (1f, 2f, 3f);
				Vector3 to = new Vector3 (5f, 6f, 7f);

				// Here result = (4, 5, 6)
				Vector3 result = Vector3.Lerp (from, to, 0.75f);*/
			case 0:
				return;
			case 1:
				AddTriangle(cell.v1, cell.e4, cell.e1);
				break;
			case 2:
				AddTriangle(cell.v2, cell.e1, cell.e2);
				break;
			case 3:
				AddQuad(cell.v1, cell.e4, cell.e2, cell.v2);
				break;
			case 4:
				AddTriangle(cell.v3, cell.e2, cell.e3);
				break;
			case 5:
				AddTriangle(cell.v1, cell.e4, cell.e1);
				AddQuad(cell.e1, cell.e4, cell.e3, cell.e2);
				AddTriangle(cell.v3, cell.e2, cell.e3);
				break;
			case 6:
				AddQuad(cell.e1, cell.e3, cell.v3, cell.v2);
				break;
			case 7:
				//AddPentagon(a.position, c.position, c.xEdgePosition, b.yEdgePosition, b.position);
				AddTriangle(cell.v1, cell.e4, cell.v2);
				AddTriangle(cell.v2, cell.e4, cell.e3);
				AddTriangle(cell.v3, cell.v2, cell.e3);
				break;
			case 8:
				AddTriangle(cell.v4, cell.e3, cell.e4);
				break;
			case 9:
				AddQuad(cell.v1, cell.v4, cell.e3, cell.e1);
				break;
			case 10:
				//AddQuad(a.xEdgePosition, c.xEdgePosition, d.position, b.position);
				AddTriangle(cell.v2, cell.e1, cell.e2);
				AddQuad(cell.e1, cell.e4, cell.e3, cell.e2);
				AddTriangle(cell.v4, cell.e3, cell.e4);
				break;
			case 11:
				//AddPentagon(b.position, a.position, a.yEdgePosition, c.xEdgePosition, d.position);
				AddTriangle(cell.v4, cell.e3, cell.v1);
				AddTriangle(cell.v1, cell.e3, cell.e2);
				AddTriangle(cell.e2, cell.v2, cell.v1);
				break;
			case 12:
				AddQuad(cell.e4, cell.v4, cell.v3, cell.e2);
				break;
			case 13:
				AddPentagon(cell.v1, cell.v4, cell.v3, cell.e2, cell.e1);
				break;
			case 14:
				AddPentagon(cell.e4, cell.v4, cell.v3, cell.v2, cell.e1);
				break;
			case 15:
				AddQuad(cell.v1, cell.v4, cell.v3, cell.v2);
				break;
		}
	}

	private int CalculateBinaryIndex(Cell cell)
	{
		int result = 0;

		if (cell.d1 > planet.isoLevel)
		{
			result += 1;
		}

		if (cell.d2 > planet.isoLevel)
		{
			result += 2;
		}

		if (cell.d3 > planet.isoLevel)
		{
			result += 4;
		}

		if (cell.d4 > planet.isoLevel)
		{
			result += 8;
		}

		return result;
	}

	private void AddTriangle(Vector3 a, Vector3 b, Vector3 c)
	{
		int vertexIndex = Vertices.Count;
		Vertices.Add(a);
		Vertices.Add(b);
		Vertices.Add(c);
		Triangles.Add(vertexIndex);
		Triangles.Add(vertexIndex + 1);
		Triangles.Add(vertexIndex + 2);
	}

	private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		int vertexIndex = Vertices.Count;
		Vertices.Add(a);
		Vertices.Add(b);
		Vertices.Add(c);
		Vertices.Add(d);
		Triangles.Add(vertexIndex);
		Triangles.Add(vertexIndex + 1);
		Triangles.Add(vertexIndex + 2);
		Triangles.Add(vertexIndex);
		Triangles.Add(vertexIndex + 2);
		Triangles.Add(vertexIndex + 3);
	}

	private void AddPentagon(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e)
	{
		int vertexIndex = Vertices.Count;
		Vertices.Add(a);
		Vertices.Add(b);
		Vertices.Add(c);
		Vertices.Add(d);
		Vertices.Add(e);
		Triangles.Add(vertexIndex);
		Triangles.Add(vertexIndex + 1);
		Triangles.Add(vertexIndex + 2);
		Triangles.Add(vertexIndex);
		Triangles.Add(vertexIndex + 2);
		Triangles.Add(vertexIndex + 3);
		Triangles.Add(vertexIndex);
		Triangles.Add(vertexIndex + 3);
		Triangles.Add(vertexIndex + 4);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(CenterPosition, .1f);
		Handles.Label(CenterPosition, "Chunk Origin \n Position");

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(BottomLeftPosition, .1f);
		Handles.Label(BottomLeftPosition, "Chunk Bottom \n Left Position");

		foreach (Cell cell in cells)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(cell.v1, .05f);
			Gizmos.DrawSphere(cell.v2, .05f);
			Gizmos.DrawSphere(cell.v3, .05f);
			Gizmos.DrawSphere(cell.v4, .05f);
		}
	}
}
