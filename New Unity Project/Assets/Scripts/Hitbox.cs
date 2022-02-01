using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ShotType { chip, drive, drop, smash, special};
public class Hitbox : MonoBehaviour
{
    FighterController _self;
    [SerializeField] ShotType _shotType;
    [SerializeField] GameObject _character;
    bool _coolDown = true;
    bool _sCoolDown = true;

    List<GameObject> cooldowns = new List<GameObject>();

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
        if (!cooldowns.Contains(collision.gameObject))
        {
            var ball = collision.transform.GetComponent<ShuttleCock>();
            if (ball != null)
            {
                float facing = 1;
                if (_character.GetComponent<SpriteRenderer>().flipX)
                {
                    facing = -1;
                }

                _self.OnSuccessfulHit(collision.ClosestPointOnBounds(transform.position));

                //Play shot type depending on button pressed
                switch (_shotType)
                {
                    case ShotType.chip:
                        ball.Shoot(new Vector3(1f * facing, 2f), true, true, _self.GetFilter());
                        break;

                    case ShotType.drive:
                        ball.Shoot(new Vector3(12f * facing, 3f), true, false, _self.GetFilter());
                        break;

                    case ShotType.drop:
                        ball.Shoot(new Vector3(6f * facing, 6f), true, false, _self.GetFilter());
                        break;

                    case ShotType.smash:
                        ball.Shoot(new Vector3(16f * facing, -2f), true, false, _self.GetFilter());
                        break;

                    default:
                        Debug.Log("Fuccy Wuccy Has Occurred");
                        break;
                }
            }
            cooldowns.Add(collision.gameObject);
        }
    }

    //Leaving this for the meantime just incase the new version isnt working

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.transform == _self.transform)
    //    {
    //        return;
    //    }
    //    if (_coolDown || (!_coolDown && collision.transform.GetComponent<SubWoofer>() && _sCoolDown))
    //    {
    //        var ball = collision.transform.GetComponent<ShuttleCock>();
    //        if (ball != null)
    //        {
    //            float facing = 1;
    //            if (_character.GetComponent<SpriteRenderer>().flipX)
    //            {
    //                facing = -1;
    //            }

    //            _self.OnSuccessfulHit(collision.ClosestPointOnBounds(transform.position));

    //            //Play shot type depending on button pressed
    //            switch (_shotType)
    //            {
    //                case ShotType.chip:
    //                    ball.Shoot(new Vector3(1f * facing, 2f), true, true, _self.GetFilter());
    //                    break;

    //                case ShotType.drive:
    //                    ball.Shoot(new Vector3(12f * facing, 3f), true, false, _self.GetFilter());
    //                    break;

    //                case ShotType.drop:
    //                    ball.Shoot(new Vector3(6f * facing, 6f), true, false, _self.GetFilter());
    //                    break;

    //                case ShotType.smash:
    //                    ball.Shoot(new Vector3(16f * facing, -2f), true, false, _self.GetFilter());
    //                    break;

    //                default:
    //                    Debug.Log("Fuccy Wuccy Has Occurred");
    //                    break;
    //            }
    //        }
    //        if (!_coolDown)
    //        {
    //            _sCoolDown = false;
    //        }
    //        _coolDown = false;
    //    }
    //}

    public void SetType(ShotType type) {
        _shotType = type;
    }

    public void ResetCD()
    {
        //_coolDown = true;
        cooldowns.Clear();
    }

    public void ResetSCD()
    {
        //_sCoolDown = true;
    }
}
