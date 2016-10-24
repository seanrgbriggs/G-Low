using UnityEngine;
using System.Collections;

public class FlipPad : MonoBehaviour {

    public float jump_boost;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * jump_boost * Mathf.Sqrt(rb.drag / 0.4f), ForceMode.VelocityChange);
        }
    }


}
