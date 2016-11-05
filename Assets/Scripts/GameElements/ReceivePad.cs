using UnityEngine;
using System.Collections;

public class ReceivePad : MonoBehaviour {
    public float traction = 10;
    public float alignment_power = 100;


    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.AddForce(-transform.up * traction * rb.drag / 0.4f, ForceMode.Acceleration);

            if (Vector3.Distance(col.transform.up, transform.up) > 0.5f)
            {
                col.transform.rotation = Quaternion.RotateTowards(col.transform.rotation, transform.rotation, Time.deltaTime * alignment_power);
            }
        }
    }

}
