using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
	public float TotalRadius { get; private set; } = 0f;

	[Range(-100f, 100f)]
	public float amplitude = 1f;
	[Range(-10f, 10f)]
	public float frequency = 1f;
	[Range(-10f, 10f)]
	public float isoLevel = 0f;
	[Range(1, 100)]
	public int radiusInChunks = 1;
	[Range(1, 100)]
	public int chunkSize = 1;
	[Range(1, 10)]
	public int chunkResolution = 1;
	public Vector2 center = new Vector2();

	[SerializeField]
	private Chunk chunkPrefab;
	private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

	private void Start()
	{
		CreatePlanet();
	}

	public void CreatePlanet()
	{
		TotalRadius = CalculateActualRadius();

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
				chunks.Add(bottomLeftPosition, chunk);
			}
		}
	}

	public void DestroyPlanet()
	{
		foreach (KeyValuePair<Vector2, Chunk> entry in chunks)
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

		chunks.Clear();
	}

	private float CalculateActualRadius()
	{
		return radiusInChunks * chunkSize;
	}

}
