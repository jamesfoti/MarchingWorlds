using UnityEngine;

public static class Sdf2D
{
	public static float Planet(Vector2 position, float radius, float amplitude, float frequency, float xOffset, float yOffset)
	{
		// SOURCES:
		// https://link.springer.com/article/10.1007/s10035-021-01105-6

		float result = 0f;

		if (position != null)
		{
			float xPosition = (float) position.x / radius + xOffset;
			float yPosition = (float) position.y / radius + yOffset;
			result = Circle(position, radius) * Mathf.PerlinNoise(xPosition * frequency, yPosition * frequency) * amplitude;
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