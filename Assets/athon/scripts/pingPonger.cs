using UnityEngine;
using System.Collections;

public class pingPonger : MonoBehaviour {

	public GameObject pos;
	public GameObject vel;

	public GameObject disp;

	public RenderTexture[] posTex;
	public RenderTexture[] velTex;

	public Camera velCam;
	public Camera posCam;

	int buffer,a,b = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (buffer == 1) {
			buffer = 0;
			a = 1;
			b = 0;
		} else {
			buffer = 1;
			a = 0;
			b = 1;
		}

		vel.GetComponent<Renderer> ().sharedMaterial.SetTexture ("_MainTex", posTex [a]);
		vel.GetComponent<Renderer> ().sharedMaterial.SetTexture ("_Velocity", velTex [a]);
		velCam.targetTexture =velTex [b];
		velCam.Render ();
		vel.GetComponent<Renderer> ().sharedMaterial.SetTexture ("_MainTex", posTex [a]);
		vel.GetComponent<Renderer> ().sharedMaterial.SetTexture ("_Velocity", velTex [b]);
		posCam.targetTexture =posTex [b];
		posCam.Render ();
		disp.GetComponent<Renderer> ().sharedMaterial.SetTexture ("_MainTex", posTex [b]);
	}
}

/*
 *      velUniforms.velTex.value = velTexture[a];
        velUniforms.posTex.value = posTexture[a];

        renderer.render(velScene, processCamera, velTexture[b]);

        posUniforms.velTex.value = velTexture[b];
        posUniforms.posTex.value = posTexture[a];

        renderer.render(posScene, processCamera, posTexture[b]);

        dispUniforms.posTex.value = posTexture[b];

//        renderer.setSize(dispSize, dispSize, false);
        renderer.setViewport(0,0,dispSize.x, dispSize.y);
*/