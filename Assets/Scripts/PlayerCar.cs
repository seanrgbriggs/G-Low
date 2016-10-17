using UnityEngine;
using System.Collections;

public class PlayerCar : MonoBehaviour {
    private Rigidbody rb;
    public Transform[] wheels;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        foreach (Transform wheel in wheels) {
            if (Physics.Raycast(wheel.position, -transform.up, out hit, 20.0f)) {

                rb.AddForceAtPosition(hit.normal * 9.8f * (2.0f - hit.distance) * 0.25f, wheel.position, ForceMode.Acceleration);


                rb.AddForce(transform.forward * Input.GetAxis("Vertical") * 5, ForceMode.Acceleration);
                rb.AddTorque(transform.up * Input.GetAxis("Horizontal") * 1, ForceMode.Acceleration);
            }
        }

            //rb.AddForce(hit.normal * 9.8f * (2.0f - hit.distance));
            //transform.Rotate(Vector3.Cross(transform.up, hit.normal), Mathf.Min(Vector3.Angle(transform.up, hit.normal), 10 * Time.deltaTime), Space.World);




       GetComponentInChildren<Camera>().transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 60);
    }
}
