using UnityEngine;
using System.Collections;

public class ViveWandControl : MonoBehaviour {

    //public GameObject flowParent;
    //GameObject[] pictures;
    //GameObject picture;
    //bool following = false;
    Dials d;
	public GameObject controller1;
    public GameObject controller2;
    public bool click = false;
    public bool pad = false;
    // bool setup = false;
    // public PlayControl player;

	// Use this for initialization
	void Start () {
        var trackedController = controller1.GetComponent<SteamVR_TrackedController1>();
        var trackedController2 = controller2.GetComponent<SteamVR_TrackedController1>();


        trackedController.TriggerClicked += new ClickedEventHandler1(DoClick);
        trackedController.TriggerUnclicked += new ClickedEventHandler1(DoUnClick);

        trackedController.TriggerClicked += new ClickedEventHandler1(DoPad);
        trackedController.TriggerUnclicked += new ClickedEventHandler1(DoUnpad);

        Debug.Log(trackedController);

        //trackedController2.TriggerClicked += new ClickedEventHandler(DoStart);

        // trackedController2.PadClicked += new ClickedEventHandler(DoReset);

    }

    void DoStart(object sender, ClickedEventArgs1 e) {
        //player.triggerPlay = true;
    }
    /*
    void DoReset(object sender, ClickedEventArgs e) {
        player.triggerReset = true;
        setup = false;
    }
    */
    void DoClick(object sender, ClickedEventArgs1 e) {
        click = true;
    }

    void DoUnClick(object sender, ClickedEventArgs1 e) {
        click = false;
    }

    void DoPad(object sender, ClickedEventArgs1 e) {
        pad = true;
    }

    void DoUnpad(object sender, ClickedEventArgs1 e) {
        pad = false;
    }

    // Update is called once per frame
    void Update () {
        /*
		if (Input.GetMouseButtonDown (0)) {
			if (!setup) {
				pictures = new GameObject[flowParent.transform.childCount];
				for (int i = 0; i < flowParent.transform.childCount; i++) {
					pictures [i] = flowParent.transform.GetChild (i).gameObject;
				}
				setup = true;
			}
			picture = findClosest (controller1.transform.position);
			picture.GetComponent<isFollowing> ().following = false;
			following = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			following = false;
			picture.GetComponent<isFollowing> ().following = true;
		}
		if (following)
			follow ();
            */
	}

	void follow(){
        /*
		picture.transform.position = Vector3.Lerp (picture.transform.position, controller1.transform.position, .1f);
		//picture.transform.LookAt (controller1.transform.position);
        picture.transform.LookAt (Camera.main.transform.position);
        */

    }

       
}
