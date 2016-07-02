using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]

public class wireFrameAthon : MonoBehaviour {
	
	MeshRenderer MRend;
	MeshFilter MFilter;
	Mesh MMesh;
	
	Renderer rend;

	public float lineWidth = 1;
	public float speed;
	public float[] colors;
	public float colorRandomize = .05f;
	public float saturation=1;
	public float brightness=1;

    //public RenderTexture[] resizeTextures;

	public RenderTexture renderTex;

	public GameObject ping;
	public Material pingMat;
	
	public Material lineMat;
	public Texture2D lineTex;
	public Texture2D shadowTex;
	Mesh mesh;
	Hashtable edges;
	Hashtable Verts;
	Hashtable VertNormCheck;
	public float textureTile = 100;

	public int numPoints = 1024;
	
	Texture2D texture;
	Texture2D posTexture;
	
	List<Vector3> vertPositions;
	
	public GameObject wireFrame;
	public float normalOffset = .1f;
	
	public float pointSize = .5f;
	public Color pointColor = Color.white;
	
	float[] distances;
	
	private const int h1 = 12178051;
	private const int h2 = 12481319;
	private const int h3 = 15485863;
	
	int pointCount;
	
	Vector3[] verts;
	int[] faces;
	
	Matrix4x4 prevMat;
	Vector3 prevScale;
	
	float prevNormalOffset;
	
	public float shadowSpeed = 1;
	public float shadowTile = 1;
	
	public float hueShift;
	public float minEdgeDistance = .0001f;
	public float maxEdgeDistance = 1e6f;

	List<GameObject> meshObjects;

	int count = -1;
	int dCount = -1;
	int tCount = -1;
	int triCount = 0;

	void makeMesh(){
		GameObject n = new GameObject ();
		n.AddComponent<MeshRenderer> ();
		n.AddComponent<MeshFilter> ();
		n.AddComponent<updateBounds> ();
//		n.AddComponent<Renderer>();
		meshObjects.Add(n);
	}

	void connectShaders(){
		GameObject n = meshObjects [meshObjects.Count - 1];
		MRend = n.GetComponent<MeshRenderer> ();
		MFilter = n.GetComponent<MeshFilter> ();
//		Renderer Rend = n.GetComponent<Renderer>();
		MFilter.mesh = new Mesh();
		mesh = MFilter.mesh;
		MRend.sharedMaterial = lineMat;
		MRend.sharedMaterial.SetTexture ("_MainTex", texture);
		MRend.sharedMaterial.SetTexture ("_SpriteTex", lineTex);
		MRend.sharedMaterial.SetTexture ("_ShadowTex", shadowTex);
		MRend.sharedMaterial.SetTexture ("_PosTex", posTexture);
		MRend.sharedMaterial.SetTexture ("_Offset", renderTex);
	}
	
	// Use this for initialization
	void Start () {
        Init();
	}

