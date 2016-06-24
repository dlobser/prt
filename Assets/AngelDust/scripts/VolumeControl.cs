using UnityEngine;
using System.Collections;

public class VolumeControl : MonoBehaviour {

	AudioSource audi;
	Vector3 prevPos = Vector3.zero;
	public float avgAmount = 10;
	float avg;
	// Use this for initialization
	void Start () {
		audi = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (transform.position, prevPos);
		prevPos = transform.position;
		avg = ((avg * (avgAmount - 1)) + dist) / avgAmount;
		audi.volume = avg * 5;
	}
}
