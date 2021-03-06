﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterMenuGlobal : MonoBehaviour {
    CharacterSelectPanel[] panels;

	// Use this for initialization
	void Start () {
        panels = FindObjectsOfType<CharacterSelectPanel>();

        foreach (string s in Input.GetJoystickNames())
        {
            print(s);
        }
	}
	
	// Update is called once per frame
	void Update () {
        int players = 0;
        bool ready = true;

	    foreach (CharacterSelectPanel panel in panels) {
            if (panel.started && !panel.ready) {
                ready = false;
            }

            if (panel.started) {
                players++;
            }
        }

        if (ready && players > 0) {
            if (MapSelect.map != "") {
                SceneManager.LoadScene(MapSelect.map, LoadSceneMode.Single);
            } else {
                SceneManager.LoadScene("VincentScene", LoadSceneMode.Single);
            }
        }
	}
}
