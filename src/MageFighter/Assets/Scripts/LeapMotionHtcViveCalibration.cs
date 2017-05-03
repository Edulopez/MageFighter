using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapMotionHtcViveCalibration : MonoBehaviour {

    public GameObject LeapMotionRig;

    public GameObject HtcViveEyes;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        LeapMotionRig.transform.position = HtcViveEyes.transform.position;
	}
}
