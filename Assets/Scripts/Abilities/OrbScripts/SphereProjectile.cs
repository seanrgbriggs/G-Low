using UnityEngine;
using System.Collections;

public class SphereProjectile : MonoBehaviour {

    Rigidbody rb;
    float hoverDist = 3.0f;
    float speed = 30.0f;

    Color base_col;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 20.0f))
        {
            //print(hit.transform.name);
            rb.AddForce(hit.normal * 9.8f * (hoverDist - hit.distance) * 0.25f, ForceMode.Acceleration);
            rb.AddForce(transform.forward, ForceMode.Acceleration);

            rb.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
            print(hit.normal + " " + rb.rotation.eulerAngles);
        }
    }
}
