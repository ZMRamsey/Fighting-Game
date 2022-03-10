using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GanzFighter : FighterController
{

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
    }

    public override void OnSuperEnd(bool instant) {
    }

    public override void OnSuperEvent() {
    }

    public override void OnFighterUpdate() {
    }

    public override void InitializeFighter() {
        base.InitializeFighter();
    }

    public override void ResetFighter() {
        base.ResetFighter();
        transform.root.GetComponentInChildren<GanzHurtbox>().Repair();
    }

}
