using UnityEngine;using System;

public static class Sdf2D
{
	public static float Planet(Vector2 position, float radius, float amplitude, float frequency)
	{
		float result = 0f;

		if (position != null)
		{
			float xPosition = position.x / radius - 0.5f;
			float yPosition = position.y / radius - 0.5f;
			result += Circle(position, radius); //* Mathf.PerlinNoise(xPosition, yPosition) * amplitude;
			//Debug.Log(position);
			//Debug.Log(result);
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