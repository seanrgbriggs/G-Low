using UnityEngine;
using System.Collections;

public class SpeedPad : MonoBehaviour {

    public float power;
	
	// Update is called once per frame
	void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            col.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Acceleration);
        }
    }
}
