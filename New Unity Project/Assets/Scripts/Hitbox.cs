using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    FighterController _self;
    [SerializeField] string _shotType = "chip";
    [SerializeField] GameObject _character;

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
            float facing = 1;
            if (_character.GetComponent<SpriteRenderer>().flipX)
            {
                facing = -1;
            }
            //Play shot type depending on button pressed
            switch (_shotType)
            {
                case "chip":
                    ball.Shoot(new Vector3(1f * facing, 2f));
                    break;

                case "drive":
                    ball.Shoot(new Vector3(12f * facing, 3f));
                    break;

                case "drop":
                    ball.Shoot(new Vector3(6f * facing, 6f));
                    break;

                case "smash":
                    ball.Shoot(new Vector3(16f * facing, -2f));
                    break;

                default:
                    Debug.Log("Fuccy Wuccy Has Occurred");
                    break;
            }
        }
    }
}