    public void Init() {

     //   for (int i = 0; i < resizeTextures.Length; i++) {
      //      resizeTextures[i] = new RenderTexture(numPoints, numPoints, 24);
      //      resizeTextures[i].filterMode = FilterMode.Point;
           // resizeTextures[i].height = numPoints;
           // resizeTextures[i].width = numPoints;
      //  }

        meshObjects = new List<GameObject>();
        makeMesh();
        connectShaders();

        vertPositions = new List<Vector3>();

        //		MRend = GetComponent<MeshRenderer> ();
        //		MFilter = GetComponent<MeshFilter> ();
        //		MRend.sharedMaterial = lineMat;
        //		rend = GetComponent<Renderer> ();

        if (colors.Length < 1)
            colors = new float[] { .5f };

        edges = new Hashtable();
        Verts = new Hashtable();
        VertNormCheck = new Hashtable();


        //		mesh = wireFrame.GetComponent<MeshFilter> ().mesh;
       // texture = new Texture2D(numPoints, numPoints, TextureFormat.RGBA32, false);
        //texture = new Texture2D(numPoints, numPoints, TextureFormat.RGBAFloat, false);
       // texture.filterMode = FilterMode.Point;
        posTexture = new Texture2D(numPoints, numPoints, TextureFormat.RGBA32, false);
        posTexture.filterMode = FilterMode.Point;
        lineMat.SetFloat("_Tile", textureTile);
        //		rend.sharedMaterial = lineMat;
        if (lineTex == null)
            lineTex = lineMat.GetTexture("_SpriteTex") as Texture2D;
        if (shadowTex == null)
            shadowTex = lineTex;
        //		rend.sharedMaterial.SetTexture ("_MainTex", texture);
        //		rend.sharedMaterial.SetTexture ("_SpriteTex", lineTex);
        //		rend.sharedMaterial.SetTexture ("_ShadowTex", shadowTex);
        //		rend.sharedMaterial.SetTexture ("_PosTex", posTexture);
        //		rend.sharedMaterial.SetTexture ("_Offset", renderTex);

        ping.GetComponent<Renderer>().sharedMaterial = pingMat;

        pingMat.SetTexture("_MainTex", posTexture);

        verts = mesh.vertices;
        faces = mesh.triangles;
      
        //		makeEdges ();
        makeSimpleLines();
        //		SetPoints (particlePos);
        makeTexture();

    }

    void meshElements(){
		
		
		count = -1;
		dCount = -1;
		tCount = -1;
		triCount = 0;
	}

