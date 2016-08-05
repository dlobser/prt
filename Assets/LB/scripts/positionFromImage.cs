using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class positionFromImage : MonoBehaviour {

	public Texture2D tex;
	Texture2D newTex;
	public GameObject mat;
	public int resolution = 1024;
	List<Vector3> xy;
	public float depth = 0;


	public string url;

	void Start () {
		xy = new List<Vector3> ();
		setupNewTex ();
		Debug.Log (url.Length);
		if (url.Length>0) {
			StartCoroutine( loadURL() );
		}
		else
			makePixels ();


	}

	IEnumerator loadURL() {
//		while (true) {
			WWW www = new WWW(url);
			yield return www;
			www.LoadImageIntoTexture(newTex);
			makePixels ();

//		}
	}

	void setupNewTex(){
		int width =  resolution;
		int height = resolution;
		newTex = new Texture2D (width, height, TextureFormat.RGBAFloat,false);
		newTex.filterMode = FilterMode.Point;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				newTex.SetPixel (i, j, tex.GetPixel (i, j));
			}
		}
		newTex.Apply ();
	}

	void makePixels(){

//		tex.Resize (resolution, resolution);
		int width = tex.width;
		int height = tex.height;
		Texture2D newTex2 = new Texture2D (width, height, TextureFormat.RGBAFloat,false);
		newTex2.filterMode = FilterMode.Point;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				Color col = newTex.GetPixel (i, j);
				if (col.r > .1f)
					xy.Add (new Vector3 (i, j, col.r));
			}
		}
		int count = -1;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				Vector3 vec = xy [++count];
				Vector2 rand = new Vector2 (Random.value / (float)width, Random.value / (float)height);
				if (count > xy.Count - 2)
					count = -1;
				newTex2.SetPixel(i,j,new Color((float)vec.x/(float)width +rand.x,(float)vec.y/(float)height +rand.y,vec.z * depth,0));
			}
		}
		newTex2.Apply ();
		mat.GetComponent<MeshRenderer>().material.SetTexture("_MainTex",newTex2);

//		byte[] bytes = newTex.EncodeToPNG();
//		File.WriteAllBytes("Assets/saved.png", bytes);

	}
	

}
