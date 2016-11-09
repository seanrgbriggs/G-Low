using UnityEngine;
using System.Collections;

public class OrbAbilities : PlayerAbilities {

	GameController gc;

    OrbPartGenerator gen;
    OrbPartPhysics[] parts;

    public float orbSpawnTimer = 3;
    float curOrbSpawnTime = 0.0f;

	public float projectile_speed = 5;

    public GameObject projectile;
    public GameObject mine;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        gen = GetComponentInChildren<OrbPartGenerator>();
        parts = GetComponentsInChildren<OrbPartPhysics>();


		gc = FindObjectOfType<GameController> ();
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
        if (!gen.Disable() && !base.UseAbility())
        {
            return false;
        }

		PlayerCar[] players = gc.getPlayers ().ToArray();

		PlayerCar target_player = null;;
		int player_index = System.Array.IndexOf (players, player);

		Vector3 proj_velocity;

		if (players.Length == 1) {
			proj_velocity = transform.forward;
		} else {

			if (player_index == 0) {
				target_player = players [1];
			} else if (player_index < players.Length) {
				target_player = players [player_index - 1];
			}


			Vector3 target_pos = target_player.transform.position + target_player.GetComponent<Rigidbody> ().velocity;
			proj_velocity = target_pos - transform.position;
		}

		GameObject proj = (GameObject)Instantiate (projectile, transform.position + transform.forward * 2, Quaternion.LookRotation (proj_velocity));
		proj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", player.base_col);
		proj.GetComponent<Rigidbody> ().velocity = proj_velocity;
		proj.GetComponent<SphereProjectile> ().SetShooter (gameObject);

		return true;
    }

    public override bool UseUltimate()
    {
        if (!gen.Disable() && !base.UseUltimate())
        {
            return false;
        }

        ((GameObject)Instantiate(mine, transform.position, transform.rotation)).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", player.base_col);
        return true;
    }

}
