using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HondaAbilities : PlayerAbilities {

    public float nitrous_power;
    public float spin_power;

    bool is_nitrous;
    Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }


    protected override void Update() {
        if (GetAbilityCooldown() >= 0.25f && Input.GetButtonDown("Ability" + id) && !is_nitrous)
        {
            UseAbility();
        }
        else if(is_nitrous)
        {
            UseAbility();
        }else if (abil_cd < abil_max && !is_nitrous)
        {
            abil_cd += Time.deltaTime;
        }

        if (Input.GetButtonDown("Ultimate" + id))
        {
            UseUltimate();
        }
        if (ult_cd < ult_max)
        {
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
        if (!base.UseUltimate())
        {
            return false;
        }
        
        rb.AddTorque(transform.up * spin_power, ForceMode.Acceleration);
        return true;
    }


    
}
