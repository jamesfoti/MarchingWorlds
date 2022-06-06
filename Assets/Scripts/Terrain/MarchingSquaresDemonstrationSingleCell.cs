using UnityEngine;

public class MarchingSquaresDemonstrationSingleCell : MonoBehaviour
{
	public Camera mainCamera;
	public Color offColor = Color.white;
	public Color onColor = Color.green;
	public float offDensityValue = 0f;
	public float onDensityValue = .5f;
	public float isoLevel = .5f;

	private MarchingSquaresHelper marchingSquaresHelper;
	private Mesh mesh;

	private void Start()
	{
		marchingSquaresHelper = new MarchingSquaresHelper(new Vector2(0, 0), 1, 1, 1, isoLevel);
		mesh = new Mesh();

		gameObject.transform.Find("v1").GetComponent<Renderer>().material.color = offColor;
		gameObject.transform.Find("v2").GetComponent<Renderer>().material.color = offColor;
		gameObject.transform.Find("v3").GetComponent<Renderer>().material.color = offColor;
		gameObject.transform.Find("v4").GetComponent<Renderer>().material.color = offColor;
	}

	private void Update()
	{
		DetectObjectClick();
	}

	private void DetectObjectClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.name == "v1")
				{
					Material material = hit.transform.GetComponent<Renderer>().material;
					if (material.color == offColor)
					{
						marchingSquaresHelper.cells[0].d1 = onDensityValue;
						material.color = onColor;
					}
					else
					{
						marchingSquaresHelper.cells[0].d1 = offDensityValue;
						material.color = offColor;
					} 
				}
				else if (hit.transform.name == "v2")
				{
					Material material = hit.transform.GetComponent<Renderer>().material;
					if (material.color == offColor)
					{
						marchingSquaresHelper.cells[0].d2 = onDensityValue;
						material.color = onColor;
					}
					else
					{
						marchingSquaresHelper.cells[0].d2 = offDensityValue;
						material.color = offColor;
					}
				}
				else if (hit.transform.name == "v3")
				{
					Material material = hit.transform.GetComponent<Renderer>().material;
					if (material.color == offColor)
					{
						marchingSquaresHelper.cells[0].d3 = onDensityValue;
						material.color = onColor;
					}
					else
					{
						marchingSquaresHelper.cells[0].d3 = offDensityValue;
						material.color = offColor;
					}
				}
				else if (hit.transform.name == "v4")
				{
					Material material = hit.transform.GetComponent<Renderer>().material;
					if (material.color == offColor)
					{
						marchingSquaresHelper.cells[0].d4 = onDensityValue;
						material.color = onColor;
					}
					else
					{
						marchingSquaresHelper.cells[0].d4 = offDensityValue;
						material.color = offColor;
					}
				}

				ClearMesh();
				CreateMesh();
			}
		}
	}

	private void CreateMesh()
	{
		marchingSquaresHelper.MarchTheSquares();
		mesh.vertices = marchingSquaresHelper.vertices.ToArray();
		mesh.triangles = marchingSquaresHelper.triangles.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void ClearMesh()
	{
		mesh.Clear();
		marchingSquaresHelper.vertices.Clear();
		marchingSquaresHelper.triangles.Clear();
	}
}
