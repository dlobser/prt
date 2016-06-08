using UnityEngine;
using System.Collections;

public class blitter : MonoBehaviour {
	public Texture aTexture;
	public RenderTexture rTex;
	void Start() {
		if (!aTexture || !rTex)
			Debug.LogError("A texture or a render texture are missing, assign them.");
		
	}
	void Update() {
		Graphics.Blit(aTexture, rTex);
	}
}