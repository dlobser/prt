using UnityEngine;
using System.Collections;

public class pointOnSurfaceToTexture : MonoBehaviour {

	protected Texture2D tex;
	public GameObject obj;
	protected Mesh mesh;
	public int resolution = 256;
	public Material mat;
	Vector3 min;
	Vector3 max;
	Vector3[] points;

	public bool useIndicators = false;


	public GameObject indicator;
	// Use this for initialization
	void Start () {
//		resolution = 32;
		mesh = obj.GetComponent<MeshFilter> ().mesh;
		tex = new Texture2D (resolution, resolution, TextureFormat.ARGB32, false);
		tex.filterMode = FilterMode.Point;
		mat.SetTexture ("_MainTex", tex);
		min = Vector3.zero;
		max = Vector3.zero;
		points = new Vector3[resolution * resolution];

		GetPoints ();
	}

	void GetPoints(){

		for (int i = 0; i < mesh.triangles.Length; i++) {
//			Debug.Log (mesh.triangles [i]);
		}

		Vector3[] rando = randomPoints (mesh, resolution * resolution);
		for (int i = 0; i < resolution; i++) {
			for (int j = 0; j < resolution; j++) {
//				int which = (int)((((float)i+((float)j*resolution))/(resolution*resolution))* ((mesh.triangles.Length)/3));
//				print (((mesh.triangles.Length)/3));
//				print(i);
//				print (j);
//				print((((float)i+((float)j*resolution)))/(resolution*resolution));///(resolution*resolution)));
//				which *= 3;
//				Vector3 a = mesh.vertices [mesh.triangles [which]];
//				Vector3 b = mesh.vertices [mesh.triangles [which+1]];
//				Vector3 c = mesh.vertices [mesh.triangles [which+2]];
//
//				Vector3 barycentric = new Vector3(Random.value, Random.value, Random.value);
//				float sum = barycentric.x + barycentric.y + barycentric.z;
//				Vector3 randValue = barycentric / sum;

//				Vector3 d = Vector3.Lerp (a, b, Random.value);
//				Vector3 e = Vector3.Lerp (d, c, Random.value);

				Vector3 e = rando [i + (j * resolution)];// GenerateRandomPoint (mesh);// a*barycentric.x+b*barycentric.y+c*barycentric.z;
					
				if (e.x < min.x)
					min.x = e.x;
				if (e.y < min.y)
					min.y = e.y;
				if (e.z < min.z)
					min.z = e.z;

				if (e.x > max.x)
					max.x = e.x;
				if (e.y > max.y)
					max.y = e.y;
				if (e.z > max.z)
					max.z = e.z;


//				print (e);

				points[i+(j*resolution)] = e;
//				tex.SetPixel (i, j, new Color (e.x,e.y,e.z, 1.0f));
			}
		}

		float scalar = (max.x - min.x);
		if (scalar < (max.y - min.y))
			scalar = (max.y - min.y);
		if (scalar < (max.z - min.z))
			scalar = (max.z - min.z);

		for (int i = 0; i < resolution; i++) {
			for (int j = 0; j < resolution; j++) {
//				Color col = tex.GetPixel (i, j);
				Vector3 e = points[i+(j*resolution)];
				e.x -= min.x;
				e.y -= min.y;
				e.z -= min.z;
				e.x /= scalar;
				e.y /= scalar;
				e.z /= scalar;
//				print (e);
//				print (min.z);
//				print (max.z);
				tex.SetPixel (i, j, new Color (e.x,e.y,e.z, 1.0f));
				if (useIndicators) {
					GameObject g = Instantiate (indicator);
					g.transform.position = e;
				}
//				tex.SetPixel (i, j, new);
			}
		}

		print (min);
		print (max);

		tex.Apply ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private Vector3[] randomPoints(Mesh mesh, int amount){

		float[] triangleSurfaceAreas = CalculateSurfaceAreas(mesh);

		// 2 - Normalize area weights
		float[] normalizedAreaWeights = NormalizeAreaWeights(triangleSurfaceAreas);

		Vector3[] rando = new Vector3[amount];

		for (int i = 0; i < rando.Length; i++) {
	
			// 3 - Generate 'triangle selection' random #
			float triangleSelectionValue = Random.value*.99f;

			// 4 - Walk through the list of weights to select the proper triangle
			int triangleIndex = SelectRandomTriangle (normalizedAreaWeights, triangleSelectionValue);

			// 5 - Generate a random barycentric coordinate
			Vector3 randomBarycentricCoordinates = GenerateRandomBarycentricCoordinates ();

			// 6 - Using the selected barycentric coordinate and the selected mesh triangle, convert
			//     this point to world space.
			rando[i] =  ConvertToLocalSpace(randomBarycentricCoordinates, triangleIndex, mesh);
		}
		return rando;


	}
 private Vector3 GenerateRandomPoint(Mesh mesh)
     {
         // 1 - Calculate Surface Areas
         float[] triangleSurfaceAreas = CalculateSurfaceAreas(mesh);
 
         // 2 - Normalize area weights
         float[] normalizedAreaWeights = NormalizeAreaWeights(triangleSurfaceAreas);
 
         // 3 - Generate 'triangle selection' random #
         float triangleSelectionValue = Random.value;
 
         // 4 - Walk through the list of weights to select the proper triangle
         int triangleIndex = SelectRandomTriangle(normalizedAreaWeights, triangleSelectionValue);
 
         // 5 - Generate a random barycentric coordinate
         Vector3 randomBarycentricCoordinates = GenerateRandomBarycentricCoordinates();
 
         // 6 - Using the selected barycentric coordinate and the selected mesh triangle, convert
         //     this point to world space.
         return ConvertToLocalSpace(randomBarycentricCoordinates, triangleIndex, mesh);
     }
 
     private float[] CalculateSurfaceAreas(Mesh mesh)
     {
		int triangleCount = (mesh.triangles.Length-3) / 3;
 
         float[] surfaceAreas = new float[triangleCount];
 
 
         for (int triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
         {
             Vector3[] points = new Vector3[3];
             points[0] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 0]];
             points[1] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 1]];
             points[2] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 2]];
 
             // calculate the three sidelengths and use those to determine the area of the triangle
             // http://www.wikihow.com/Sample/Area-of-a-Triangle-Side-Length
             float a = (points[0] - points[1]).magnitude;
             float b = (points[0] - points[2]).magnitude;
             float c = (points[1] - points[2]).magnitude;
 
             float s = (a + b + c) / 2;
 
             surfaceAreas[triangleIndex] = Mathf.Sqrt(s*(s - a)*(s - b)*(s - c));
         }
 
         return surfaceAreas;
     }
 
     private float[] NormalizeAreaWeights(float[] surfaceAreas)
     {
         float[] normalizedAreaWeights = new float[surfaceAreas.Length];
 
         float totalSurfaceArea = 0;
         foreach (float surfaceArea in surfaceAreas)
         {
             totalSurfaceArea += surfaceArea;
         }
 
         for (int i = 0; i < normalizedAreaWeights.Length; i++)
         {
             normalizedAreaWeights[i] = surfaceAreas[i] / totalSurfaceArea;
         }
 
         return normalizedAreaWeights;
     }
 
     private int SelectRandomTriangle(float[] normalizedAreaWeights, float triangleSelectionValue)
     {
         float accumulated = 0;
 
         for (int i = 0; i < normalizedAreaWeights.Length; i++)
         {
             accumulated += normalizedAreaWeights[i];
 
             if (accumulated >= triangleSelectionValue)
             {
                 return i;
             }
         }
 		
         // unless we were handed malformed normalizedAreaWeights, we should have returned from this already.
         throw new System.ArgumentException("Normalized Area Weights were not normalized properly, or triangle selection value was not [0, 1]" + triangleSelectionValue);
     }
 
     private Vector3 GenerateRandomBarycentricCoordinates()
     {
         Vector3 barycentric = new Vector3(Random.value, Random.value, Random.value);
 
         while (barycentric == Vector3.zero)
         {
             // seems unlikely, but just in case...
             barycentric = new Vector3(Random.value, Random.value, Random.value);
         }
 
         // normalize the barycentric coordinates. These are normalized such that x + y + z = 1, as opposed to
         // normal vectors which are normalized such that Sqrt(x^2 + y^2 + z^2) = 1. See:
         // http://en.wikipedia.org/wiki/Barycentric_coordinate_system
         float sum = barycentric.x + barycentric.y + barycentric.z;
 
         return barycentric / sum;
     }
 
     private Vector3 ConvertToLocalSpace(Vector3 barycentric, int triangleIndex, Mesh mesh)
     {
         Vector3[] points = new Vector3[3];
         points[0] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 0]];
         points[1] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 1]];
         points[2] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 2]];
 
         return (points[0] * barycentric.x + points[1] * barycentric.y + points[2] * barycentric.z);
     }
}