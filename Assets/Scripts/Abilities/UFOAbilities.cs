using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UFOAbilities : PlayerAbilities {

    public float magnetize_range;
    public float pulse_range;

    public float magnetize_force;
    public float pulse_force;

    GameController gc;

    protected override void Start() {
        base.Start();
        gc = FindObjectOfType<GameController>();
    }

    public override bool UseAbility() //Magnetize
    {
        if (!base.UseAbility())
        {
            return false;
        }

        return forceOnPlayersWithin(-magnetize_force);

    }

    public override bool UseUltimate()
    {
        if (!base.UseUltimate())
        {
            return false;
        }

        return forceOnPlayersWithin(pulse_force); 
    }

    public bool forceOnPlayersWithin(float magnitude) {
        bool successful = false;

        foreach (PlayerCar p in gc.getPlayers())
        {
            p.GetComponent<Rigidbody>().AddForce((p.transform.position - transform.position).normalized * magnitude);
            successful = true;
        }

        return successful;
    }
    
}
