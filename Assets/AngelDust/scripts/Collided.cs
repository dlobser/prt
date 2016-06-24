using UnityEngine;
using System.Collections;

public class Collided : MonoBehaviour {

    public bool triggered = false;
    void OnTriggerExit(Collider other) {
        triggered = true;
        
    }
	
}
