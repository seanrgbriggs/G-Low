using UnityEngine;
using System.Collections;

public class SpeedPad : MonoBehaviour {

    public float power;
	
	// Update is called once per frame
	void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * power * rb.drag / .4f, ForceMode.Acceleration);
        }
    }
}
