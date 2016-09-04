using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LBControl: MonoBehaviour {

	public AudioManagerMic audiM;

	public float audioLevels = 1;
	public float effectLevels = 1;

	public bool autoTransition = true;

	public float videoLow = 0f;
	public float videoHigh = 1f;

	public float videoOscillateSpeed;
	public float videoOscillateAmount;

	public Material videoMat;
	public Material videoMatCheck;

	public float mult = 10;
	float[] nob;
	float[] nob2;

	public GameObject A;
	public GameObject B;
	public GameObject C;
	public GameObject initial;
	public GameObject noise;
	public GameObject wire;

	public GameObject Menu;

	public TextAsset[] texts;

	public string sessionName = "first";

	public GameObject[] things;
	int whichThing = 0;

	bool recording = false;
	bool buffered = false;
	bool playing = false;
	int audioPlayCounter = 0;
	int playCounter = 0;
	float startTime = 0;
	float startBufferTime = 0 ;

	float recordCounter = 0;
	float recordFrequency = .5f;

	Vector3 initInit;
	Dials d;

	bool recordMode = false;

	string[] presets;

	KeyCode[] keyCodes;

    public GameObject controller;
    public GameObject controller2;

    public ViveWandControl ViveWand;

	public float settingsCounter = 1;
	float settingsTime = 0;
	float settingsLerp = 0;
	int settingsWhich = 0;

	void Awake () {
		
//		Application.targetFrameRate = 30;


		keyCodes = new KeyCode[] {
			KeyCode.Alpha0,
			KeyCode.Alpha1,
			KeyCode.Alpha2,
			KeyCode.Alpha3,
			KeyCode.Alpha4,
			KeyCode.Alpha5,
			KeyCode.Alpha6,
			KeyCode.Alpha7,
			KeyCode.Alpha8,
			KeyCode.Alpha9
		};
		presets = new string[9];
		d = Dials.Instance;
		d.mult = mult;
//		Application.targetFrameRate = 60;
		initInit = initial.transform.position;
		nob = new float[100];
		nob2 = new float[100];
		switch3DObject (0);

//		for (int i = 0; i < 8; i++) {
//			presets [i] = (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
//			print (presets [i]);
////			d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
//		}
		presets [1] = "0,0.5922341,0,0,8.897638,2.917476,9.683504,0,9.215755,0,0,0,0,10,0,0,2.519684,0,0,0,0,0,0,0,0,0,0,,2.362213,6.028938,0,0,0,7.638378,0,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets [2] = "0,0.5109644,0.6299213,0.8194852,4.676285,3.289375,0,1.811023,9.574375,0,0.3149605,0.5511856,0,9.890175,10,9.067984,10,0,0,0,0,0,0,0,0,0,0,,3.070869,0,8.893852,0,0,0,0,0,0,0.5312157,9.212589,6.692905,1.023617,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets [3] = "0,3.333145,0.6299213,0,4.330711,9.92126,8.581238,0.1574802,10,0,0.3149606,0.5511811,5.905514,0,10,10,0,0,0,0,0,0,0,0,0,0,0,,3.070866,0,5.196857,0,0,0,0,0,0,0.5254173,9.212598,6.692913,1.023622,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets[4] = "0,1.552517,0,0,8.897638,8.844385,0,0,9.886007,0,0,0,0,10,0,0,2.519684,0,0,0,0,0,0,0,0,0,0,,2.362213,6.028938,0,0,0,0,0,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets[5] = "0,0.5793357,0.4843688,0,8.897638,8.844385,0,0,1.045694,0,0,0,0,10,9.000654,0,8.252735,0,0,0,0,0,0,0,0,0,0,,2.362213,8.620224,0,0,0,3.135395,0,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets[6] = "0,0.5793357,0.4843688,0,8.897638,9.479952,0,0,8.338823,0,0,0,0,10,9.000654,0,8.252735,0,0,0,0,0,0,0,0,0,0,,2.362213,8.620224,0,0,0,3.135395,6.7202,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets[7] = "0,0.5793357,0.4843688,5.740736,8.897638,9.479952,0,0,9.553757,0,0,0,0,0,9.000654,0,8.252735,0,0,0,0,0,0,0,0,0,0,,2.362213,1.962032,0,0,0,1.177502,0.8564758,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		presets[8] =  "0,0.546062,0.4843688,0,8.825817,5.703783,0,0,3.662252,0,0,0,0,9.396534,9.509058,9.662819,8.100395,0,0,0,0,0,0,0,0,0,0,,0,1.298008,0,0.3359795,0,2.051105,6.7202,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
		d.checkDials (true);
		d.readDials (presets [1]);

	}


	//get knob
	float gn(int a, float w,float m){
		float r = 0;
		if (!playing)
			r = 1 + (audiM.GetBands (new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}) * w * m);
		else if (playCounter < d.volumeBuffer.Length)
			r = 1 + d.volumeAudioBuffer [audioPlayCounter];//* w * m;
		return Mathf.Lerp(1, r, effectLevels) * audioLevels;
	}

	void switch3DObject(float which){
		whichThing = (int)which;
		if (whichThing > things.Length)
			whichThing = 0;
		print (whichThing);
		for(int j = 0 ; j < things.Length ; j++){
			things[j].SetActive(false);
		}
		things[whichThing].SetActive(true);
		C = things [whichThing];
	}

	void objSwitcher(){
		for (int i = 0; i < things.Length; i++) {
			if(d.buttons[0,i]>.5f)
				switch3DObject (i);
		
		}
		if (Input.GetKeyUp(KeyCode.W) ) {
			switch3DObject (++whichThing);
		}
	}

	public void setHighVideo(float high){
		videoHigh = high;
	}
	public void setLowVideo(float low){
		videoLow = low;
	}

	public void SetVideoOscillateSpeed(float high){
		videoOscillateSpeed = high;
	}
	public void SetvideoOscillateAmount(float low){
		videoOscillateAmount = low;
	}

	public void setEffectLevels(float effect){
		effectLevels = effect;	
	}

	public void setAudioLevel(float audio){
		audioLevels = audio;	
	}

	void updateLevels(){
		A.GetComponent<MeshRenderer>().sharedMaterial.SetVector ("_VideoLevels", new Vector4 (videoLow, videoHigh, videoOscillateSpeed, videoOscillateAmount));
		videoMatCheck.SetVector ("_VideoLevels", new Vector4 (videoLow, videoHigh, videoOscillateSpeed, videoOscillateAmount));
	}

	void settingsSwitch(){
		settingsTime += Time.deltaTime;
		if (settingsTime > settingsCounter) {
			settingsTime = 0;
			settingsWhich++;
			if (settingsWhich > presets.Length-1) {
				settingsWhich = 0;
			}
			d.readDials (presets [settingsWhich]);
		}

	}

	public void triggerAutoTransition(bool which){
		autoTransition = which;
	}

	public void setSettingsCounter(float value){
		settingsCounter = value;
	}


	void Update () {

		updateLevels ();
		if(autoTransition)
			settingsSwitch ();

		if(!playing)
			d.checkDials (false);
		if ( Input.GetKeyUp( KeyCode.U) && recordMode || MidiInput.GetKnob (45,MidiInput.Filter.Realtime) > .5f) {
			recordMode = false;
			Debug.Log (recordMode);
		}
		else if (Input.GetKeyUp( KeyCode.I) || MidiInput.GetKnob (44,MidiInput.Filter.Realtime) > .5f) {
			recordMode = true;
			Debug.Log (recordMode);
		}
		if (MidiInput.GetKnob (47, MidiInput.Filter.Realtime) > .5f) {
			for (int i = 0; i < 8; i++) {
				string s = texts [i].text;// System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt");
				print (s);
				presets [i] = s;//(System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
				d.readDials ( s);//(System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
			}
		}
//		if (Input.GetKeyUp( KeyCode.L) || MidiInput.GetKnob (48, MidiInput.Filter.Realtime) > .5f) {
//			for (int i = 0; i < 8; i++) {
//				System.IO.File.WriteAllText ("Assets/athon/data/data_" + sessionName + i + ".txt", presets[i]);
//			}
//			Debug.Log ("saved");
//		}
//		if (Input.any) {
			for (int i = 0; i < 9; i++) {
//				Debug.Log (keyCodes [i]);
				if (Input.GetKeyUp (keyCodes [i])) {
					if (recordMode) {
						presets [i] = d.recordDials ();
					} else {
						d.readDials (presets [i]);
					if(Menu.transform.GetChild(0).gameObject.activeInHierarchy)
							Menu.transform.GetChild(0).transform.GetChild(1). GetComponent<UIMidi> ().setSliders ();
					}
					Debug.Log (presets [i]);
				}
			}
//		}

		if (Input.GetKeyUp (KeyCode.R)) {
			recording = !recording;
			audiM.Play ();
//			d.checkDials (true);
			Debug.Log (recording);
		}
		if (Input.GetKeyUp (KeyCode.M)) {
			if (!Menu.activeInHierarchy)
				Menu.SetActive (true);
			else
				Menu.SetActive (false);
		}
		if (recording) {
			recordCounter += Time.deltaTime;
			if (recordCounter > recordFrequency) {
				recordCounter = 0;
				d.makeBuffer (audiM.GetBands (new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }));
			}
			d.makeAudioBuffer(audiM.GetBands (new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }));
		}
