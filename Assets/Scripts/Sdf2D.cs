using UnityEngine;

public static class Sdf2D
{
	public static float Planet(Vector2 position, TerrainData terrainData)
	{
		float result = 0f;

		if (position != null && position.magnitude <= terrainData.realWorldRadius)
		{
			for (int i = 0; i < terrainData.octaves; i++)
			{
				float xPosition = ((float)position.x / terrainData.realWorldRadius + terrainData.xOffset);
				float yPosition = ((float)position.y / terrainData.realWorldRadius + terrainData.yOffset);
				result += Mathf.PerlinNoise(xPosition * terrainData.frequency, yPosition * terrainData.frequency);

				terrainData.frequency /= 2f;
			}

			result /= terrainData.octaves;
		}

		return result;
	}

	public static float Circle(Vector2 position, float radius)
	{
		float result = 0f;

		if (position != null)
		{
			result = radius - position.magnitude;
		}

		return result;
	}

	private static Vector2 Abs(Vector2 position)
	{
		float x = Mathf.Abs(position.x);
		float y = Mathf.Abs(position.y);

		Vector2 result = new Vector2(x, y);
		return result;
	}
}