using UnityEngine;
using System.Collections;

public class WaypointScript : MonoBehaviour{

    public int id;
    public float value;

	// Use this for initialization
	void Awake () {
        //	Destroy (GetComponent<Collider> ());
        //	Destroy (GetComponent<Renderer> ());
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            //mesh.enabled = false;
        }

        if (name.EndsWith(")"))
        {
            int i = name.IndexOf("(");
            int j = name.IndexOf(")");
            id = System.Convert.ToInt32(name.Substring(i + 1, j - i - 1));
        } else
        {
            id = 0;
        }
	}


	public float distanceFrom(Vector3 pos){
		return (GetComponent<Collider> ().ClosestPointOnBounds (pos) - pos).magnitude;
	}

	public static float distBetween(WaypointScript w1, WaypointScript w2, Vector3 pos){
		WaypointScript first;
		WaypointScript second;

		if (w1.value < w2.value) {
            first = w1;
			second = w2;
		} else {
			first = w2;
			second = w1;
		}

        float firstval = first.value;
        float secondval = second.value;

        if (first.value == 0 && second.value > 0.5f)
        {
            firstval = secondval;
            secondval = 1;

            WaypointScript third = first;
            first = second;
            second = third;
        }

		return firstval + (secondval - firstval) * (first.distanceFrom (pos) / (first.distanceFrom (pos) + second.distanceFrom(pos)));

	}


}
