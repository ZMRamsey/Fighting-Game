using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleHit : MonoBehaviour
{
    //[SerializeField] GameObject _ball;
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
            //Handle bouncing off player event here
            if (_self.getSpeed() >= 20)
            {
                Debug.Log("KO");
            }
            else
            {
                Debug.Log("Boink");
            }
        }

    }
   
}
