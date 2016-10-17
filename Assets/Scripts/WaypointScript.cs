using UnityEngine;
using System.Collections;

public class WaypointScript : MonoBehaviour{

	public float value;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float distanceFrom(Vector3 pos){
		return (GetComponent<Collider> ().ClosestPointOnBounds (pos) - pos).magnitude;
	}

	public static float distBetween(WaypointScript w1, WaypointScript w2, Vector3 pos){
		WaypointScript first;
		WaypointScript second;
		if (w1.value < w2.value) {
			first = w1;
			second = w2;
		} else {
			first = w2;
			second = w1;
		}

		return first.value + (first.distanceFrom (pos) / first.distanceFrom (second.transform.position));

	}


}
