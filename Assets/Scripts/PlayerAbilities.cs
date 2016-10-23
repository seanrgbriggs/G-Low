using UnityEngine;

public abstract class PlayerAbilities : MonoBehaviour {

    public float abil_max;
    public float ult_max;

	private AudioSource aud_src;
	public AudioClip abil_clip;
	public AudioClip ult_clip;

    protected float abil_cd;
    protected float ult_cd;

    protected PlayerCar player;
    protected int id;

    protected virtual void Start()
    {
        abil_cd = 0;
        ult_cd = 0;

		aud_src = GetComponent<AudioSource> ();

        player = GetComponent<PlayerCar>();
        id = player.id;
    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Ability" + id))
        {
            UseAbility();
        } else if (Input.GetButtonDown("Ultimate" + id))
        {
            UseUltimate();
        }

        if (abil_cd < abil_max)
        {
            abil_cd += Time.deltaTime;
        }
        if (ult_cd < ult_max)
        {
            ult_cd += Time.deltaTime;
        }
    }

    public virtual bool UseAbility()
    {
        bool u = (abil_cd >= abil_max);
        if (u)
        {
            abil_cd = 0;
			if (aud_src != null) {
				aud_src.PlayOneShot (abil_clip);
			}
        }
        return u;
    }

    public virtual bool UseUltimate()
    {
        bool u = (ult_cd >= ult_max);
        if (u)
        {
            ult_cd = 0;
			if (aud_src != null) {
				aud_src.PlayOneShot (ult_clip);
			}
        }
        return u;
    }

    public float GetAbilityCooldown()
    {
        return abil_cd / abil_max;
    }

    public float GetUltimateCooldown()
    {
        return ult_cd / ult_max;
    }
}
