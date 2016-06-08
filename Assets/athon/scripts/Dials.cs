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

	int[] dialID = { 14, 15, 16, 17, 18, 19, 20, 21, 22,
	57,58,59,60,61,62,63,65,66,
	94,95,96,97,102,103,104,105,106};
	
	int[] knobID = {2, 3, 4, 5, 6, 8, 9, 12, 13,
		42, 43, 50, 51, 52, 53, 54, 55, 56,
		85, 86, 87, 88, 89, 90, 91, 92, 93};

	int[] buttonID;
	
	void Awake () {
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
//				print (!(MidiInput.GetKnob (dialID [++q], MidiInput.Filter.Realtime)*mult).Equals(dials[i,j])+ " , " + dials [i, j]);
				if (refresh||!(MidiInput.GetKnob (dialID [q], MidiInput.Filter.Realtime)*mult).Equals(prevDials [i, j])) {
//					print (q);
//					print (dialID [q]);
//					print ("fuh: " + MidiInput.GetKnob (dialID [q], MidiInput.Filter.Realtime) + ","+
//						dials[i,j]+","+i+","+j);
					dials [i, j] = ( MidiInput.GetKnob (dialID [q], MidiInput.Filter.Realtime) * mult);
					prevDials [i, j] = ( MidiInput.GetKnob (dialID [q], MidiInput.Filter.Realtime) * mult);

				}
				if (refresh||!(MidiInput.GetKnob (knobID [q], MidiInput.Filter.Realtime)*mult).Equals(prevKnobs [i, j])) {
//					print ("fuh: " + MidiInput.GetKnob (knobID [q], MidiInput.Filter.Realtime) + ","+knobs[i,j]+","+i+","+j);
					knobs [i, j] = ( MidiInput.GetKnob (knobID [q], MidiInput.Filter.Realtime) * mult);
					prevKnobs [i, j] = ( MidiInput.GetKnob (knobID [q], MidiInput.Filter.Realtime) * mult);


				}
//				if(MidiInput.GetKnob(knobID[q],MidiInput.Filter.Realtime)!=knobs[i,j]) knobs[i,j] = (knobs[i,j]+MidiInput.GetKnob(knobID[q],MidiInput.Filter.Realtime)*mult)/2;
				if (i < 2) {
//					print (i);
					buttons [i, j] = 
						MidiInput.GetKey (buttonID[q]);
				}
			}
		}

	}
	public string recordDials () {
		string s = "";
		s += parser(dials);
		s += ",";
		s += parser(knobs);
		print (s);
		return s;

	}
	public void readDials(string s){
		if (s != null) {
			print (s);
			if (s.Length > 0) {
				string[] first = s.Split (new string[] { ",," }, System.StringSplitOptions.None);
				print (first [0]);
				print (first [1]);
				string[] dial = first [0].Split (new string[] { "," }, System.StringSplitOptions.None);
				string[] knob = first [1].Split (new string[] { "," }, System.StringSplitOptions.None);
				int q = -1;
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 9; j++) {
						dials [i, j] = float.Parse (dial [++q]);
						knobs [i, j] = float.Parse (knob [q]);
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