//		if (Input.GetKeyUp (KeyCode.S)) {
//			System.IO.File.WriteAllText ("Assets/athon/data/session_" + sessionName + ".txt",d.buffer);
//			System.IO.File.WriteAllText ("Assets/athon/data/sessionAudio_" + sessionName + ".txt",d.audioBuffer);
//		}
		if (Input.GetKeyUp (KeyCode.P)) {
			if (!buffered) {
//				d.readBuffer(System.IO.File.ReadAllText ("Assets/athon/data/session_" + sessionName + ".txt"));
//				d.readAudioBuffer(System.IO.File.ReadAllText ("Assets/athon/data/sessionAudio_" + sessionName + ".txt"));
				buffered = true;
			}
			playing = !playing;
			startTime = Time.time;
			startBufferTime = d.timeBuffer [0];
			audiM.Play ();
		}
		if (playing) {
			if (playCounter < d.timeBuffer.Length - 1) {
				if (Time.time - startTime > d.timeBuffer [playCounter] - startBufferTime) {
					float diff = (d.timeBuffer[playCounter] - startBufferTime - Time.time - startTime) /
						(d.timeBuffer[playCounter] - startBufferTime - 
							d.timeBuffer[playCounter-1] - startBufferTime);
					d.readDials (d.presetBuffer [playCounter-1], d.presetBuffer [playCounter], diff);
				}
				while (d.timeBuffer [playCounter] - startBufferTime < Time.time - startTime) {
					playCounter++;
				}
				while (d.timeAudioBuffer [audioPlayCounter] - startBufferTime < Time.time - startTime) {
					audioPlayCounter++;
				}
			} else {
				playing = false;
				Debug.Log ("playtime is OVER!");
				//Camera.main.gameObject.GetComponent<CaptureStandard> ().enabled = false;
			}
			if (Input.GetKeyUp (KeyCode.O)) {
				playing = !playing;
				//Camera.main.gameObject.GetComponent<CaptureStandard> ().enabled = false;
			}
		}
		if (Input.GetKeyUp (KeyCode.B)) {
			audiM.Play ();
		}
				
		float t = Time.deltaTime;

		//Camera.main.transform.parent.transform.localPosition = new Vector3 (0, 0, Mathf.Pow(d.knobs[1,2]*.1f,3) * -5f);
		//Camera.main.transform.parent.transform.parent.transform.Rotate (0, d.knobs[1,3] * -.1f*t*60,0);

		objSwitcher ();
		things [whichThing].transform.Rotate (d.knobs[1,4] * -.1f * t * 60, d.knobs[1,5] * -.1f * t * 60, d.knobs[1,6] * -.1f * t * 60);

		noise.GetComponent<Renderer> ().sharedMaterial.SetColor ("_Color", new Color (1, 1, 1, d.dials[0,0]*.1f*(1f+gn (1,d.knobs[0,0],10)) ));
		A.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Amount",  d.dials[0,1]*gn (2,d.knobs[0,1],10)*.01f*(1+d.dials [0,2]*2f)*gn (3,d.knobs[0,2],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Speed", d.dials[0,3]*.1f*gn (4,d.knobs[0,3],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Freq", d.dials[0,4]*.01f*gn (5,d.knobs[0,4],10)*(1+d.dials[0,5])*3f*gn (6,d.knobs[0,5],10) );
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Which", d.dials[0,6]*.1f );

		for (int i = 0; i < 8; i++) {
			if (d.buttons [1, i] > .3f) {
				if (recordMode) {
					presets [i] = d.recordDials ();
//					System.IO.File.WriteAllText ("Assets/athon/data/data_" + i + ".txt", d.recordDials ());
					print ("Saved: " + i);
				} else {
					d.readDials (presets [i]);
//					d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
				}
			}

		}
		
			
		C.GetComponent<Renderer>().sharedMaterial.SetVector ("_Pos", 
			new Vector4(
//                controller.transform.position.x,
//                controller.transform.position.y,
//                controller.transform.position.z,
                Mathf.Sin(Mathf.Pow(d.dials[1,0],2)*Time.time*3f)*d.dials[1,3]*.2f,
                Mathf.Cos(Mathf.Pow(d.dials[1,1],2)*Time.time*3f)*d.dials[1,3]*.2f,
                Mathf.Sin(Mathf.Pow(d.dials[1,2],2)*Time.time*3f)*d.dials[1,3]*.2f,
                0 ));
//        C.GetComponent<Renderer>().sharedMaterial.SetVector("_Pos2",
//            new Vector4(
//                controller2.transform.position.x,
//                controller2.transform.position.y,
//                controller2.transform.position.z,
//                0));
        C.GetComponent<Renderer>().sharedMaterial.SetVector ("_Speeds", 
			new Vector4(
				d.dials[1,5],
				d.dials[1,6],
				d.dials[1,7],0 ));
//		initial.GetComponent<Renderer>().sharedMaterial.SetColor ("_Color",new Color(1,1,1, d.dials [0,7]*.1f*gn (8,d.knobs[0,7],10)  ));
		C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_SinAdd", d.dials[0,8]*.1f*gn (9,d.knobs[0,8],10) );
      
		C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Gravity", d.dials[1, 4] * .1f);
//        if (ViveWand.click)
//		    C.GetComponent<Renderer>().sharedMaterial.SetFloat ("_Gravity", 1 );
//        else
//            C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Gravity", 0);
//		
//        Debug.Log(ViveWand.click);
        wire.GetComponent<wireFrameAthon>().lineWidth = d.knobs[1,0] * .005f;
		wire.GetComponent<Renderer>().sharedMaterial.SetColor("_Color",new Color(1,1,1, d.knobs[1,1] * .02f));


	}
}
