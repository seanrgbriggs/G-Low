using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarUI : MonoBehaviour {
    public Image abilityMeter;
    public Image ultMeter;
    public Text spedometer;

    private PlayerCar car;

	// Use this for initialization
	void Start () {
        car = GetComponentInParent<PlayerCar>();
	}
	
	// Update is called once per frame
	void Update () {
        spedometer.text = (int)car.GetComponent<Rigidbody>().velocity.magnitude + " MPH";
	}
}
