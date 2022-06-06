using System;
using UnityEngine;

[Serializable]
public struct TerrainData
{
	[Range(1f, 100f)]
	public float frequency;
	[Range(1, 100)]
	public int octaves;
	[Range(-100f, 100f)]
	public float xOffset;
	[Range(-100f, 100f)]
	public float yOffset;
	[Range(0f, 1f)]
	public float isoLevel;
	[Range(1, 100)]
	public int chunkResolution;
	[Range(1, 100)]
	public int chunkSize;
	[Range(1, 100)]
	public int radiusInChunks;
	[HideInInspector]
	public float realWorldRadius;
	public Vector2 center;
	public bool isWallsErected;
	public float wallDepth;
}

