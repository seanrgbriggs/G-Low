﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	List<PlayerCar> players;

	WaypointScript[] waypoints;

    OmniController omni;

    public GameObject[] UIPrefabs;

    public Color[] playerColors;
    public static Color[] staticColors;

    public int laps_max;
    public float playerSpeedMultiplier = 1.0f;

    public bool playMusic = true;

    AudioSource aud_src;
    public AudioClip[] game_music;


	void Awake(){
		omni = FindObjectOfType<OmniController>();
		omni.StartGame ();
	}
    // Use this for initialization

	void Start()
    {
        players = new List<PlayerCar>(FindObjectsOfType<PlayerCar>());
        staticColors = playerColors;

        waypoints = FindObjectsOfType<WaypointScript>();


        List<WaypointScript> sorter = new List<WaypointScript>();
        sorter.AddRange(waypoints);
        sorter.Sort(((x, y) => x.id- y.id));
        waypoints = sorter.ToArray();


        float lap_length = 0;
        for (int i = 1; i < waypoints.Length; i++)
        {
            lap_length += Vector3.Distance(waypoints[i].transform.position, waypoints[i - 1].transform.position);
        }

        lap_length += (waypoints[0].transform.position - waypoints[waypoints.Length - 1].transform.position).magnitude;

        for (int i = 1; i < waypoints.Length; i++)
        {
            waypoints[i].value = waypoints[i - 1].value + Vector3.Distance(waypoints[i].transform.position, waypoints[i - 1].transform.position) / lap_length;
        }

        foreach (PlayerCar player in players)
        {
            player.ReadyFields();
        }
        aud_src = GetComponent<AudioSource>();

    }

    void Tunes() {
        if (aud_src != null)
        {
            aud_src.clip = game_music[Random.Range(0, game_music.Length - 1)];
            aud_src.Play();
        }
    }

    // Update is called once per frame
    void Update () {
        players.Sort (((x, y) => -(x.getLaps() + x.getDistance ()).CompareTo (y.getLaps() + y.getDistance ())));
        if (playMusic) {
            if (aud_src != null && !aud_src.isPlaying) {
                Tunes();
            }
        }
	}

	public WaypointScript[] getWaypoints(){

        return (WaypointScript[])waypoints.Clone();
	}

    public int getPosition(PlayerCar p)
    {
        return players.IndexOf(p);
    }

    public List<PlayerCar> getPlayers()
    {
        return players;
    }

    public int getNumPlayers()
    {
        return players.Count;
    }

}
