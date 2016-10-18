using UnityEngine;
using System.Collections;

public class Fuckery : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
