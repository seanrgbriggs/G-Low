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

        List<PlayerCar> players = GetPlayersWithin(magnetize_range);

        bool successful = players.Count > 0;

        foreach(PlayerCar p in players)
        {
            p.GetComponent<Rigidbody>().AddForce((transform.position - p.transform.position).normalized * magnetize_force);
        }

        return successful;
    }

    public override bool UseUltimate()
    {
        if (!base.UseAbility())
        {
            return false;
        }

        List<PlayerCar> players = GetPlayersWithin(pulse_range);

        bool successful = players.Count > 0;

        foreach (PlayerCar p in players)
        {
            p.GetComponent<Rigidbody>().AddForce((p.transform.position - transform.position).normalized * pulse_force);
        }

        return successful; 
    }

    public List<PlayerCar> GetPlayersWithin(float dist) {
        List<PlayerCar> players = new List<PlayerCar>();
        players.Remove(player);

        foreach(PlayerCar p in players)
        {
            if((transform.position - p.transform.position).magnitude > dist)
            {
                players.Remove(p);
            }
        }

        return players;
    }
    
}
