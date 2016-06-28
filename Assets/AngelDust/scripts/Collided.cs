using UnityEngine;
using System.Collections;

public class Collided : MonoBehaviour {

    public bool triggered = false;
    bool first = true;
    public int res = -1;
    void OnTriggerExit(Collider other) {
        if (first)
            first = false;
        else { 
        triggered = true;
        res = other.GetComponent<resolution>().res;
      }
    }
	
}
