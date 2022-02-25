using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
	public int TotalRadius { get; private set; } = 0;
	public int TotalNumberOfCells { get; private set; } = 0;
	public int NumberOfCellsInWidth { get; private set; } = 0;
	public int NumberOfCellsInHeight { get; private set; } = 0;
	public Dictionary<Vector2, Chunk> Chunks { get; private set; } = new Dictionary<Vector2, Chunk>();

	[Range(-100f, 100f)]
	public float amplitude = 1f;
	[Range(-100f, 100f)]
	public float frequency = 1f;
	[Range(-100f, 100f)]
	public float xOffset = 1f;
	[Range(-100f, 100f)]
	public float yOffset = 1f;
	[Range(-10f, 10f)]
	public float isoLevel = 0f;
	[Range(1, 100)]
	public int radiusInChunks = 1;
	[Range(1, 100)]
	public int chunkSize = 1;
	[Range(1, 100)]
	public int chunkResolution = 1;
	public Vector2 center = new Vector2();

	[SerializeField]
	private Chunk chunkPrefab;
	
	private void Start()
	{
		DestroyPlanet();
		CreatePlanet();
	}

	public void CreatePlanet()
	{
		TotalRadius = CalculateActualRadius();
		TotalNumberOfCells = 0;
		NumberOfCellsInWidth = radiusInChunks * chunkSize * chunkResolution * 2;
		NumberOfCellsInHeight = radiusInChunks * chunkSize * chunkResolution * 2;

		for (int y = -radiusInChunks; y < radiusInChunks; y++)
		{
			for (int x = -radiusInChunks; x < radiusInChunks; x++)
			{
				Vector2 bottomLeftPosition = new Vector2(x * chunkSize, y * chunkSize) + center;

				Chunk chunk = Instantiate(chunkPrefab);
				chunk.Initialize(this, bottomLeftPosition);
				chunk.CreateMesh();
				chunk.transform.parent = transform;
				chunk.gameObject.name = $"BottomLeft = {chunk.BottomLeftPosition},  Center = {chunk.CenterPosition}";
				Chunks.Add(bottomLeftPosition, chunk);
				TotalNumberOfCells += chunk.MarchingSquaresHelper.Cells.Count;
			}
		}
	}

	public void DestroyPlanet()
	{
		foreach (KeyValuePair<Vector2, Chunk> entry in Chunks)
		{
			Chunk chunk = entry.Value;
			chunk.ClearMesh();

			if (Application.isPlaying)
			{
				Destroy(chunk.gameObject);
			}
			else
			{
				DestroyImmediate(chunk.gameObject);
			}
		}

		Chunks.Clear();
	}

	private int CalculateActualRadius()
	{
		return radiusInChunks * chunkSize;
	}

}
