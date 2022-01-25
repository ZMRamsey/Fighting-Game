using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleHit : MonoBehaviour
{
    ShuttleCock _self;

    private void Awake()
    {
        _self = transform.root.GetComponent<ShuttleCock>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform == _self.transform)
        {
            return;
        }
        var hurt = collision.transform.GetComponent<Hurtbox>();
        if (hurt != null)
        {
            new HitRegister(_self.GetSpeedPercent(), new Vector3(0, 0, 0));
        }
        if (collision.gameObject.layer == 10)
        {
            //point over stuff
            if (_self.transform.position.x > 0)
            {
                //give point to one side
            }
            else
            {
                //give point to other side
            }
            //shouldnt be any issue since the net will take up x = 0
        }
    }
}
