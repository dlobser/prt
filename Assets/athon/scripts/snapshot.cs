using UnityEngine;
using System.Collections;

public class snapshot : MonoBehaviour {
	
	public Camera cam;
	public int horizontalRes;
	public bool saveLocal = false;
	public string testFile = "Assets/testScreenShot.png";

	void Update(){
		if (Input.GetKeyDown (KeyCode.R)) {
			takeSnapshot ();
		}
	}

	public void takeSnapshot(){
		
		int width = horizontalRes;
		int height = horizontalRes;//(int)(horizontalRes*(9f/16f));
		RenderTexture renderTex = new RenderTexture(width,height,24);
		cam.targetTexture = renderTex;
		cam.targetTexture.filterMode = FilterMode.Point;
		Texture2D screenShot = new Texture2D(width,height, TextureFormat.RGB24, false);
		cam.Render();
		RenderTexture.active = renderTex;
		screenShot.ReadPixels(new Rect(0, 0, width,height), 0, 0);
		cam.targetTexture = null;
		RenderTexture.active = null;
		if(saveLocal){
			byte[] bytes = screenShot.EncodeToPNG();
			string filename = testFile;
			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));

		}
		
	}
}
