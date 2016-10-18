using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	List<PlayerCar> players;

	WaypointScript[] waypoints;

	public int laps_max;

    // Use this for initialization
    void Awake()
    {
        GameObject[] player_objs = GameObject.FindGameObjectsWithTag("Player");

        players = new List<PlayerCar>();
        for (int i = 0; i < player_objs.Length; i++)
        {
            players.Add(player_objs[i].GetComponent<PlayerCar>());
        }


        GameObject[] waypoint_objs = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new WaypointScript[waypoint_objs.Length];

        float lap_length = 0;
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypoint_objs[i].GetComponent<WaypointScript>();
            if (i > 0)
            {
                lap_length += (waypoints[i].transform.position - waypoints[i - 1].transform.position).magnitude;
            }
        }
        lap_length += (waypoints[0].transform.position - waypoints[waypoints.Length - 1].transform.position).magnitude;

        List<WaypointScript> sorter = new List<WaypointScript>();
        sorter.AddRange(waypoints);
        sorter.Sort(((x, y) => x.id.CompareTo(y.id)));
        waypoints = sorter.ToArray();

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i > 0)
            {
                waypoints[i].value = (waypoints[i].transform.position - waypoints[i - 1].transform.position).magnitude / lap_length;
            }
        }
        waypoints[waypoints.Length - 1].value = (waypoints[0].transform.position - waypoints[waypoints.Length - 1].transform.position).magnitude / lap_length;

    }

    // Update is called once per frame
    void Update () {
        players.Sort (((x, y) => x.getDistance ().CompareTo (y.getDistance ())));
	}

	public WaypointScript[] getWaypoints(){

        return (WaypointScript[])waypoints.Clone();
	}

}
