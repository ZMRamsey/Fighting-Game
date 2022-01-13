using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleHit : MonoBehaviour
{
    [SerializeField] GameObject _ball;

    private void OnCollisionEnter(Collision collision)
    {
        //Player hit
        if (collision.gameObject.layer == 7)
        {
            //Handle bouncing off player if done here
            //collision.gameObject.GetComponent;
            if (_ball.GetComponent<Ball>().getSpeed() >= 20)
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
