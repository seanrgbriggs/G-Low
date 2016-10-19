using UnityEngine;
using System.Collections;

public class SpinMeRightRound : MonoBehaviour {
    public Vector3 r;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += r * Time.deltaTime;
	}
}
