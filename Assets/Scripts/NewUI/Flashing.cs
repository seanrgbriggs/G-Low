using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flashing : MonoBehaviour {
    public Graphic targetGraphic;

    public float maxAlpha;
    public float minAlpha;
    public float period;
    
	
	// Update is called once per frame
	void Update () {
        Color c = targetGraphic.color;
        c.a = minAlpha + Mathf.Cos(Time.time * 2 * Mathf.PI / period) * (maxAlpha - minAlpha);
        targetGraphic.color = c;
	}
}
