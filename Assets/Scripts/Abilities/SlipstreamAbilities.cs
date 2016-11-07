using UnityEngine;
using System.Collections;

public class SlipstreamAbilities : PlayerAbilities {

	public float roll_power;
	public float max_boost_power;

	public float isolation_range;

	GameController gc;
	Rigidbody rb;

    public GameObject dashParticles;

    private float thrustBase;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		gc = FindObjectOfType<GameController> ();
		rb = GetComponent<Rigidbody> ();

        thrustBase = GetComponent<PlayerCar>().thrustPower;

	}

	protected override void Update(){
		base.Update ();
		boostIfAlone ();
	}

	public override bool UseAbility() //Magnetize
	{
		if (!base.UseAbility())
		{
			return false;
		}
        
		return Roll(-roll_power);

	}

	public override bool UseUltimate()
	{
		if (!base.UseAbility())
		{
			return false;
        }

        return Roll(roll_power); 
	}

	// Update is called once per frame
	bool Roll (float power) {
        power *= FindObjectOfType<GameController>().playerSpeedMultiplier;

        ParticleSystem p = SpawnParticles(dashParticles).GetComponent<ParticleSystem>();
        p.startRotation3D = transform.eulerAngles * Mathf.Deg2Rad;

        rb.AddForce (transform.right * power, ForceMode.VelocityChange);
		return true;
	}

	void boostIfAlone(){
		foreach (PlayerCar p in gc.getPlayers()) {
			if (Vector3.Distance (transform.position, p.transform.position) <= isolation_range && p != player) {
				ult_cd = 0;
				return;
			}
		}

        GetComponent<PlayerCar>().thrustPower = thrustBase + max_boost_power * ult_cd / ult_max;
        //rb.AddForce (max_boost_power * ult_cd / ult_max * transform.forward, ForceMode.Acceleration);
    }
}
