using UnityEngine;
using System.Collections;

public class LaserTrap : MonoBehaviour {
    private MeshRenderer mesh;
    private Collider collider;
    private Material mat;
    private Color baseColor;
    private float baseScale;
    private bool charged = false;

    public float period = 5;
    public float threshold = 0.8f;
    public float maxIntensity = 2.0f;
    public float targetScale = .2f;
    public Color targetColor = new Color(0.0f, 0.3f, 1.0f, 1.0f);
    public float targetThreshold = 0;
    public float resetThreshold = 0;
    public float phase = 0;

	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        mat = Instantiate(mesh.material);
        mesh.material = mat;
        baseColor = mat.GetColor("_EmissionColor");
        baseScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        float f = Mathf.Sin(Time.timeSinceLevelLoad * 2 * Mathf.PI / period + phase);

        if (f > threshold)
        {
            mesh.enabled = true;
            collider.enabled = true;

            float power = ((f - threshold) / threshold * (maxIntensity));

            mat.SetColor("_EmissionColor", baseColor * power);
            Vector3 v = transform.localScale;
            v.x = baseScale * power;
            v.z = baseScale * power;
            transform.localScale = v;

            charged = true;
        }
        else if (f > targetThreshold && !charged)
        {
            mesh.enabled = true;
            collider.enabled = false;

            mat.SetColor("_EmissionColor", targetColor);
            Vector3 v = transform.localScale;
            v.x = targetScale;
            v.z = targetScale;
            transform.localScale = v;
        }
        else
        {
            mesh.enabled = false;
            collider.enabled = false;
        }

        if (f < resetThreshold)
        {
            charged = false;
        }
	}
}
