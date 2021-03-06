﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class AngelControl : MonoBehaviour {

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

	bool recordMode = true;

	string[] presets;

	KeyCode[] keyCodes;

    public GameObject controller;
    public GameObject controller2;

    public GameObject tracker1;
    public GameObject tracker2;

    public ViveWandControl ViveWand;

	bool triggered = false;

	public GameObject title;

    float prevSpeed1;
    float prevSpeed2;

    float titleAlpha = 1;

    bool meshMade = false;
    
//	public ViveWandControl V1;
//	public ViveWandControl V2;

	void Start () {

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
		presets = new string[8];
		d = Dials.Instance;
		d.mult = mult;
		Application.targetFrameRate = 60;
		initInit = initial.transform.position;
		nob = new float[100];
		nob2 = new float[100];
		switch3DObject (0);

		//for (int i = 0; i < 8; i++) {
			//presets [i] = (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
//			print (presets [i]);
//			d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
		//}
		d.checkDials (true);
		d.readDials ("0,1.340679,0.629921,0,8.897638,5.538507,0,9.479017,0,0,0,0,0,10,0,0,2.519684,0,0,0,0,0,0,0,0,0,0,,2.362213,0,0,0,0,0,0,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,");
		d.knobs [1, 0] = 0;
	}

	//get knob
	float gn(int a, float w,float m){
//		float r = 0;
//		if (!playing)
//			r = 1 + (audiM.GetBands (new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }) * w * m);
//		else if (playCounter < d.volumeBuffer.Length)
//			r = 1 + d.volumeAudioBuffer [audioPlayCounter];//* w * m;
//		return r;
		return 1;
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

    void Update() {



        if (!triggered && tracker1.GetComponent<Collided>().triggered ||  tracker2.GetComponent<Collided>().triggered ) {

            /*
            int res = 128;
            if (tracker1.GetComponent<Collided>().res > 0)
                res = tracker1.GetComponent<Collided>().res;
            else if (tracker2.GetComponent<Collided>().res > 0)
                res = tracker2.GetComponent<Collided>().res;

   
            if (!meshMade) {
                //wire.GetComponent<wireFrameAthon>().numPoints = res;
                wire.GetComponent<wireFrameAthon>().Init();
                meshMade = true;
            }
             
            if (d.dials[0, 7] > 0)
                d.dials[0, 7] *= .99f;
            if (d.knobs[1, 0] < .2f)
                d.knobs[1, 0] += .01f;
            //			d.readDials ("0,1.340679,0.629921,0,8.897638,5.538507,0,0,0,0,0,0,0,10,0,0,2.519684,0,0,0,0,0,0,0,0,0,0,,2.362213,0,0,0,0,0,0,0,0,0.5633354,6.284904,0,0.5511856,0,0,0,0,0,0,0,0,0,0,0,0,0,0,");
            if (d.dials[0, 7] < .01f && d.knobs[1, 0] > .48f)
                triggered = true;
            titleAlpha *= .9f;
            title.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, titleAlpha);
            */
            d.dials[0, 7] = 0;
            d.knobs[1, 0] = .2f;
            //d.knobs[1, 1] = .9f;
            triggered = true;
            //			Debug.Log (d.dials [0, 7]);
        }
        if (triggered) {
            title.SetActive(false);
        }
        //		if(!playing)
        //			d.checkDials (false);
        //		if ( Input.GetKeyUp( KeyCode.U) && recordMode || MidiInput.GetKnob (45,MidiInput.Filter.Realtime) > .5f) {
        //			recordMode = false;
        //			Debug.Log (recordMode);
        //		}
        //		else if (Input.GetKeyUp( KeyCode.I) || MidiInput.GetKnob (44,MidiInput.Filter.Realtime) > .5f) {
        //			recordMode = true;
        //			Debug.Log (recordMode);
        //		}
        //		if (MidiInput.GetKnob (47, MidiInput.Filter.Realtime) > .5f) {
        //			for (int i = 0; i < 8; i++) {
        //				string s = texts [i].text;// System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt");
        //				print (s);
        //				presets [i] = s;//(System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
        //				d.readDials ( s);//(System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
        //			}
        //		}
        //		if (Input.GetKeyUp( KeyCode.L) || MidiInput.GetKnob (48, MidiInput.Filter.Realtime) > .5f) {
        //			for (int i = 0; i < 8; i++) {
        //				System.IO.File.WriteAllText ("Assets/athon/data/data_" + sessionName + i + ".txt", presets[i]);
        //			}
        //			Debug.Log ("saved");
        //		}
        //		if (Input.any) {
       // for (int i = 0; i < 8; i++) {
            //				Debug.Log (keyCodes [i]);
         //   if (Input.GetKeyUp(keyCodes[i])) {
             //   if (recordMode) {
             //       presets[i] = d.recordDials();
             //   } else {
             //       d.readDials(presets[i]);
             //       Menu.GetComponent<UIMidi>().setSliders();
             //   }
                //					Debug.Log (presets [i]);
          //  }
     //   }
        //		}

        //		if (Input.GetKeyUp (KeyCode.R)) {
        //			recording = !recording;
        //			audiM.Play ();
        ////			d.checkDials (true);
        //			Debug.Log (recording);
        //		}
        //		if (Input.GetKeyUp (KeyCode.M)) {
        //			if (!Menu.transform.parent.gameObject.activeInHierarchy)
        //				Menu.transform.parent.gameObject.SetActive (true);
        //			else
        //				Menu.transform.parent.gameObject.SetActive (false);
        //		}
        //		if (recording) {
        //			recordCounter += Time.deltaTime;
        //			if (recordCounter > recordFrequency) {
        //				recordCounter = 0;
        //				d.makeBuffer (audiM.GetBands (new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }));
        //			}
        //			d.makeAudioBuffer(audiM.GetBands (new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }));
        //		}
        //		if (Input.GetKeyUp (KeyCode.S)) {
        //			System.IO.File.WriteAllText ("Assets/athon/data/session_" + sessionName + ".txt",d.buffer);
        //			System.IO.File.WriteAllText ("Assets/athon/data/sessionAudio_" + sessionName + ".txt",d.audioBuffer);
        //		}
        //		if (Input.GetKeyUp (KeyCode.P)) {
        //			if (!buffered) {
        //				d.readBuffer(System.IO.File.ReadAllText ("Assets/athon/data/session_" + sessionName + ".txt"));
        //				d.readAudioBuffer(System.IO.File.ReadAllText ("Assets/athon/data/sessionAudio_" + sessionName + ".txt"));
        //				buffered = true;
        //			}
        //			playing = !playing;
        //			startTime = Time.time;
        //			startBufferTime = d.timeBuffer [0];
        //			audiM.Play ();
        //		}
        //		if (playing) {
        //			if (playCounter < d.timeBuffer.Length - 1) {
        //				if (Time.time - startTime > d.timeBuffer [playCounter] - startBufferTime) {
        //					float diff = (d.timeBuffer[playCounter] - startBufferTime - Time.time - startTime) /
        //						(d.timeBuffer[playCounter] - startBufferTime - 
        //							d.timeBuffer[playCounter-1] - startBufferTime);
        //					d.readDials (d.presetBuffer [playCounter-1], d.presetBuffer [playCounter], diff);
        //				}
        //				while (d.timeBuffer [playCounter] - startBufferTime < Time.time - startTime) {
        //					playCounter++;
        //				}
        //				while (d.timeAudioBuffer [audioPlayCounter] - startBufferTime < Time.time - startTime) {
        //					audioPlayCounter++;
        //				}
        //			} else {
        //				playing = false;
        //				Debug.Log ("playtime is OVER!");
        //				//Camera.main.gameObject.GetComponent<CaptureStandard> ().enabled = false;
        //			}
        //			if (Input.GetKeyUp (KeyCode.O)) {
        //				playing = !playing;
        //				//Camera.main.gameObject.GetComponent<CaptureStandard> ().enabled = false;
        //			}
        //		}
        //		if (Input.GetKeyUp (KeyCode.B)) {
        //			audiM.Play ();
        //		}

        float t = Time.deltaTime;

        //Camera.main.transform.parent.transform.localPosition = new Vector3 (0, 0, Mathf.Pow(d.knobs[1,2]*.1f,3) * -5f);
        //Camera.main.transform.parent.transform.parent.transform.Rotate (0, d.knobs[1,3] * -.1f*t*60,0);

        objSwitcher();
        things[whichThing].transform.Rotate(d.knobs[1, 4] * -.1f * t * 60, d.knobs[1, 5] * -.1f * t * 60, d.knobs[1, 6] * -.1f * t * 60);

        noise.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 1, 1, d.dials[0, 0] * .1f * (1f + gn(1, d.knobs[0, 0], 10))));
        A.GetComponent<Renderer>().sharedMaterial.SetFloat("_Amount", d.dials[0, 1] * gn(2, d.knobs[0, 1], 10) * .01f * (1 + d.dials[0, 2] * 2f) * gn(3, d.knobs[0, 2], 10));
        C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Speed", d.dials[0, 3] * .1f * gn(4, d.knobs[0, 3], 10));
        C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Freq", d.dials[0, 4] * .01f * gn(5, d.knobs[0, 4], 10) * (1 + d.dials[0, 5]) * 3f * gn(6, d.knobs[0, 5], 10));
        C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Which", d.dials[0, 6] * .1f);
        /*
        for (int i = 0; i < 8; i++) {
            if (d.buttons[1, i] > .3f) {
                if (recordMode) {
                    presets[i] = d.recordDials();
                    //					System.IO.File.WriteAllText ("Assets/athon/data/data_" + i + ".txt", d.recordDials ());
                    print("Saved: " + i);
                } else {
                    d.readDials(presets[i]);
                    //					d.readDials (System.IO.File.ReadAllText ("Assets/athon/data/data_" + i + ".txt"));
                }
            }

        }
        */

        C.GetComponent<Renderer>().sharedMaterial.SetVector("_Pos",
            new Vector4(
               tracker1.transform.position.x,
               tracker1.transform.position.y,
               tracker1.transform.position.z,
                //Mathf.Sin(Mathf.Pow(d.dials[1,0],2)*Time.time*3f)*d.dials[1,3]*.2f,
                //Mathf.Cos(Mathf.Pow(d.dials[1,1],2)*Time.time*3f)*d.dials[1,3]*.2f,
                //Mathf.Sin(Mathf.Pow(d.dials[1,2],2)*Time.time*3f)*d.dials[1,3]*.2f,
                0));
        C.GetComponent<Renderer>().sharedMaterial.SetVector("_Pos2",
            new Vector4(
                tracker2.transform.position.x,
                tracker2.transform.position.y,
                tracker2.transform.position.z,
                0));
        C.GetComponent<Renderer>().sharedMaterial.SetVector("_Speeds",
            new Vector4(
                d.dials[1, 5],
                d.dials[1, 6],
                d.dials[1, 7], 0));
        initial.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 1, 1, d.dials[0, 7] * .1f * gn(8, d.knobs[0, 7], 10)));
        C.GetComponent<Renderer>().sharedMaterial.SetFloat("_SinAdd", d.dials[0, 8] * .1f * gn(9, d.knobs[0, 8], 10));



        // C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Gravity", d.dials[1, 4] * .1f);
        if (controller.GetComponent<SteamVR_TrackedController1>().triggerPressed)
            //        if (ViveWand.click)
            C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Repel1", -1);
        else
            C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Repel1", 1);

        if (controller2.GetComponent<SteamVR_TrackedController1>().triggerPressed)
            C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Repel2", -1);
        else
            C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Repel2", 1);

        float b = controller.GetComponent<SteamVR_TrackedController1>().padPosition.x;
        float a = controller2.GetComponent<SteamVR_TrackedController1>().padPosition.x;
        float by = controller.GetComponent<SteamVR_TrackedController1>().padPosition.y;
        float ay = controller2.GetComponent<SteamVR_TrackedController1>().padPosition.y;

        // float bC = (1+controller.GetComponent<SteamVR_TrackedController>().padPosition.y)*.5f;
        // float aC = (1+controller2.GetComponent<SteamVR_TrackedController>().padPosition.y)*.5f;

        // if((1+tracker1.transform.localPosition.z)*.5f != aC) {
        //     tracker1.transform.localPosition = new Vector3(0,0,aC);
        // }
        //  if ((1 + tracker2.transform.localPosition.z) * .5f != bC) {
        //     tracker2.transform.localPosition = new Vector3(0, 0, bC);
        // }

        float colb = (b + 1) * .5f;
        float cola = (a + 1) * .5f;
      ///  tracker1.GetComponent<MeshRenderer>().material.color = new Color(col, 1 - col, .5f, 1);
