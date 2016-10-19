using UnityEngine;
using System.Collections;

public class FlipPad : MonoBehaviour {

    public float jump_boost;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            print("FLIP:)");
            col.GetComponent<Rigidbody>().AddForce(transform.up * jump_boost, ForceMode.VelocityChange);
        }
    }


}
