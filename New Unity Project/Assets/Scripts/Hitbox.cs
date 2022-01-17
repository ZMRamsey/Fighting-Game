using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType { chip, drive, drop, smash, special};
public class Hitbox : MonoBehaviour
{
    FighterController _self;
    [SerializeField] ShotType _shotType;
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
        var ball = collision.transform.GetComponent<ShuttleCock>();
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
                case ShotType.chip:
                    ball.Shoot(new Vector3(1f * facing, 2f));
                    break;

                case ShotType.drive:
                    ball.Shoot(new Vector3(12f * facing, 3f));
                    break;

                case ShotType.drop:
                    ball.Shoot(new Vector3(6f * facing, 6f));
                    break;

                case ShotType.smash:
                    ball.Shoot(new Vector3(16f * facing, -2f));
                    break;

                default:
                    Debug.Log("Fuccy Wuccy Has Occurred");
                    break;
            }
        }
    }
}
