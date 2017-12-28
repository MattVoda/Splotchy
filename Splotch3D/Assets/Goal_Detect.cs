using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Detect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        Debug.Log("GOALLLLLLLLLLLL");
    }

}
