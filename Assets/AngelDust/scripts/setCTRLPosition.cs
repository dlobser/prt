using UnityEngine;
using System.Collections;

public class setCTRLPosition : MonoBehaviour {

    public GameObject controlPoint;
    public GameObject plat;
    public Vector2 pos;
    public bool active = false;
    float transparency;
	// Use this for initialization
	void Start () {
        activate();
	}
	
	// Update is called once per frame
	void Update () {
        transout();
	}

    public void setPos(Vector2 position) {
        if (!active)
            active = true;
        controlPoint.transform.localPosition = new Vector3(position.x, 0, position.y);
        transparency = 1;
        activate();
    }

    void setTrans(float t) {
        Color c = controlPoint.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        controlPoint.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, t);
        c = plat.GetComponent<MeshRenderer>().material.color;
        plat.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, t*.5f);
    }

    void activate() {
        if (controlPoint.active && active == false)
            controlPoint.SetActive(false);
        else if(active)
            controlPoint.SetActive(true);
        if (plat.active)
            plat.SetActive(false);
       else if(active)
            plat.SetActive(true);
    }

    void transout() {
        if(active && transparency > .01f) {
            transparency *= .95f;
            setTrans(transparency);
        }
        else if (active) {
            transparency = 0;
            active = false;
            activate();

        }

    }
}