	void makeSimpleLines(){
		//

		pointCount = Mathf.Min(65000,numPoints*numPoints * 4);// edges.Count * 4;

//		
//		count = -1;
//		dCount = -1;
//		tCount = -1;
//		triCount = 0;

		meshElements ();
		
//		int particleCount = -1;
//		Verts.Clear ();
		
		if (MMesh == null){
			MFilter.mesh = new Mesh();
			MMesh = MFilter.sharedMesh;
		}

		MMesh.Clear();
		MMesh.vertices = new Vector3[pointCount];
		MMesh.triangles = new int[pointCount*3];
		MMesh.uv = new Vector2[pointCount];
		MMesh.uv2 = new Vector2[pointCount];
		
		Vector2[] uvs = MMesh.uv;
		Vector2[] uvs2 = MMesh.uv2;
		Vector3[] vs = new Vector3[pointCount];
		int[] tris = new int[pointCount * 3];
		
		int h = -1;
		int v = -1;
		int total = -1;
//		foreach (DictionaryEntry entry in edges) {
		for(int j = 0 ; j < numPoints*numPoints ; j++) {
//			int[] b = entry.Value as int[];
			h+=1;
			if(h>numPoints-1){
				h=0;
				v+=1;
			}

			total+=4;
			if(total+4>65000){
				MMesh.vertices = vs;
				MMesh.triangles = tris;
				MMesh.uv = uvs;
				MMesh.uv2 = uvs2;
				
				MMesh.RecalculateNormals();
				MMesh.RecalculateBounds();
				meshElements ();
				makeMesh ();
				connectShaders ();

				MFilter.mesh = new Mesh();
				MMesh = MFilter.sharedMesh;

				MMesh.Clear();
				MMesh.vertices = new Vector3[pointCount];
				MMesh.triangles = new int[pointCount*3];
				MMesh.uv = new Vector2[pointCount];
				MMesh.uv2 = new Vector2[pointCount];
				
				uvs = MMesh.uv;
				uvs2 = MMesh.uv2;
				vs = new Vector3[pointCount];
				tris = new int[pointCount * 3];
				total=-1;

			}

//			Vector3 p = Vector3.zero;//randVec (1);//Vector3.zero;//verts [b [0]];
//			Vector3 s = randVec (1);//;//Vector3.zero;//verts [b [1]];
			
			//make verts to instantiate dots
//			if (!Verts.ContainsKey (getHashedCell (p))) {
//				Vector3 qp = p;
//				qp+=mesh.normals[b[0]].normalized*normalOffset;
//				qp = wireFrame.transform.localToWorldMatrix.MultiplyPoint (qp);
//				Verts.Add (getHashedCell (p), new float[]{qp.x,qp.y,qp.z});
//				++particleCount;
//			}
			
			
			
//			p = wireFrame.transform.localToWorldMatrix.MultiplyPoint (p);
//			s = wireFrame.transform.localToWorldMatrix.MultiplyPoint (s);
			
//			distances [++dCount] = Vector3.Distance (p, s);
			
			
			float off = (float) 1/(numPoints);
			float offset = (float)++dCount*off;
			
			
//			Vector3 start = new Vector3(offset+off*.3f, -1,0);
//			Vector3 start2 = new Vector3(offset+off*.3f,1,0);
//			Vector3 end = new Vector3(offset+off*.7f, -1,0);
//			Vector3 end2 = new Vector3(offset+off*.7f,1,0);

			Vector2 goodUV = new Vector2((float)h/numPoints,(float)v/numPoints);
			Vector3 goodPos = new Vector3(goodUV.x,goodUV.y,0);
			Vector2[] uv2v = new Vector2[]{
				new Vector2(0,0),
				new Vector2(0,1),
				new Vector2(1,0),
				new Vector2(1,1)};
			
//			print (goodUV);
//			print (count);
			vs[++count] = goodPos;//p;
//			p+=mesh.normals[b[0]].normalized*normalOffset;
			uvs[count] = goodUV;//new Vector2((float)h/numPoints,(float)v/numPoints);
			uvs2[count] = uv2v[0];
			vs[++count] = goodPos;//p;
			uvs[count] = goodUV;//new Vector2(offset+off*.1f,1);
			uvs2[count] = uv2v[1];
			
			vs[++count] = goodPos;//;
			uvs[count] = goodUV;//new Vector2(offset+off*.2f,0);
			uvs2[count] = uv2v[2];
//			s+=mesh.normals[b[1]].normalized*normalOffset;
			vs[++count] = goodPos;//;
			uvs[count] = goodUV;//new Vector2(offset+off*.2f,1);
			uvs2[count] = uv2v[3];
			
//			Vector3 vec1 = Vector3.LerpUnclamped(p,s,-1f);
//			Vector3 vec2 = p;
//			Vector3 vec3 = s;
//			Vector3 vec4 = Vector3.LerpUnclamped(p,s,2f);

			Vector3 rando = randVec(1);
			vertPositions.Add(rando);
//			vertPositions.Add(rando);
//			vertPositions.Add(rando);
//			vertPositions.Add(rando);

			tris[++tCount] = triCount+0;
			tris[++tCount] = triCount+1;
			tris[++tCount] = triCount+3;
			
			tris[++tCount] = triCount+0;
			tris[++tCount] = triCount+3;
			tris[++tCount] = triCount+2;
			
			triCount+=4;
			
		}
		
		
		MMesh.vertices = vs;
		MMesh.triangles = tris;
		MMesh.uv = uvs;
		MMesh.uv2 = uvs2;
		
		MMesh.RecalculateNormals();
		MMesh.RecalculateBounds();
		//
		
	}

	Vector3 randVec(float amt){
		return Random.insideUnitSphere * amt;
	}
	
	void makeEdges(){
		edges.Clear ();
		int q = 0;
		int qc = 0;
		for(int i = 0 ; i < faces.Length-1 ; i++){
			if(q<faces.Length-1){
				if(!edges.ContainsKey(hasher(verts, faces[q],faces[q+1]))){
					if(!edges.ContainsKey((hasher(verts, faces[+1],faces[q])))){
						float dist = Vector3.Distance (verts[faces[q]],verts[faces[q+1]]);
						if(dist>minEdgeDistance&&dist<maxEdgeDistance)
							edges.Add(hasher(verts, faces[q],faces[q+1]), new int[]{faces[q],faces[q+1]});
					}
				}
				
				q++;
				qc++;
				if(q>0&&qc==2){
					if(!edges.ContainsKey(hasher(verts, faces[q],faces[q-2]))){
						if(!edges.ContainsKey(faces[(q-2)]+","+faces[q])){
							float dist = Vector3.Distance (verts[faces[q-2]],verts[faces[q]]);
							if(dist>minEdgeDistance&&dist<maxEdgeDistance)
								edges.Add(hasher(verts, faces[q],faces[q-2]), new int[]{faces[q],faces[q-2]});
							
							
						}
					}
				}
				
				if(qc>1){
					qc=0;
					q++;
				}
			}
			
		}
	}
	
	
	float frac(float t){
		return t-Mathf.Floor(t);
	}
	
