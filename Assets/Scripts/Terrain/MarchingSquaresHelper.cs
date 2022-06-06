using System.Collections.Generic;
using UnityEngine;

public class MarchingSquaresHelper
{
	public List<Vector3> vertices { get; private set; } = new List<Vector3>();
	public List<int> triangles { get; private set; } = new List<int>();
	public List<Cell> cells { get; private set; } = new List<Cell>();

	private float isoLevel = 0f;

	public MarchingSquaresHelper(Vector2 _position, int _width, int _height, float _cellSize, float _isoLevel)
	{
		isoLevel = _isoLevel;

		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				Vector2 bottomLeftCellPosition = new Vector2(x, y) * _cellSize + _position;
				Cell cell = new Cell(bottomLeftCellPosition, _cellSize);
				cells.Add(cell);
			}
		}
	}

	public void MarchTheSquares()
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
				AddHexagon(cell.v1, cell.e4, cell.e3, cell.v3, cell.e2, cell.e1);
				break;
			case 6:
				AddQuad(cell.e1, cell.e3, cell.v3, cell.v2);
				break;
			case 7:
				AddPentagon(cell.v1, cell.e4, cell.e3, cell.v3, cell.v2);
				break;
			case 8:
				AddTriangle(cell.v4, cell.e3, cell.e4);
				break;
			case 9:
				AddQuad(cell.v1, cell.v4, cell.e3, cell.e1);
				break;
			case 10:
				AddHexagon(cell.e1, cell.e4, cell.v4, cell.e3, cell.e2, cell.v2);
				break;
			case 11:
				AddPentagon(cell.v1, cell.v4, cell.e3, cell.e2, cell.v2);
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

	private void AddHexagon(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, Vector3 f)
	{
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		vertices.Add(d);
		vertices.Add(e);
		vertices.Add(f);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 3);
		triangles.Add(vertexIndex + 4);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 4);
		triangles.Add(vertexIndex + 5);
	}
}
