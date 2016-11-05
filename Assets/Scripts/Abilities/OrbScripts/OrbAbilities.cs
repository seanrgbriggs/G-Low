using UnityEngine;
using System.Collections;

public class OrbAbilities : PlayerAbilities {

    OrbPartGenerator gen;
    OrbPartPhysics[] parts;

    public float orbSpawnTimer = 3;
    float curOrbSpawnTime = 0.0f;

    public GameObject projectile;
    public GameObject mine;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        gen = GetComponentInChildren<OrbPartGenerator>();
        parts = GetComponentsInChildren<OrbPartPhysics>();

       // print(gen + " " + parts);
	}

    protected override void Update()
    {
        base.Update();

        if(curOrbSpawnTime > orbSpawnTimer)
        {
            gen.Enable();
            curOrbSpawnTime = 0;
        }else
        {
            curOrbSpawnTime += Time.deltaTime;
        }
    }

    public override bool UseAbility()
    {
        if (!base.UseAbility() && !gen.Disable())
        {
            return false;
        }

        ((GameObject)Instantiate(projectile, transform.position + transform.forward * 2, transform.rotation)).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", player.base_col);
        return true;
    }

    public override bool UseUltimate()
    {
        if (!base.UseUltimate() && !gen.Disable())
        {
            return false;
        }

        ((GameObject)Instantiate(mine, transform.position, transform.rotation)).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", player.base_col);
        return true;
    }

}
