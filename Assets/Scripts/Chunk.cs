using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public Vector2 bottomLeftPosition { get; private set; } = new Vector2();
	public Vector2 centerPosition { get; private set; } = new Vector2();
	public List<Vector3> vertices { get; private set; } = new List<Vector3>();
	public List<int> triangles { get; private set; } = new List<int>();
	public List<Cell> cells { get; private set; } = new List<Cell>();

	private Mesh mesh;
	private Planet planet = new Planet();
	private int chunkSize = 0;
	private float halfChunkSize = 0f;
	private float cellSize = 0f;
	private int numberOfCellsInWidth = 0;
	private int numberOfCellsInHeight = 0;

	public void Initialize(Planet _planet, Vector3 _bottomLeftPosition)
	{
		mesh = new Mesh();
		planet = _planet;
		chunkSize = planet.chunkSize;
		halfChunkSize = chunkSize * .5f;
		cellSize = 1f / planet.chunkResolution;
		numberOfCellsInWidth = chunkSize * planet.chunkResolution;
		numberOfCellsInHeight = chunkSize * planet.chunkResolution;

		bottomLeftPosition = _bottomLeftPosition;
		centerPosition = new Vector2(bottomLeftPosition.x + halfChunkSize, bottomLeftPosition.y + halfChunkSize);

		for (int y = 0; y < numberOfCellsInHeight; y++)
		{
			for (int x = 0; x < numberOfCellsInWidth; x++)
			{
				Vector2 bottomLeftCellPosition = new Vector2(x, y) * cellSize + bottomLeftPosition;
				Cell cell = new Cell(bottomLeftCellPosition, cellSize);
				cells.Add(cell);
			}
		}
	}

	public void CreateMesh()
	{
		ClearMeshData();
		SetDensities();
		MarchTheSquares();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void ClearMeshData()
	{
		vertices.Clear();
		triangles.Clear();
		mesh.Clear();
	}

	private void SetDensities()
	{
		foreach (Cell cell in cells)
		{
			cell.d1 = Sdf2D.Planet(cell.v1, planet.totalRadius, planet.amplitude, planet.frequency, planet.xOffset, planet.yOffset, planet.scale);
			cell.d2 = Sdf2D.Planet(cell.v2, planet.totalRadius, planet.amplitude, planet.frequency, planet.xOffset, planet.yOffset, planet.scale);
			cell.d3 = Sdf2D.Planet(cell.v3, planet.totalRadius, planet.amplitude, planet.frequency, planet.xOffset, planet.yOffset, planet.scale);
			cell.d4 = Sdf2D.Planet(cell.v4, planet.totalRadius, planet.amplitude, planet.frequency, planet.xOffset, planet.yOffset, planet.scale);
		}
	}

	private void MarchTheSquares()
	{
		foreach (Cell cell in cells)
		{
			March(cell);
		}
	}

	private void March(Cell cell)
	{
		int binaryIndex = CalculateBinaryIndex(cell);

		switch (binaryIndex)
		{
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
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		vertices.Add(d);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	private void AddPentagon(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e)
	{
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		vertices.Add(d);
		vertices.Add(e);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 3);
		triangles.Add(vertexIndex + 4);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(centerPosition, .1f);
		Handles.Label(centerPosition, "Chunk Origin \n Position");

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(bottomLeftPosition, .1f);
		Handles.Label(bottomLeftPosition, "Chunk Bottom \n Left Position");
		
		foreach (Cell cell in cells)
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
