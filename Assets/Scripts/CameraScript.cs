using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public Transform tr;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.forward = tr.position - transform.position;
	}
}
