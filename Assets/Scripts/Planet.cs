using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
	public int totalRadius { get; private set; } = 0;
	public int totalNumberOfCells { get; private set; } = 0;
	public int numberOfCellsInWidth { get; private set; } = 0;
	public int numberOfCellsInHeight { get; private set; } = 0;
	public Dictionary<Vector2, Chunk> chunks { get; private set; } = new Dictionary<Vector2, Chunk>();

	[Range(-100f, 100f)]
	public float amplitude = 1f;
	[Range(-100f, 100f)]
	public float frequency = 1f;
	[Range(-100f, 100f)]
	public float xOffset = 1f;
	[Range(-100f, 100f)]
	public float yOffset = 1f;
	[Range(1f, 100f)]
	public float scale = 1f;
	[Range(-10f, 10f)]
	public float isoLevel = 1f;
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
		totalRadius = CalculateActualRadius();
		totalNumberOfCells = 0;
		numberOfCellsInWidth = radiusInChunks * chunkSize * chunkResolution * 2;
		numberOfCellsInHeight = radiusInChunks * chunkSize * chunkResolution * 2;

		for (int y = -radiusInChunks; y < radiusInChunks; y++)
		{
			for (int x = -radiusInChunks; x < radiusInChunks; x++)
			{
				Vector2 bottomLeftPosition = new Vector2(x * chunkSize, y * chunkSize) + center;

				Chunk chunk = Instantiate(chunkPrefab);
				chunk.Initialize(this, bottomLeftPosition);
				chunk.CreateMesh();
				chunk.transform.parent = transform;
				chunk.gameObject.name = $"BottomLeft = {chunk.bottomLeftPosition},  Center = {chunk.centerPosition}";
				chunks.Add(bottomLeftPosition, chunk);
				totalNumberOfCells += chunk.cells.Count;
			}
		}
	}

	public void DestroyPlanet()
	{
		foreach (KeyValuePair<Vector2, Chunk> entry in chunks)
		{
			Chunk chunk = entry.Value;
			chunk.ClearMeshData();

			if (Application.isPlaying)
			{
				Destroy(chunk.gameObject);
			}
			else
			{
				DestroyImmediate(chunk.gameObject);
			}
		}

		chunks.Clear();
	}

	private int CalculateActualRadius()
	{
		return radiusInChunks * chunkSize;
	}

}
