using UnityEngine;
using System.Collections;

public class SpinMeRightRound : MonoBehaviour {
    public Vector3 r;
    public bool useRigidBody = false;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
	    if (useRigidBody) {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (useRigidBody) {
            rb.angularVelocity = r;
        } else {
            transform.localEulerAngles += r * Time.deltaTime;
        }
	}
}
