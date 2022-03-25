using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanzHurtbox : Hurtbox
{
    private bool damaged = false;
    // Start is called before the first frame update
    void Awake() {
    }

    // Update is called once per frame
    void Update() {

    }

    public override void ProcessHurt(Collider collision, ShuttleCock hurt, float axis)
    {
        if (damaged)
        {
            base.ProcessHurt(collision, hurt, axis);
        }
        else
        {
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2.0f, true);
            //hurt._speed = 1;
            var hitMes = new HitMessage(new Vector3(axis, 5, 0), new VelocityInfluence(), false, FighterFilter.both, ShotType.chip);
            hurt.SetOwner(FighterFilter.both);
            hurt.Shoot(hitMes);
            Damage();
        }
    }

    public void Damage()
    {
        damaged = true;
    }

    public void Repair()
    {
        damaged = false;
    }
}
