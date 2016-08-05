using UnityEngine;
using System.Collections;

public class webCamTexture2 : MonoBehaviour {


	// Use this for initialization
	void Start() {
		WebCamTexture webcamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<MeshRenderer>();
		renderer.material.SetTexture("_NoiseTex",  webcamTexture);
		webcamTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
