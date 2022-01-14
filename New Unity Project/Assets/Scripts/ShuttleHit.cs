using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleHit : MonoBehaviour
{
    Ball _self;

    private void Awake()
    {
        _self = transform.root.GetComponent<Ball>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Player hit
        if (collision.gameObject.layer == 7)
        {
            //new HitRegister(damage, new Vector3(0, 0, 0));
        }

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
            new HitRegister(_self.getSpeed(), new Vector3(0, 0, 0));
        }
    }

}
