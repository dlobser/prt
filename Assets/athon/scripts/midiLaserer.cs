using UnityEngine;
using System.Collections;

public class midiLaserer : MonoBehaviour {

	public AudioManagerMic audiM;

	GameObject[] kids;
	public float mult = 10;
	float[] nob;
	float[] nob2;
	public AudioClip aud;
	public TrailRenderer[] trails;

	public GameObject A;
	public GameObject B;
	public GameObject C;
	public GameObject initial;
	public GameObject noise;
	public GameObject wire;

	public GameObject[] things;
	int whichThing = 0;

	Vector3 initInit;

	Vector3 rotAdd = Vector3.zero;
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		initInit = initial.transform.position;
//		kids = new GameObject[3];
		nob = new float[100];
		nob2 = new float[100];
		switch3DObject (0);
//		for (int i = 0; i < nob.Length; i ++) {
//			nob[i] = i+14;
//		}
//		kids [0] = transform.gameObject;
//		kids [1] = transform.GetChild (0).gameObject;
//		kids [2] = transform.GetChild (0).transform.GetChild (0).gameObject;
		
		//		addAudio (0);
		//		addAudio (1);
//		addAudio (2);
		
		
	}
	
	void addAudio(int i){
		AudioSource audi = kids [i].AddComponent<AudioSource> ();
		audi.clip = aud;
		audi.Play ();
		audi.loop = true;
		audi.spatialBlend = 1.0f;
		audi.dopplerLevel = 1;
		audi.rolloffMode = AudioRolloffMode.Custom;
		audi.maxDistance = 10;
		audi.minDistance = 5;
	}
//
	float gn(int a, int w,float m){
		float r = 1 + (audiM.GetBands (a) * nob2 [w] * m);
		return r;
	}

//	int[] getArray(int a, int b){
//		int ba = b - a;
//		int[] r = new int[ba+1];
//
//		for(int i = 0; i <= ba ; i++){
//			r[i]=
//		}
//	}
	// Update is called once per frame

	void switch3DObject(float which){
//		for (int i = 0; i < things.Length; i++) {
//			if(which>.5f){
				whichThing = (int)which;
			print (whichThing);
				for(int j = 0 ; j < things.Length ; j++){
					things[j].SetActive(false);
				}
				things[whichThing].SetActive(true);
//			}
//		}
	}

	void objSwitcher(){
		for (int i = 0; i < things.Length; i++) {
			if(MidiInput.GetKey (i + 23)>.5f)
				switch3DObject (i);
//			print (MidiInput.GetKey (i + 23)+" , " +  (i+ 23));
		}
	}

	void Update () {
		float t = Time.deltaTime;
		
		for (int i = 0; i < nob.Length; i ++) {
			nob[i] = MidiInput.GetKnob(i+14,MidiInput.Filter.Realtime)*mult;
			nob2[i] = MidiInput.GetKnob(i,MidiInput.Filter.Realtime)*mult;
			//			print (nob[i]);
		}
		for (int i = 0; i < trails.Length; i++) {
			trails[i].time = nob2[3];
			trails[i].startWidth =( nob2[2]*.4f)+.05f;
		}
		Camera.main.transform.localPosition = new Vector3 (0, 0, Mathf.Pow(nob2 [50]*.1f,2) * -5f);
		Camera.main.transform.parent.transform.Rotate (0, nob2 [51] * -.1f*t*60,0);

		objSwitcher ();
		things [whichThing].transform.Rotate (nob2 [52] * -.1f * t * 60, nob2 [53] * -.1f * t * 60, nob2 [54] * -.1f * t * 60);
		//		print (nob2 [9]+","+nob2[12]);

		//2,3,4,5,6,8,9,12,13
		//42,43,50,51,52,53,54,55,56
		int q = -1;
		int qq = 1;
//		print (audiM.GetBands(new int[]{1,2,3,4,5,6}));
//		initial.transform.position = new Vector3 (0, 0, nob [++q])+initInit;
		noise.GetComponent<Renderer> ().sharedMaterial.SetColor ("_Color", new Color (1, 1, 1, nob [++q]*.1f*(1f+gn (1,2,10)) ));
		A.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Amount", nob [++q]*gn (2,3,10)*.01f*(1+nob [++q]*2f)*gn (3,4,10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Speed", nob [++q]*.1f*gn (4,5,10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Freq", nob [++q]*.01f*gn (5,6,10)*(1+nob [++q])*3f*gn (6,8,10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Which", nob [++q]*.1f );

		C.GetComponent<Renderer>().sharedMaterial.SetVector ("_Pos", 
			new Vector4(
				Mathf.Sin(nob2[57]*Time.time*3f)*nob2[60]*.2f,
				Mathf.Cos(nob2[58]*Time.time*3f)*nob2[60]*.2f,
				Mathf.Sin(nob2[59]*Time.time*3f)*nob2[60]*.2f,0 ));
		C.GetComponent<Renderer>().sharedMaterial.SetVector ("_Speeds", 
			new Vector4(
				nob2[62]*.4f,
				nob2[63]*.4f,
				nob2[65]*.4f,0 ));
		initial.GetComponent<Renderer>().sharedMaterial.SetColor ("_Color",new Color(1,1,1, nob [++q]*.1f*gn (8,12,10)  ));
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_SinAdd", nob [++q]*.1f*gn (9,13,10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Gravity", nob2[61]*.1f );

//		print (nob2 [2]);
		wire.GetComponent<wireFrameAthon>().lineWidth = nob2 [42] * .005f;
		wire.GetComponent<Renderer>().sharedMaterial.SetColor("_Color",new Color(1,1,1, nob2 [43] * .01f));
//		print (nob [57 - 14]);
//		Vector3 baseRot = new Vector3 (nob [q] * t * 60, nob [++q] * t * 60, nob [++q] * t * 60);
//		kids[0].transform.parent.transform.parent.transform.Rotate (baseRot);
//		kids[0].transform.parent.transform.Rotate (new Vector3 (nob [++q]*t*60, nob[++q]*t*60, nob[++q]*t*60));
//		kids[0].transform.localPosition = new Vector3 (0,15.5f+nob2[6],0);
//		//		kids [0].GetComponent<AudioSource> ().pitch = (nob [0]*.1f)+.8f;
//		kids[1].transform.Rotate (new Vector3 ( 0,nob[++q]*t*60,0));
//		kids[1].transform.localPosition = new Vector3 (0,nob2[6]*2,0);
//		//		kids [1].GetComponent<AudioSource> ().pitch = (nob [3]*.1f)+.8f;
//		kids[2].transform.Rotate (new Vector3 (nob [++q]*t*60, nob[++q]*t*60, 0));
//		kids[2].transform.localPosition = new Vector3 (nob2[13],nob2[6]*2,0);
//		kids[2].transform.localScale = new Vector3 (.1f+nob2[8]*.2f,.1f+nob2[8]*.2f,.1f+nob2[8]*.2f);
//		kids [2].GetComponent<AudioSource> ().pitch = (nob [19]*.1f)+.8f;
//		kids [2].GetComponent<AudioSource> ().maxDistance = (nob2 [9]*3)+10;
//		kids [2].GetComponent<AudioSource> ().dopplerLevel = (nob2 [12]*.5f);
		
		
		
	}
}