	public void makeTexture(){
		
		int detail = numPoints;//edges.Count;
		//		detail -= 1;
		//texture.Resize ((int)detail, (int)detail);
		//texture.filterMode = FilterMode.Point;
		//posTexture.Resize ((int)detail, (int)detail);
		//posTexture.filterMode = FilterMode.Point;
//		int on = 0;
//		int off = 0;
//		int q = -1;
		Vector3 vp;
		
		//		print (detail * 4);
//		for (int i = 0; i < detail; i++) {
//
//			
//		}
		for (int i = 0; i < (int)detail; i++) {
			for (int j = 0; j < (int)detail; j++) {
				//vp = vertPositions[i+j*detail];
                Vector3 rCol = Random.insideUnitSphere*.5f;
				//posTexture.SetPixel(i,j,new Color(vp.x,vp.y,vp.z,.1f));
                posTexture.SetPixel(i, j, new Color(rCol.x+.5f,rCol.y+.5f,rCol.z+.5f, 1));

               // float colA = colors[(int)Mathf.Floor(Random.value*(colors.Length-1))];
				//float colC = colA+Random.Range(-colorRandomize,colorRandomize);// Mathf.Lerp(colA,colB,(float)i/detail);
				//texture.SetPixel (i,j,
				//                  new Color(
				//	1,
				//	0,
			//		colC,
			//		1));
			}
		}
		
		//texture.Apply ();
		posTexture.Apply ();
		
	}
	
	public int hasher(Vector3[] verts, int a, int b){
		return getHashedCell(verts[a])+getHashedCell(verts[b]);
	}
	public int getHashedCell(Vector3 pos) {
		int x = Mathf.FloorToInt (pos.x / .1f);
		int y = Mathf.FloorToInt (pos.y / .1f);
		int z = Mathf.FloorToInt (pos.z / .1f);
		return x * h1 + y * h2 + z * h3;
	}

	void updateBounds(){
		Transform camTransform = Camera.main.transform;
		float distToCenter = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2.0f;
		Vector3 center = camTransform.position + camTransform.forward * distToCenter;
		float extremeBound = 5000.0f;
		MeshFilter filter = GetComponent<MeshFilter> ();
		filter.sharedMesh.bounds = new Bounds (center, Vector3.one * extremeBound);
	}
	// Update is called once per frame
	void Update () {
		
		
//		MRend.sharedMaterial.SetFloat ("_Tile", textureTile);
//		MRend.sharedMaterial.SetFloat ("_Speed", speed);
//		MRend.sharedMaterial.SetFloat ("_Saturation", saturation);
//		MRend.sharedMaterial.SetFloat ("_Brightness", brightness);
//		MRend.sharedMaterial.SetFloat ("_ShadowSpeed", shadowSpeed);
//		MRend.sharedMaterial.SetFloat ("_ShadowTile", shadowTile);
//		MRend.sharedMaterial.SetFloat ("_UNPnts", edges.Count*4);
        if(MRend!=null)
		MRend.sharedMaterial.SetFloat ("_LineWidth", lineWidth);
//		MRend.sharedMaterial.SetFloat ("_HueShift", hueShift);
		
//		Transform camTransform = Camera.main.transform;
//		float distToCenter = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2.0f;
//		Vector3 center = camTransform.position + camTransform.forward * distToCenter;
//		float extremeBound = 5000.0f;
//		MeshFilter filter = GetComponent<MeshFilter> ();
//		filter.sharedMesh.bounds = new Bounds (center, Vector3.one * extremeBound);
		
	}
}
