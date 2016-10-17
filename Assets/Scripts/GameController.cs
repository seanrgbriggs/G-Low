using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	List<PlayerCar> players;

	WaypointScript[] waypoints;

	public int laps_max;

	// Use this for initialization
	void Start () {
		GameObject[] player_objs = GameObject.FindGameObjectsWithTag ("Player");
 
		players = new List<PlayerCar> ();
		for (int i = 0; i < player_objs.Length; i++) {
			players.Add(player_objs [i].GetComponent<PlayerCar> ());
		}


		GameObject[] waypoint_objs = GameObject.FindGameObjectsWithTag ("Waypoint");	
		waypoints = new WaypointScript[waypoint_objs.Length];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = waypoint_objs [i].GetComponent<WaypointScript> ();
		}

 
	}
	
	// Update is called once per frame
	void Update () {
		players.Sort (((x, y) => x.getDistance ().CompareTo (y.getDistance ())));
		for (int i = 0; i < players.Count; i++) {
			print (i + ": " + players.ToArray () [i].gameObject.name);
		}
 	
	}

	public WaypointScript[] getWaypoints(){
		return waypoints;
	}

}
