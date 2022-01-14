using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    FighterController _self;
    [SerializeField] string _shotType;

    private void Awake()
    {
        _self = transform.root.GetComponent<FighterController>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        var ball = collision.transform.GetComponent<Ball>();
        if (ball != null && collision.transform == _self.transform)
        {
            Debug.Log("A winner is you");
            //Play shot type depending on button pressed
            switch (_shotType)
            {
                case "chip":
                    ball.Shoot();
                    break;

                case "drive":
                    ball.Shoot();
                    break;

                case "drop":
                    ball.Shoot();
                    break;

                case "smash":
                    ball.Shoot();
                    break;

                default:
                    ball.Shoot();
                    break;
            }
        }
    }
}
