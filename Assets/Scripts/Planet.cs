using System;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
	public Dictionary<Vector2, Chunk> chunks { get; private set; } = new Dictionary<Vector2, Chunk>();

	public TerrainData terrainData = new TerrainData();

	[SerializeField]
	private Chunk chunkPrefab;
	
	private void Start()
	{
		DestroyPlanet();
		CreatePlanet();
	}

	public void CreatePlanet()
	{
		terrainData.realWorldRadius = CalculateActualRadius();

		for (int y = -terrainData.radiusInChunks; y < terrainData.radiusInChunks; y++)
		{
			for (int x = -terrainData.radiusInChunks; x < terrainData.radiusInChunks; x++)
			{
				Vector2 bottomLeftPosition = new Vector2(x * terrainData.chunkSize, y * terrainData.chunkSize) + terrainData.center;

				Chunk chunk = Instantiate(chunkPrefab);
				chunk.Initialize(terrainData, bottomLeftPosition);
				chunk.CreateMesh();
				chunk.transform.parent = transform;
				chunk.gameObject.name = $"BottomLeft = {chunk.bottomLeftPosition},  Center = {chunk.centerPosition}";
				chunks.Add(bottomLeftPosition, chunk);
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
		return terrainData.radiusInChunks * terrainData.chunkSize;
	}
}
