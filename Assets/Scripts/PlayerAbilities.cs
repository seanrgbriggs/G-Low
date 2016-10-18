using UnityEngine;

public abstract class PlayerAbilities : MonoBehaviour {

    protected float abil_cd;
    protected float ult_cd;

    protected PlayerCar player;
    protected int id;

    protected virtual void Start()
    {
        abil_cd = 0;
        ult_cd = 0;

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
    }

    public virtual bool UseAbility()
    {
        return (abil_cd >= 1);
    }

    public virtual bool UseUltimate()
    {
        return (ult_cd >= 1);
    }

    public float GetAbilityCooldown()
    {
        return abil_cd;
    }

    public float GetUltimateCooldown()
    {
        return ult_cd;
    }
}
