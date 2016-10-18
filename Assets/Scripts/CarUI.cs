using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarUI : MonoBehaviour {
    public Image abilityMeter;
    public Image ultMeter;
    public Text spedometer;
    public Text place;
    public Text dist;

    private PlayerCar car;

	// Use this for initialization
	void Start () {
        car = GetComponentInParent<PlayerCar>();
	}
	
	// Update is called once per frame
	void Update () {
        spedometer.text = (int)car.GetComponent<Rigidbody>().velocity.magnitude + " MPH";
        place.text = FindObjectOfType<GameController>().getPosition(car) + "";
        //dist.text = car.getDistance() + "";
    }
}
