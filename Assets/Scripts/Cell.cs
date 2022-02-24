using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public Vector2 v1, v2, v3, v4;
	public Vector2 e1, e2, e3, e4;
	public float d1, d2, d3, d4;

	public Cell(Vector2 _position, float size)
	{
		v1 = _position;
		v2 = new Vector2(v1.x + size, v1.y);
		v3 = new Vector2(v1.x + size, v1.y + size);
		v4 = new Vector2(v1.x, v1.y + size);

		float halfSize = size * .5f;
		e1 = new Vector2(v1.x + halfSize, v1.y);
		e2 = new Vector2(v1.x + size, v1.y + halfSize);
		e3 = new Vector2(v1.x + halfSize, v1.y + size);
		e4 = new Vector2(v1.x, v1.y + halfSize);

		d1 = 0f;
		d2 = 0f;
		d3 = 0f;
		d4 = 0f;
	}
}