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
        if (collision.transform == _self.transform)
        {
            return;
        }
        var ball = collision.transform.GetComponent<Ball>();
        if (ball != null)
        {
            //Play shot type depending on button pressed
            switch (_shotType)
            {
                case "chip":
                    ball.Shoot(new Vector3(1, 5, 0));
                    break;

                case "drive":
                    ball.Shoot(new Vector3(15, 5, 0));
                    break;

                case "drop":
                    ball.Shoot(new Vector3(10, 10, 0));
                    break;

                case "smash":
                    ball.Shoot(new Vector3(25, -5, 0));
                    break;

                default:
                    Debug.Log("Fuccy Wuccy Has Occurred");
                    break;
            }
        }
    }
}
