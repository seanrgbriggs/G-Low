﻿using UnityEngine;
using System.Collections;

public class SphereMine : MonoBehaviour {

    float primeTime = 3.0f;
    float priming = 0.0f;

    float jumpForce = 500.0f;
    float delay = 2.0f;

    float maxRotationSpeed = 30f;

    bool primed;
    Color base_col;
    Light light;

    void Start() {
        base_col = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        light = GetComponent<Light>();
        light.color = base_col;
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.up, maxRotationSpeed * (priming / primeTime));
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", base_col * (priming / primeTime));

        if (priming < primeTime || primed)
        {
            priming += Time.deltaTime;
        }

        if (primed) {
            light.intensity = (priming - primeTime) / delay;
        }
    }

    void OnTriggerEnter(Collider col) {
        if((priming > primeTime) && col.tag == "Player" && !primed)
        {
            //GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Acceleration);
            Invoke("Boom", delay);
            primed = true;
        }
    }

    void Boom()
    {
        foreach(RaycastHit hit in Physics.SphereCastAll(transform.position, 25f, -transform.up))
        {
            if(hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<PlayerCar>().Die();
            }
        }
        Destroy(gameObject);
    }
}