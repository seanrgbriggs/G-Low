using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HondaAbilities : PlayerAbilities {

    public float nitrous_power;

    public Material spectralMat;
    Material normalMat;

    bool is_nitrous;
    bool is_spectral;
    Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();

        normalMat = GetComponent<MeshRenderer>().material;
    }


    protected override void Update() {
        if (GetAbilityCooldown() >= 0.25f && Input.GetButtonDown("Ability" + id) && !is_nitrous)  {
            UseAbility();
        } else if(is_nitrous) {
            UseAbility();
        } else if (abil_cd < abil_max && !is_nitrous) {
            abil_cd += Time.deltaTime;
        }

        if (GetUltimateCooldown() >= 0.25f && Input.GetButtonDown("Ultimate" + id) && !is_spectral) {
            UseUltimate();
        } else if (is_spectral) {
            UseUltimate();
        } else if (ult_cd < ult_max && !is_spectral) {
            ult_cd += Time.deltaTime;
        }
    }

    public override bool UseAbility() //Nitrous
    {
        if(abil_cd <= 0)
        {
            abil_cd = - abil_max / 4;
            is_nitrous = false;
            return false;
        }

        is_nitrous = true;
        abil_cd -= Time.deltaTime * 2;
        rb.AddForce(transform.forward * nitrous_power, ForceMode.Acceleration);
        
        return true;
    }

    public override bool UseUltimate()
    {
        if(ult_cd <= 0) {
            ult_cd = -ult_max / 4;
            is_spectral = false;
            GetComponent<MeshRenderer>().material = normalMat;
            gameObject.layer = PlayerCar.LAYER_DEFAULT;
            return false;
        }

        if (!is_spectral) {

            GetComponent<MeshRenderer>().material = spectralMat;
        }

        is_spectral = true;
        ult_cd -= Time.deltaTime * 2;
        gameObject.layer = PlayerCar.LAYER_SPECTRAL;
        
        return true;
    }


    
}
