using UnityEngine;
using System.Collections;

public class TextureFromWeb : MonoBehaviour {
	public string url = "http://zellox.com/wp-content/uploads/2016/02/flowers-in-black-and-white.jpg";
	IEnumerator Start() {
		GetComponent<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
		while (true) {
			WWW www = new WWW(url);
			yield return www;
//			www.LoadImageIntoTexture(GetComponent<Renderer>().material.mainTexture);
		}
	}
}