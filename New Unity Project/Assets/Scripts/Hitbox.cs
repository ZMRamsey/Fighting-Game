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

    public List<GameObject> cooldowns = new List<GameObject>();

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

                InputHandler handler = _self.GetComponent<InputHandler>();
                //Play shot type depending on button pressed
                switch (_shotType)
                {
                    case ShotType.chip:
                        ball.Shoot(new Vector3(1f * facing, 2f), handler.GetInput(), true, true, _self.GetFilter(), _self);
                        break;

                    case ShotType.drive:
                        ball.Shoot(new Vector3(12f * facing, 3f), handler.GetInput(), true, false, _self.GetFilter(), _self);
                        break;

                    case ShotType.drop:
                        ball.Shoot(new Vector3(6f * facing, 6f), handler.GetInput(), true, false, _self.GetFilter(), _self);
                        break;

                    case ShotType.smash:
                        ball.Shoot(new Vector3(16f * facing, -2f), handler.GetInput(), true, false, _self.GetFilter(), _self);
                        break;

                    default:
                        Debug.Log("Fuccy Wuccy Has Occurred");
                        break;
                }
            }
            if (_self.GetFilter() != GameManager.Get().GetLastHit())
            {
                GameManager.Get().IncreaseRally();
                GameManager.Get().SetLastHitter(_self.GetFilter());
                //Debug.Log("Rally:" + GameManager.Get()._rally + " - Last Hit By:" + GameManager.Get().GetLastHit().ToString());
            }
            cooldowns.Add(collision.gameObject);
        }
    }

    public void SetType(ShotType type) {
        _shotType = type;
    }

    public void ResetCD()
    {
        cooldowns.Clear();
    }
}
