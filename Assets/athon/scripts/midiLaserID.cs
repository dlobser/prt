using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class midiLaserID : MonoBehaviour {

	public AudioManagerMic audiM;

	public float mult = 10;
	float[] nob;
	float[] nob2;

	public GameObject A;
	public GameObject B;
	public GameObject C;
	public GameObject initial;
	public GameObject noise;
	public GameObject wire;

    public Transform tracker;

	public GameObject[] things;
	int whichThing = 0;

	Vector3 initInit;
	Dials d;

	bool recordMode = true;

	Vector3 rotAdd = Vector3.zero;

	int whichData = 0;

	string[] presets;

	void Start () {
		presets = new string[8];
		d = Dials.Instance;
		d.mult = mult;
		Application.targetFrameRate = 60;
		initInit = initial.transform.position;
		nob = new float[100];
		nob2 = new float[100];
		switch3DObject (0);
		for (int i = 0; i < 8; i++) {
			d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
		}
		d.checkDials (true);
			
		checkButtons ();
		d.readDials (presets [0]);

	}

	float gn(int a, float w,float m){
		float r = 1 + 0;//(audiM.GetBands (a) * w * m);
		return r;
	}

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
			if(d.buttons[0,i]>.5f)
				switch3DObject (i);
			//			print (MidiInput.GetKey (i + 23)+" , " +  (i+ 23));
		}
	}

	void checkButtons(){
		if (MidiInput.GetKnob (45,MidiInput.Filter.Realtime) > .5f) {
			recordMode = false;
			print (recordMode);
		}
		if (MidiInput.GetKnob (44,MidiInput.Filter.Realtime) > .5f) {
			recordMode = true;
			print (recordMode);
		}
		if (MidiInput.GetKnob (47, MidiInput.Filter.Realtime) > .5f) {
			for (int i = 0; i < 8; i++) {
				string s = System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt");
				print (s);
				presets[i] = (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
				d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
			}
		}
		if (MidiInput.GetKnob (48, MidiInput.Filter.Realtime) > .5f) {
			for (int i = 0; i < 8; i++) {
				System.IO.File.WriteAllText ("Assets/athon/data/data_" + i + ".txt", presets[i]);
				
			}
		}
	}
	void Update () {
		d.checkDials (false);

		checkButtons ();
//		print (MidiInput.GetKnob (14));
				
		float t = Time.deltaTime;
//
//		for (int i = 0; i < nob.Length; i ++) {
//			nob[i] = MidiInput.GetKnob(i+14,MidiInput.Filter.Realtime)*mult;
//			nob2[i] = MidiInput.GetKnob(i,MidiInput.Filter.Realtime)*mult;
//		}

		Camera.main.transform.localPosition = new Vector3 (0, 0, Mathf.Pow(d.knobs[1,2]*.1f,2) * -5f);
		Camera.main.transform.parent.transform.Rotate (0, d.knobs[1,3] * -.1f*t*60,0);

		objSwitcher ();
		things [whichThing].transform.Rotate (d.knobs[1,4] * -.1f * t * 60, d.knobs[1,5] * -.1f * t * 60, d.knobs[1,6] * -.1f * t * 60);
		//		print (nob2 [9]+","+nob2[12]);

		//2,3,4,5,6,8,9,12,13
		//42,43,50,51,52,53,54,55,56
		int q = -1;
		int qq = 1;

		noise.GetComponent<Renderer> ().sharedMaterial.SetColor ("_Color", new Color (1, 1, 1, d.dials[0,0]*.1f*(1f+gn (1,d.knobs[0,0],10)) ));
		A.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Amount",  d.dials[0,1]*gn (2,d.knobs[0,1],10)*.01f*(1+d.dials [0,2]*2f)*gn (3,d.knobs[0,2],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Speed", d.dials[0,3]*.1f*gn (4,d.knobs[0,3],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Freq", d.dials[0,4]*.01f*gn (5,d.knobs[0,4],10)*(1+d.dials[0,5])*3f*gn (6,d.knobs[0,5],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Which", d.dials[0,6]*.1f );

//		for (int i = 0; i < 8; i++) {
//			if (d.buttons [1, i] > .3f) {
//				print (d.buttons [1, i]);
//				print (presets [i]);
//			}
//		}
		for (int i = 0; i < 8; i++) {
			if (d.buttons [1, i] > .3f) {
				//d.checkDials (true);
//				print (i);
//				print (presets [i]);
				if (recordMode) {
					presets [i] = d.recordDials ();
//					System.IO.File.WriteAllText ("Assets/athon/data/data_" + i + ".txt", d.recordDials ());
					print ("Saved: " + i);
				} else {
					d.readDials (presets [i]);
//					print (presets [i]);
//					print (i);
//					d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
				}
			}

		}
		
			
		C.GetComponent<Renderer>().sharedMaterial.SetVector ("_Pos", 
			new Vector4(
                tracker.position.x,
                tracker.position.y,
                tracker.position.z,
				//Mathf.Sin(Mathf.Pow(d.dials[1,0],2)*Time.time*3f)*d.dials[1,3]*.2f,
				//Mathf.Cos(Mathf.Pow(d.dials[1,1],2)*Time.time*3f)*d.dials[1,3]*.2f,
				//Mathf.Sin(Mathf.Pow(d.dials[1,2],2)*Time.time*3f)*d.dials[1,3]*.2f,
        0 ));
		C.GetComponent<Renderer>().sharedMaterial.SetVector ("_Speeds", 
			new Vector4(
				d.dials[1,5],
				d.dials[1,6],
				d.dials[1,7],0 ));
		initial.GetComponent<Renderer>().sharedMaterial.SetColor ("_Color",new Color(1,1,1, d.dials [0,7]*.1f*gn (8,d.knobs[0,7],10)  ));
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_SinAdd", d.dials[0,8]*.1f*gn (9,d.knobs[0,8],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Gravity", d.dials[1,4]*.1f );

		wire.GetComponent<wireFrameAthon>().lineWidth = d.knobs[1,0] * .005f;
		wire.GetComponent<Renderer>().sharedMaterial.SetColor("_Color",new Color(1,1,1, d.knobs[1,1] * .02f));


	}
}
