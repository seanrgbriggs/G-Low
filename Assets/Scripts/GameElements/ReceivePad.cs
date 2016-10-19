using UnityEngine;
using System.Collections;

public class ReceivePad : MonoBehaviour {
    public float traction = 10;
    public float alignment_power = 100;


    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            print("HI");
            col.GetComponent<Rigidbody>().AddForce(-transform.up * traction, ForceMode.Acceleration);

            if (Vector3.Distance(col.transform.up, transform.up) > 0.5f)
            {
                //col.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(col.transform.forward, transform.forward) * alignment_power);
                //col.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(col.transform.up, transform.up) * alignment_power);

                Vector3 axis = Vector3.Cross(col.transform.up, transform.up).normalized;
                float angle = Vector3.Angle(col.transform.up, transform.up);

                col.transform.Rotate(axis, Mathf.Min(angle, Time.deltaTime * alignment_power), Space.World);
            }
        }
    }

}
