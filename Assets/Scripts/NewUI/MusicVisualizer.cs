using UnityEngine;
using System.Collections;

public class MusicVisualizer : MonoBehaviour {
    public AudioSource source;
    public FFTWindow window = FFTWindow.BlackmanHarris;
    public int channel = 0;
    public float multiplier = 40;
    public float lerpTime = 0.2f;
    public float baseline = 4;

    private Transform[] viewObjects;

    public int numSamples;

    float[] data;

    // Use this for initialization
    void Start() {
        viewObjects = new Transform[transform.childCount];

        for (int i = 0; i < viewObjects.Length; i++) {
            viewObjects[i] = transform.GetChild(i);
        }

        data = new float[numSamples];
    }

    // Update is called once per frame
    void Update() {
        source.GetSpectrumData(data, channel, window);

        for (int i = 0; i < viewObjects.Length; i++) {
            float intensity = data[i] * multiplier;

            Vector3 v = viewObjects[i].localScale;
            v.y = baseline + Mathf.Lerp(v.y, intensity, lerpTime);
            viewObjects[i].localScale = v;
        }

        /*
        for (int i = 1; i < data.Length - 1; i++) {
            Debug.DrawLine(new Vector3(i - 1, data[i] + 10, 0), new Vector3(i, data[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(data[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(data[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), data[i - 1] - 10, 1), new Vector3(Mathf.Log(i), data[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(data[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(data[i]), 3), Color.blue);
        }*/

    }
}