//Debug.Log(a);
       // Debug.Log(b);

        // float b = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestRight)).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x;
        //  float a = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestLeft)).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x;
        if (a != prevSpeed1 && a != 0) {
            tracker1.transform.GetChild(0).GetComponent<setCTRLPosition>().setPos(new Vector2(a * .5f, ay * .5f));
            prevSpeed1 = Mathf.Max(-1f,a-.05f);
            tracker1.GetComponent<MeshRenderer>().material.color = new Color(cola,.5f, 1 - cola, 1);
        }
        if (b != prevSpeed2 && b != 0) {
            tracker2.transform.GetChild(0).GetComponent<setCTRLPosition>().setPos(new Vector2(b * .5f, by*.5f));
            prevSpeed2 = Mathf.Max(-1f, b - .05f);
            tracker2.GetComponent<MeshRenderer>().material.color = new Color(colb,.5f, 1 - colb, 1);

        }
        
        C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Force1", prevSpeed2);
        C.GetComponent<Renderer>().sharedMaterial.SetFloat("_Force2", prevSpeed1);

        wire.GetComponent<wireFrameAthon>().lineWidth = d.knobs[1,0] * .005f;
		wire.GetComponent<Renderer>().sharedMaterial.SetColor("_Color",new Color(1,1,1, d.knobs[1,1] * .02f));


	}
}
