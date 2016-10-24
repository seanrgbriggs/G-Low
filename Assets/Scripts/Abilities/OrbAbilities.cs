using UnityEngine;
using System.Collections;

public class OrbAbilities : PlayerAbilities {

    OrbPartGenerator gen;
    OrbPartPhysics[] parts;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        gen = GetComponentInChildren<OrbPartGenerator>();
        parts = GetComponentsInChildren<OrbPartPhysics>();
	}

    public override bool UseAbility()
    {
        if (!base.UseAbility())
        {
            return false;
        }

        return gen.Disable();
    }

    public override bool UseUltimate()
    {
        if (!base.UseUltimate())
        {
            return false;
        }

        return gen.Enable();
    }

}
