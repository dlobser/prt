using UnityEngine;
using System.Collections;
using System.IO;

public class keepLog : MonoBehaviour {

	string log;
	float usage = 0;
	string startTime;
	string endTime;
//	TextAsset text;
	// Use this for initialization
	void Start () {
		startTime = System.DateTime.Now.ToString("yyyy:MM:DD:hh:mm:ss");
//		text = (TextAsset) Resources.Load ("usage.txt", typeof (TextAsset));

		log = (System.IO.File.ReadAllText (Application.persistentDataPath+"/usage.txt"));// text.text;
	}
	
	// Update is called once per frame
	void Update () {
		usage += Time.deltaTime;
	}

	void OnApplicationQuit() {
		endTime = System.DateTime.Now.ToString("yyyy:MM:DD:hh:mm:ss");
		log+=startTime+","+endTime+","+usage+"|\n";
		System.IO.File.WriteAllText(Application.persistentDataPath+"/usage.txt",log);

	}
}
