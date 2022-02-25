using System.Collections.Generic;
using UnityEngine;

public class MarchingSquaresHelper
{
	public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
	public List<int> Triangles { get; private set; } = new List<int>();
	public List<Cell> Cells { get; private set; } = new List<Cell>();

	private float isoLevel = 0f;

	public MarchingSquaresHelper(Vector2 objectBottomLeftPosition, int numberOfCellsWidth, int numberOfCellsHeight, float cellSize = 1f, float isoLevel = 0f)
	{
		this.isoLevel = isoLevel;

		for (int y = 0; y < numberOfCellsHeight; y++)
		{
			for (int x = 0; x < numberOfCellsWidth; x++)
			{
				Vector2 bottomLeftCellPosition = new Vector2(x, y) * cellSize + objectBottomLeftPosition;
				Cell cell = new Cell(bottomLeftCellPosition, cellSize);
				Cells.Add(cell);
			}
		}
	}

	public void SetDensitiesProcedural2DPlanet(float radius, float amplitude, float frequency, float xOffset, float yOffset)
	{
		foreach (Cell cell in Cells)
		{
			cell.d1 = Sdf2D.Planet(cell.v1, radius, amplitude, frequency, xOffset, yOffset);
			cell.d2 = Sdf2D.Planet(cell.v2, radius, amplitude, frequency, xOffset, yOffset);
			cell.d3 = Sdf2D.Planet(cell.v3, radius, amplitude, frequency, xOffset, yOffset);
			cell.d4 = Sdf2D.Planet(cell.v4, radius, amplitude, frequency, xOffset, yOffset);
		}
	}

	public void MarchTheSquares()
	{
		Vertices.Clear();
		Triangles.Clear();

		foreach (Cell cell in Cells)
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

		if (cell.d1 > isoLevel)
		{
			result += 1;
		}

		if (cell.d2 > isoLevel)
		{
			result += 2;
		}

		if (cell.d3 > isoLevel)
		{
			result += 4;
		}

		if (cell.d4 > isoLevel)
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
}
