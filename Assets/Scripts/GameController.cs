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
        players = new List<PlayerCar>(FindObjectsOfType<PlayerCar>());


        waypoints = FindObjectsOfType<WaypointScript>();

        float lap_length = 0;
        for (int i = 1; i < waypoints.Length; i++)
        {
            lap_length += (waypoints[i].transform.position - waypoints[i - 1].transform.position).magnitude;
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

        foreach (PlayerCar player in players)
        {
            player.AssignStuffAndShit();
        }
    }

    // Update is called once per frame
    void Update () {
        players.Sort (((x, y) => (x.getLaps() + x.getDistance ()).CompareTo (y.getLaps() + y.getDistance ())));
	}

	public WaypointScript[] getWaypoints(){

        return (WaypointScript[])waypoints.Clone();
	}

    public int getPosition(PlayerCar p)
    {
        return players.IndexOf(p);
    }

    public int getNumPlayers()
    {
        return players.Count;
    }

}
