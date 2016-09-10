using UnityEngine;
using System.Collections;

public class webCamTexture : MonoBehaviour {


	// Use this for initialization
	void Start() {
		StartCoroutine (Request ());
//		WebCamTexture webcamTexture = new WebCamTexture();
//		Renderer renderer = GetComponent<MeshRenderer>();
//		renderer.material.mainTexture = webcamTexture;
//		webcamTexture.Play();
	}

	IEnumerator Request() {
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
		if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) {
			WebCamTexture webcamTexture = new WebCamTexture();
			Renderer renderer = GetComponent<MeshRenderer>();
			renderer.material.mainTexture = webcamTexture;
			webcamTexture.Play();
		} else {
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
