using UnityEngine;
using System.Collections;

public class CameraDifference : MonoBehaviour {

	private WebCamTexture webcamTexture;

	public RenderTexture cameraBuffer;
	public RenderTexture cameraDifference;

	public Material differenceMaterial;

	void Start() {

		webcamTexture = new WebCamTexture();
		webcamTexture.Play();
	}
	
	void Update () {

		if (webcamTexture.didUpdateThisFrame) {
//			differenceMaterial.SetTexture ("_BufferTex", cameraBuffer);
			Graphics.Blit (webcamTexture, cameraDifference, differenceMaterial);
			Graphics.Blit (webcamTexture, cameraBuffer);
		}
	}
}
