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
            GanzFighter fighter = transform.root.GetComponent<GanzFighter>();
            fighter.SetOutSuit();
            fighter.StunController(0.2f);
            GameManager.Get().GetCameraShaker().SetShake(0.1f, 2.0f, true);
            //hurt._speed = 1;
            var hitMes = new HitMessage(new Vector3(axis, 5, 0), new VelocityInfluence(), true, FighterFilter.both, ShotType.chip, 1);
            hurt.SetOwner(FighterFilter.both);
            hurt.Shoot(hitMes);
            Damage();
        }
    }

    public bool IsDamaged() {
        return damaged;
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
