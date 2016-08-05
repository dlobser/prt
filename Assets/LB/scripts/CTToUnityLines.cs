using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTToUnityLines : MonoBehaviour {

	public int lines;
	public int detail;
	LineRenderer[] lineRenderers;
	public float lineWidthStart;
	public float lineWidthEnd;
	public Material mat;

	public string lineArray = "[0,0,0,0,0,1,0,0,1,1,0,0,1,0,0,0],[0,0,1,0,0,1,1,0,1,1,1,0,1,0,1,0]";
	string lineCopy = "";

	List<float[]> linesToUpdate;


	void Start () {
		makeLineRenderers ();
//		populateTestLines ();
		populateLines ();
		updateLines ();

	}
	
	void Update () {
		if (!lineCopy.Equals (lineArray)) {
			populateLines ();
			updateLines ();
		}
		lineCopy = lineArray.Clone () as string;
	}



	void updateLines(){
		for (int i = 0; i < this.transform.childCount; i++) {
			LineRenderer rend = this.transform.GetChild (i).gameObject.GetComponent<LineRenderer> ();
			if (i < linesToUpdate.Count) {
				if (linesToUpdate [i] != null) {
					rend.SetVertexCount (linesToUpdate [i].Length / 4);
					for (int j = 0; j < linesToUpdate [i].Length / 4; j++) {
						rend.SetPosition (j, new Vector3 (linesToUpdate [i] [(j * 4)], i + linesToUpdate [i] [(j * 4) + 1], linesToUpdate [i] [(j * 4) + 2]));
					}
				}
			}else
				rend.SetVertexCount (0);
		}
	}

	void populateLines(){
		
		linesToUpdate = new List<float[]> ();

		List<string[]> stringArrays = new List<string[]> ();

		string[] stringsChopped = lineArray.Split (new string[]{  "],[" , "[" , "]" }, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < stringsChopped.Length; i++) {
			string[] ln = stringsChopped[i].Split (new string[]{ "," }, System.StringSplitOptions.RemoveEmptyEntries);
			stringArrays.Add (ln);
		}

		for (int i = 0; i < stringArrays.Count; i++) {
			float[] stringToFloat = new float[stringArrays [i].Length];
			for (int j = 0; j < stringArrays[i].Length; j++) {
				stringToFloat [j] = float.Parse (stringArrays [i] [j]);

			}
			linesToUpdate.Add (stringToFloat);
		}
	}
//
//	float[] makeTestLine(int count){
//		float[] line = new float[count*4];
//		for (int i = 0, c = 0; i < count; i++) {
//			for (int j = 0; j < 4; j++,c++) {
//				line [c] = Mathf.PerlinNoise (i * .5f, j * .5f) + i * .2f;
//				if (j == 3)
//					line [c] = 0f;
//			}
//		}
//		return line;
//	}
//
//	void populateTestLines(){
//		linesToUpdate = new List<float[]> ();
//		for (int i = 0; i < lines; i++) {
//			linesToUpdate.Add (makeTestLine (detail));
//		}
//	}

	void makeLineRenderers(){
		lineRenderers = new LineRenderer[lines];
		for (int i = 0; i < lineRenderers.Length; i++) {
			GameObject G = new GameObject ();
			lineRenderers [i] = G.AddComponent<LineRenderer> ();
			lineRenderers [i].material = mat;
			lineRenderers [i].SetWidth (lineWidthStart, lineWidthEnd);
			G.transform.parent = this.transform;
		}

	}
}
