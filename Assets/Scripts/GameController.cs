using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	PlayerCar[] players;
	Queue<float> player_dists;

	WaypointScript[] waypoints;

	// Use this for initialization
	void Start () {
		GameObject[] player_objs = GameObject.FindGameObjectsWithTag ("Player");
		players = new PlayerCar[player_objs.Length];
		for (int i = 0; i < players.Length; i++) {
			players [i] = player_objs [i].GetComponent<PlayerCar> ();
		}


		GameObject[] waypoint_objs = GameObject.FindGameObjectsWithTag ("Waypoint");	
		waypoints = new WaypointScript[waypoint_objs.Length];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = waypoint_objs [i].GetComponent<WaypointScript> ();
		}

		player_dists = new Queue<float> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public WaypointScript[] getWaypoints(){
		return waypoints;
	}

}
