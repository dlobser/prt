using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dials : Singleton<Dials> {

	//converts korg nano dials to an easily readable array of values

	protected Dials () {}

	public float mult = 1;
	public float[,] dials;
	public float[,] knobs;
	public float[,] prevDials;
	public float[,] prevKnobs;
	public float[,] buttons;

	public string buffer;
	public string audioBuffer;
	public float[] volumeAudioBuffer;
	public float[] timeAudioBuffer;

	public float[] timeBuffer;
	public float[] volumeBuffer;
	public string[] presetBuffer;

	int[] dialID = { 14, 15, 16, 17, 18, 19, 20, 21, 22,
	57,58,59,60,61,62,63,65,66,
	94,95,96,97,102,103,104,105,106};
	
	int[] knobID = {2, 3, 4, 5, 6, 8, 9, 12, 13,
		42, 43, 50, 51, 52, 53, 54, 55, 56,
		85, 86, 87, 88, 89, 90, 91, 92, 93};

	int[] buttonID;
	
	void Awake () {
		buffer = "";
		audioBuffer = "";

		buttonID = new int[18];
		for (int i = 0; i < 18; i++) {
			int extra = 23;
			if (i == 8)
				extra++;
			buttonID [i] = i + extra;
		}
		dials = new float[3,9];
		knobs = new float[3,9];
		prevDials = new float[3,9];
		prevKnobs = new float[3,9];
		buttons = new float[2,9];
	}


	
	public void checkDials (bool refresh) {
		int q = -1;

		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 9; j++) {
				++q;
				if (refresh||!(MidiInput.GetKnob (dialID [q], MidiInput.Filter.Realtime)*mult).Equals(prevDials [i, j])) {
					dials [i, j] =    ( prevDials[i,j] * 10 + MidiInput.GetKnob (dialID [q], MidiInput.Filter.Realtime) * mult )/11 ;
					prevDials [i, j] = dials[i,j];
				}
				if (refresh||!(MidiInput.GetKnob (knobID [q], MidiInput.Filter.Realtime)*mult).Equals(prevKnobs [i, j])) {
					knobs [i, j] = 	  ( prevKnobs[i,j] * 10 + MidiInput.GetKnob (knobID [q], MidiInput.Filter.Realtime) * mult )/11 ;
					prevKnobs [i, j] = knobs[i,j];
				}
				if (i < 2) {
					buttons [i, j] = 
						MidiInput.GetKey (buttonID[q]);
				}
			}
		}
	}

	public void makeBuffer(float audioValue){
		buffer += ":"+Time.time+":"+audioValue+":";
		buffer += recordDials();
	}

	public void makeAudioBuffer(float audioValue){
		audioBuffer += ":"+Time.time+":"+audioValue;
	}
		
	public void readBuffer(string s){
		
		string[] first = s.Split (new string[] { ":" }, System.StringSplitOptions.None);
		volumeBuffer = new float[first.Length / 2];
		timeBuffer = new float[first.Length / 2];
		presetBuffer = new string[first.Length / 2];
//		for (int i = 0; i < first.Length; i++) {
//			print (first [i]);
//		}
		Debug.Log (first[1]);
		Debug.Log (s);
		int q = 0;
		for (int i = 0; i < first.Length/3; i++) {

			timeBuffer [i] = float.Parse(first [++q]);
			volumeBuffer [i] = float.Parse(first [++q]);
			presetBuffer [i] = first [++q];
		}
	}

	public void readAudioBuffer(string s){

		string[] first = s.Split (new string[] { ":" }, System.StringSplitOptions.None);
		volumeAudioBuffer = new float[first.Length / 2];
		timeAudioBuffer = new float[first.Length / 2];

		int q = 0;
		for (int i = 0; i < first.Length/2; i++) {

			timeAudioBuffer [i] = float.Parse(first [++q]);
			volumeAudioBuffer [i] = float.Parse(first [++q]);
		}
	}
		
	public string recordDials () {
		string s = "";
		s += parser(dials);
		s += ",";
		s += parser(knobs);
//		print (s);
		return s;

	}
	public void readDials(string s){
		if (s != null) {
			if (s.Length > 0) {
				string[] first = s.Split (new string[] { ",," }, System.StringSplitOptions.None);
				string[] dial = first [0].Split (new string[] { "," }, System.StringSplitOptions.None);
				string[] knob = first [1].Split (new string[] { "," }, System.StringSplitOptions.None);
				int q = -1;
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 9; j++) {
						dials [i, j] = float.Parse (dial [++q]);
						knobs [i, j] = float.Parse (knob [q]);
//						prevDials [i, j] = float.Parse (dial [q]);
//						prevKnobs [i, j] = float.Parse (knob [q]);
					}
				}
			}
		}
	}

	//lerp version
	public void readDials(string s, string s2, float t){
		if (s != null) {
			if (s.Length > 0) {
				string[] first = s.Split (new string[] { ",," }, System.StringSplitOptions.None);
				string[] dial = first [0].Split (new string[] { "," }, System.StringSplitOptions.None);
				string[] knob = first [1].Split (new string[] { "," }, System.StringSplitOptions.None);

				string[] first2 = s2.Split (new string[] { ",," }, System.StringSplitOptions.None);
				string[] dial2 = first2 [0].Split (new string[] { "," }, System.StringSplitOptions.None);
				string[] knob2 = first2 [1].Split (new string[] { "," }, System.StringSplitOptions.None);

				int q = -1;
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 9; j++) {
						q++;
						dials [i, j] = Mathf.Lerp(float.Parse (dial [q]),float.Parse (dial2[q]),t);
						knobs [i, j] = Mathf.Lerp(float.Parse (knob [q]),float.Parse (knob2[q]),t);
						//						prevMathf.Lerp(Dials [i, j] = float.Parse (dial [q]);
						//						prevKnobs [i, j] = float.Parse (knob [q]);
					}
				}
			}
		}
	}

	public string parser(float[,] a){
		string s = "";
		for (int i = 0; i < a.GetLength(0); i++) {
			for (int j = 0; j < a.GetLength(1); j++) {
				s += a [i, j] + ",";
			}
		}
		return s;
	}

}
